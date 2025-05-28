using DreamDay.BLL.Services.Interfaces;
using DreamDay.Models.Entities;
using DreamDay.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DreamDay.UI.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        [HttpGet("details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpGet("create")]
        public IActionResult Create() => View();

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = new User
            {
                FullName = vm.FullName,
                Email = vm.Email,
                Password = vm.Password,
                Role = string.IsNullOrEmpty(vm.Role) ? "User" : vm.Role,
                CreatedAt = DateTime.UtcNow
            };

            await _userService.CreateUserAsync(user);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            var vm = new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Password = string.Empty // Do not populate password in edit view for security
            };
            return View(vm);
        }

        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserViewModel vm)
        {
            if (id != vm.Id) return BadRequest();

            if (!ModelState.IsValid) return View(vm);

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            user.FullName = vm.FullName;
            user.Email = vm.Email;
            //if (!string.IsNullOrWhiteSpace(vm.Password))
            //{
            //    user.Password = vm.Password;
            //}
            user.Password = vm.Password;
            user.Role = string.IsNullOrEmpty(vm.Role) ? user.Role : vm.Role;

            await _userService.UpdateUserAsync(user);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("delete/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userService.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
