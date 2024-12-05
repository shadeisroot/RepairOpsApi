using RepairOpsApi.BusinessLogic.Interfaces;
using RepairOpsApi.DataAccess.Repository.Interfaces;
using RepairOpsApi.Entities;
using RepairOpsApi.Helpers.Interfaces;

namespace RepairOpsApi.BusinessLogic;

public class UserLogic : IUserLogic
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordEncrypter _passwordEncrypter;

    public UserLogic(IUserRepository userRepository, IPasswordEncrypter passwordEncrypter)
    {
        _userRepository = userRepository;
        _passwordEncrypter = passwordEncrypter;
    }

    public User RegisterUser(string username, string password)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentException("Username cannot be null or empty.");
        }

        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Password cannot be null or empty.");
        }
            
        var existingUser = _userRepository.GetUserByUsername(username);
        if (existingUser != null)
        {
            throw new ArgumentException("Username is already taken.");
        }
        
        var (hash, salt) = _passwordEncrypter.EncryptPassword(password);
        
        var user = new User
        {
            username = username,
            Hash = hash,
            Salt = salt,
        };
        
        _userRepository.RegisterUser(user);

        return user;
    }
    public User LoginUser(string username, string password)
    {
        if (string.IsNullOrEmpty(username))
            throw new ArgumentException("Username cannot be null or empty.");

        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password cannot be null or empty.");
        
        var user = _userRepository.GetUserByUsername(username);
        if (user == null)
            throw new ArgumentException("Invalid username or password.");
        
        //benytter brugerens salt i stedet for at lave en random for at få det samme hash til at tjekke om PW er ens
        var hashWithStoredSalt = _passwordEncrypter.EncryptPasswordWithUsersSalt(password, user.Salt);
        
        
        if (hashWithStoredSalt != user.Hash)
            throw new ArgumentException("Invalid username or password.");

        return user;
    }

    public List<User> GetAllUsers()
    {
        return _userRepository.GetAllUsers();
        
    }
}