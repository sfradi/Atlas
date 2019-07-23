using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtlasQuantumAPI.Domain;

namespace AtlasQuantumAPI.Repository
{
    public interface IAccountRepository
    {

        List<AccountEntity> GetAll();

        List<AccountEntity> GetAllAccountHistory();

        List<AccountEntity> GetById(Guid Id);

        bool Add(AccountEntity acc, bool cacheOnly = false);

        bool Delete(Guid id, bool cacheOnly = false);

        int Withdrawal(Guid id, decimal amount);

        bool CachedWithdrawal(Guid id, decimal amount, decimal lastAmount, bool cacheOnly = false);

        bool Deposit(Guid id, decimal amount,  bool cacheOnly = false);

        bool CachedDeposit(Guid id, decimal amount, decimal lastAmount, bool cacheOnly = false);

        bool Exist(Guid id);

        int Count();
    }
}
