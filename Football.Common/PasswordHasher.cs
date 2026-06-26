using System.Security.Cryptography;

namespace Football.Common
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100_000;

        public static string Hash(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);

            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public static bool Verify(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
                return false;

            var parts = storedHash.Split('.', 3);
            if (parts.Length != 3) return false;
            if (!int.TryParse(parts[0], out int iterations)) return false;

            byte[] salt, expectedHash;
            try
            {
                salt = Convert.FromBase64String(parts[1]);
                expectedHash = Convert.FromBase64String(parts[2]);
            }
            catch (FormatException)
            {
                return false;
            }

            byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expectedHash.Length);
            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
    }
}