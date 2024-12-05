
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

    public User GetUserByUsername(string username)
    {
        return _context.Users.FirstOrDefault(u => u.username == username);
    }
    
    public void RegisterUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }
    
    public List<User> GetAllUsers()
    {
        return _context.Users.ToList();
    }
}