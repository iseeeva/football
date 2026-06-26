using System.Security.Cryptography;
using System.Text;

// Token: 0x02000009 RID: 9
public static class GClass100
{
    // Token: 0x06000063 RID: 99 RVA: 0x000034F4 File Offset: 0x000016F4
    public static void smethod_0(string string_0, string string_1, string string_2)
    {
        FileStream fileStream = File.OpenRead(string_0);
        byte[] array = new byte[fileStream.Length];
        fileStream.Read(array, 0, array.Length);
        fileStream.Close();
        FileStream fileStream2 = new FileStream(string_1, FileMode.OpenOrCreate, FileAccess.Write);
        CryptoStream cryptoStream = new CryptoStream(fileStream2, new DESCryptoServiceProvider
        {
            Key = Encoding.ASCII.GetBytes(string_2),
            IV = Encoding.ASCII.GetBytes(string_2)
        }.CreateEncryptor(), CryptoStreamMode.Write);
        cryptoStream.Write(array, 0, array.Length);
        cryptoStream.Close();
        fileStream2.Close();
    }

    // Token: 0x06000064 RID: 100 RVA: 0x00003580 File Offset: 0x00001780
    public static void smethod_1(string string_0, string string_1, string string_2)
    {
        FileStream fileStream = new FileStream(string_0, FileMode.Open, FileAccess.Read);
        CryptoStream cryptoStream = new CryptoStream(fileStream, new DESCryptoServiceProvider
        {
            Key = Encoding.ASCII.GetBytes(string_2),
            IV = Encoding.ASCII.GetBytes(string_2)
        }.CreateDecryptor(), CryptoStreamMode.Read);
        byte[] array = new byte[fileStream.Length];
        cryptoStream.Read(array, 0, array.Length);
        cryptoStream.Close();
        fileStream.Close();
        FileStream fileStream2 = new FileStream(string_1, FileMode.OpenOrCreate, FileAccess.Write);
        fileStream2.Write(array, 0, array.Length);
        fileStream2.Close();
    }

    // Token: 0x06000065 RID: 101 RVA: 0x00003610 File Offset: 0x00001810
    public static byte[] smethod_2(string string_0, string string_1)
    {
        FileStream fileStream = new FileStream(string_0, FileMode.Open, FileAccess.Read);
        CryptoStream cryptoStream = new CryptoStream(fileStream, new DESCryptoServiceProvider
        {
            Key = Encoding.ASCII.GetBytes(string_1),
            IV = Encoding.ASCII.GetBytes(string_1)
        }.CreateDecryptor(), CryptoStreamMode.Read);
        byte[] array = new byte[fileStream.Length];
        cryptoStream.Read(array, 0, array.Length);
        cryptoStream.Close();
        fileStream.Close();
        return array;
    }

    // Token: 0x06000066 RID: 102 RVA: 0x00003684 File Offset: 0x00001884
    public static byte[] smethod_3(string string_0)
    {
        FileStream fileStream = new FileStream(string_0, FileMode.Open, FileAccess.Read);
        byte[] array = new byte[fileStream.Length];
        fileStream.Read(array, 0, array.Length);
        fileStream.Close();
        return array;
    }
}
