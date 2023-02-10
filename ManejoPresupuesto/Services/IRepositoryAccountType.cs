using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Services
{
    public interface IRepositoryAccountType
    {
        Task Create(AccountType accountType);
        Task<bool> Exits(string name, int userId);
        Task<IEnumerable<AccountType>> GetUserId(int userId);

        Task Update(AccountType accountType);

        Task<AccountType> GetAccountId(int id, int userId);
        Task Delete(int id);
    }
}
