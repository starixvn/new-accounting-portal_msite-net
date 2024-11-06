using System.Security.Cryptography;
using System.Text;

namespace Maintenance.Mail.Services;

public static class Cryptography
{
    #region Const Params

    private const string CfsKey = "&％#@?,:*!~^.";

    #endregion

    #region Public functions

    public static string CfsEnCrypt(string plainText)
    {
        var rgbKey = new byte[] { 0x21, 0x63, 0x54, 0x5a, 0x91, 0xac, 110, 0xcc };
        var rgbIV = Encoding.UTF8.GetBytes(CfsKey);
        var provider = new DESCryptoServiceProvider();
        var bytes = Encoding.UTF8.GetBytes(plainText);
        using var stream = new MemoryStream();
        using var stream2 = new CryptoStream(stream, provider.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
        stream2.Write(bytes, 0, bytes.Length);
        stream2.FlushFinalBlock();
        var encryptedText = Convert.ToBase64String(stream.ToArray());

        return encryptedText;
    }

    #endregion
}