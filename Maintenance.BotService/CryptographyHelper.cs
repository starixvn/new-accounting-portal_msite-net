namespace Maintenance.BotService
{
    using System.Security.Cryptography;
    using System.Text;

    public static class CryptographyHelper
    {
        private static readonly DESCryptoServiceProvider CryptoServiceProvider = new DESCryptoServiceProvider();

        private static readonly byte[] RgbKey = [33, 99, 84, 90, 145, 172, 110, 204];

        private static readonly byte[] RgbIv = Encoding.UTF8.GetBytes("&％#@?,:*!~^.");

        public static string EncryptPassword(string plainText)
        {
            ArgumentNullException.ThrowIfNull(plainText);

            using ICryptoTransform transform = CryptoServiceProvider.CreateEncryptor(RgbKey, RgbIv);
            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream = new(memoryStream, transform, CryptoStreamMode.Write);
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public static string DecryptPassword(string encryptedPassword)
        {
            ArgumentNullException.ThrowIfNull(encryptedPassword);

            using MemoryStream stream = new(Convert.FromBase64String(StandardizeEncryptedPasswordLength(encryptedPassword)));
            using ICryptoTransform transform = CryptoServiceProvider.CreateDecryptor(RgbKey, RgbIv);
            using StreamReader streamReader = new(new CryptoStream(stream, transform, CryptoStreamMode.Read));
            return streamReader.ReadToEnd().Trim();
        }

        private static string StandardizeEncryptedPasswordLength(string encryptedPassword)
        {
            string result = string.Empty;
            if (!string.IsNullOrWhiteSpace(encryptedPassword))
            {
                result = encryptedPassword[..(encryptedPassword.Length >> 2 << 2)];
            }

            return result;
        }
    }
}