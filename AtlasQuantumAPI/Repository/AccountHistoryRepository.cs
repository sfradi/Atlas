using System;
using System.Collections.Generic;
using AtlasQuantumAPI.Data;
using AtlasQuantumAPI.Domain;
using Dapper;

namespace AtlasQuantumAPI.Repository
{
    public class AccountHistoryRepository : RepositoryBase, IAccountHistoryRepository
    {
        public AccountHistoryRepository(IDataContext context) : base(context)
        {
        }

        public bool Add(AccountEntity acc, bool cacheOnly = false)
        {
            throw new NotImplementedException();
        }

        public bool Add(AccountHistoryEntity acc, bool cacheOnly = false)
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public bool Delete(Guid id, bool cacheOnly = false)
        {
            throw new NotImplementedException();
        }

        public bool Deposit(Guid id, decimal amount)
        {
            throw new NotImplementedException();
        }

        public bool Exist(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<AccountHistoryEntity> GetAll()
        {
            string sql = @"SELECT * FROM ACCOUNT_HISTORY";

            var result = new List<AccountHistoryEntity>();

            using (var connection = DataContext.CreateConnection())
            {
                connection.Open();

                IEnumerable<AccountHistoryEntity> accountItens = connection.Query<AccountHistoryEntity>(sql);

                foreach (var item in accountItens)
                {
                    result.Add(item);
                }

                return result;
            }
        }

        public List<AccountEntity> GetAllAccountHistory()
        {
            throw new NotImplementedException();
        }

        public List<AccountEntity> GetById(Guid Id)
        {
            throw new NotImplementedException();
        }

        public bool Withdraw(Guid id, decimal amount)
        {
            throw new NotImplementedException();
        }

        List<AccountHistoryEntity> IAccountHistoryRepository.GetAllAccountHistory()
        {
            throw new NotImplementedException();
        }

        List<AccountHistoryEntity> IAccountHistoryRepository.GetById(Guid Id)
        {
            throw new NotImplementedException();
        }
    }
}
