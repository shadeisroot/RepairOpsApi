
using Microsoft.EntityFrameworkCore;
using RepairOpsApi.DataAccess;
using RepairOpsApi.DataAccess.Repository.Interfaces;
using RepairOpsApi.Entities;

namespace RepairOpsApi.DataAccess.Repository;

public class UserRepository : IUserRepository
{
    private readonly RepairOpsApiContext _context;

    public UserRepository(RepairOpsApiContext context)
    {
        _context = context;
    }

    //henter en user ud fra username
    public User GetUserByUsername(string username)
    {
        return _context.Users.FirstOrDefault(u => u.username == username);
    }
    
    //tilføjer en user til databasen
    public void RegisterUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }
    
    //henter alle users
    public List<User> GetAllUsers()
    {
        return _context.Users.ToList();
    }
}