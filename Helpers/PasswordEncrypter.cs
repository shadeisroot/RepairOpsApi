using System.Security.Cryptography;
using System.Text;
using RepairOpsApi.Helpers.Interfaces;

namespace RepairOpsApi.Helpers;

public class PasswordEncrypter : IPasswordEncrypter
{
    
    private readonly int _keySize;
    private readonly int _iterations;
    private readonly HashAlgorithmName _algorithm;

    public PasswordEncrypter(int keySize = 64, int iterations = 10000)
    {
        _keySize = keySize;
        _iterations = iterations;
        _algorithm = HashAlgorithmName.SHA512;
    }

    public (string Hash, string Salt) EncryptPassword(string password)
    {
        // generer salt, random tal ud fra keysize (64)
        var salt = RandomNumberGenerator.GetBytes(_keySize);

        // hasher password med salt
        var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, _iterations, _algorithm, _keySize);

        // returner hash og salt
        return (Convert.ToHexString(hash), Convert.ToHexString(salt));
    }

    public string EncryptPasswordWithUsersSalt(string password, string saltHex)
    {
        var salt = Convert.FromHexString(saltHex);
        var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, _iterations, _algorithm, _keySize);
        return Convert.ToHexString(hash);
    }
}
