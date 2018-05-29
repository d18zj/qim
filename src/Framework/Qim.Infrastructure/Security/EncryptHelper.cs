using System;
using System.Security.Cryptography;
using System.Text;

namespace Qim.Security
{
    public static class EncryptHelper
    {
        public static string CreateSaltKey(int size = 16)
        {
            Ensure.Positive(size, "size");
            byte[] salt = new byte[size];
            // Generate a cryptographic random number
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Return a Base64 string representation of the random number
            return Convert.ToBase64String(salt).Substring(0, size);
        }

        public static string Hash(string text, HashType type = HashType.Md5)
        {
            Ensure.NotNullOrEmpty(text, "text");

            using (var hash = GetHashAlgorithm(type))
            {
                // Send a sample text to hash.
                var hashedBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(text));

                // Get the hashed string.
                return Convert.ToBase64String(hashedBytes);
            }

        }

      

        #region private

        private static HashAlgorithm GetHashAlgorithm(HashType type)
        {
            switch (type)
            {
                case HashType.Md5:
                    return MD5.Create();
                case HashType.Sha256:
                    return SHA256.Create();
                case HashType.Sha512:
                    return SHA512.Create();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        #endregion
    }
}