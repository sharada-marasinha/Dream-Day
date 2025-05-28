using DreamDay.BLL.Services.Implementations;
using DreamDay.BLL.Services.Interfaces;
using DreamDay.Models.ViewModels;
using DreamDay.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Web;

namespace DreamDay.UI.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public AuthController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [HttpGet("login")]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = await _userService.GetUserByEmailAsync(vm.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid credentials.");
                return View(vm);
            }

            bool passwordValid = PasswordUtility.VerifyPassword(vm.Password, user.Password);
            if (!passwordValid)
            {
                ModelState.AddModelError("", "Invalid credentials.");
                return View(vm);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect(returnUrl);
        }

        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet("accessdenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("forgot-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = await _userService.GetUserByEmailAsync(vm.Email);
            if (user == null)
            {
                // To avoid email enumeration, show generic message
                TempData["Message"] = "If the email exists, a reset link has been sent.";
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // Generate secure token
            var token = Guid.NewGuid().ToString("N");
            var expiry = DateTime.UtcNow.AddHours(1);

            await _userService.SetPasswordResetTokenAsync(user.Id, token, expiry);

            // Generate reset link
            var resetLink = Url.Action("ResetPassword", "Auth", new { token = HttpUtility.UrlEncode(token) }, Request.Scheme);

            // Send Email
            string subject = "Reset Your Password";
            string body = $"Please reset your password by clicking <a href='{resetLink}'>here</a>. The link expires in 1 hour.";

            await _emailService.SendEmailAsync(user.Email, subject, body);

            TempData["Message"] = "If the email exists, a reset link has been sent.";
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        [HttpGet("forgot-password-confirmation")]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        [HttpGet("reset-password")]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }

            var vm = new ResetPasswordViewModel { Token = token };
            return View(vm);
        }

        [HttpPost("reset-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = await _userService.GetUserByResetTokenAsync(vm.Token);

            if (user == null || user.PasswordResetTokenExpiry == null || user.PasswordResetTokenExpiry < DateTime.UtcNow)
            {
                ModelState.AddModelError("", "Invalid or expired reset token.");
                return View(vm);
            }

            // Hash new password
            string hashedPassword = PasswordUtility.HashPassword(vm.NewPassword);

            await _userService.UpdatePasswordAsync(user.Id, hashedPassword);
            await _userService.ClearPasswordResetTokenAsync(user.Id);

            TempData["Message"] = "Password has been reset successfully.";
            return RedirectToAction("Login");
        }

    }
}

