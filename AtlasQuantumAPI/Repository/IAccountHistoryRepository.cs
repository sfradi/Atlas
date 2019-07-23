using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtlasQuantumAPI.Domain;

namespace AtlasQuantumAPI.Repository
{
    public interface IAccountHistoryRepository
    {
        List<AccountHistoryEntity> GetAll();

        List<AccountHistoryEntity> GetAllAccountHistory();

        List<AccountHistoryEntity> GetById(Guid Id);

        bool Add(AccountHistoryEntity acc, bool cacheOnly = false);

        bool Delete(Guid id, bool cacheOnly = false);

        bool Withdraw(Guid id, decimal amount);

        bool Deposit(Guid id, decimal amount);

        bool Exist(Guid id);

        int Count();


    }
}
