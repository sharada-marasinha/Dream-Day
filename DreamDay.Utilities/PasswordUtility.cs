using System;
using System.Security.Cryptography;
using System.Text;

namespace DreamDay.Utilities
{
    public static class PasswordUtility
    {
        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32;  // 256 bit
        private const int Iterations = 10000; // Adjust based on performance/security balance

        /// <summary>
        /// Creates a hashed password with a salt.
        /// Returns a combined string with salt + hash for storage.
        /// </summary>
        /// <param name="password">Plain text password</param>
        /// <returns>Base64 encoded salt+hash</returns>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be empty", nameof(password));

            using var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[SaltSize];
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] key = pbkdf2.GetBytes(KeySize);

            var hashBytes = new byte[SaltSize + KeySize];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, SaltSize);
            Buffer.BlockCopy(key, 0, hashBytes, SaltSize, KeySize);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Verifies if the provided password matches the stored hashed password.
        /// </summary>
        /// <param name="password">Plain text password</param>
        /// <param name="hashedPassword">Stored Base64 encoded salt+hash</param>
        /// <returns>True if verified; otherwise, false</returns>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password)) return false;
            if (string.IsNullOrEmpty(hashedPassword)) return false;

            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            if (hashBytes.Length != SaltSize + KeySize)
                return false;

            var salt = new byte[SaltSize];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] key = pbkdf2.GetBytes(KeySize);

            for (int i = 0; i < KeySize; i++)
            {
                if (hashBytes[i + SaltSize] != key[i])
                    return false;
            }
            return true;
        }
    }
}
