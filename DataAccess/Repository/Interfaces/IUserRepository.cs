using RepairOpsApi.Entities;

namespace RepairOpsApi.DataAccess.Repository.Interfaces;

public interface IUserRepository
{
   
    User GetUserByUsername(string username);
    void RegisterUser(User user);
    
    List<User> GetAllUsers();
}