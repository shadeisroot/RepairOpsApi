using RepairOpsApi.Entities;

namespace RepairOpsApi.BusinessLogic.Interfaces;

public interface IUserLogic
{
    User LoginUser(string username, string password);
}