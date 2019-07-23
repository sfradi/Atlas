using System;
using System.Collections.Generic;
using System.Data;
using AtlasQuantumAPI.Data;
using AtlasQuantumAPI.Domain;
using Dapper;

namespace AtlasQuantumAPI.Repository
{
    public class AccountRepository : RepositoryBase, IAccountRepository
    {
        public AccountRepository(IDataContext context) : base(context)
        {

        }

        public bool Add(AccountEntity acc, bool cacheOnly = false)
        {
            if (cacheOnly)
                return true;

            string sql = @"INSERT INTO ACCOUNT (AccountId, PersonName, Amount, Ballance) VALUES (@AccountId, @PersonName, @Amount, @Ballance)";

            using (var connection = DataContext.CreateConnection())
            {
                connection.Open();

                var parameters = new DynamicParameters();

                parameters.Add("@AccountId", acc.AccountId.ToByteArray(), DbType.Binary);
                parameters.Add("@PersonName", acc.PersonName, DbType.String);
                parameters.Add("@Amount", acc.Amount, DbType.Decimal);
                parameters.Add("@Ballance", acc.Ballance, DbType.Decimal);

                var result = connection.Execute(sql, parameters);

                return result == 1;

            }

        }

        public bool CachedDeposit(Guid id, decimal amount, decimal lastMove, bool cacheOnly = false)
        {
            if (cacheOnly)
                return true;

            string sql = @"UPDATE ACCOUNT SET Amount = @Amount , Ballance = @Ballance WHERE AccountId = @AccountId";

            using (var connection = DataContext.CreateConnection())
            {
                connection.Open();

                var parameters = new DynamicParameters();

                parameters.Add("@AccountId", id.ToByteArray(), DbType.Binary);
                parameters.Add("@Amount", amount, DbType.Decimal);
                parameters.Add("@Ballance", amount, DbType.Decimal);

                var result = connection.Execute(sql, parameters);

                if (result == 1) { AccountHistory(id, id, lastMove, "D"); }

                return result == 1;

            }
        }

        public bool CachedWithdrawal(Guid id, decimal amount, decimal lastMove, bool cacheOnly = false)
        {        
          
            if (cacheOnly)
                return true;

            string sql = @"UPDATE ACCOUNT SET Amount = @Amount , Ballance = @Ballance WHERE AccountId = @AccountId";

            using (var connection = DataContext.CreateConnection())
            {
                connection.Open();

                var parameters = new DynamicParameters();

                parameters.Add("@AccountId", id.ToByteArray(), DbType.Binary);
                parameters.Add("@Amount", amount, DbType.Decimal);
                parameters.Add("@Ballance", amount, DbType.Decimal);

                var result = connection.Execute(sql, parameters);

                if (result == 1) { AccountHistory(id, id, lastMove, "W"); }

                return result == 1;

            }
        }

        public void AccountHistory(Guid accountId, Guid anotherAccountId, decimal lastMove, string type)
        {
            
            Guid TransitionId = Guid.NewGuid();

            string sql = @"INSERT INTO ACCOUNT_HISTORY (TransitionId, AccountId, Amount, Type, ReceivedFrom, OccurDate) VALUES (@TransitionId, @AccountId, @Amount, @Type, @ReceivedFrom, @OccurDate)";

            using (var connection = DataContext.CreateConnection())
            {
                connection.Open();

                var parameters = new DynamicParameters();

                parameters.Add("@TransitionId", TransitionId.ToByteArray(), DbType.Binary);
                parameters.Add("@AccountId", accountId.ToByteArray(), DbType.Binary);
                parameters.Add("@Amount", lastMove, DbType.Decimal);
                parameters.Add("@Type", type, DbType.String);
                parameters.Add("@ReceivedFrom", anotherAccountId.ToByteArray(), DbType.Binary);
                parameters.Add("@OccurDate", DateTime.Now.ToLongDateString(), DbType.DateTime);

                var result = connection.Execute(sql, parameters);
                               
                var cachedHistory = Startup.container.GetInstance<CachedAccountHistoryRepository>();
                cachedHistory.Add(new AccountHistoryEntity { AccountId = accountId, Amount = lastMove, OccurDate = DateTime.Now, ReceivedFrom = accountId, TransitionId = TransitionId , Type = type }, true);


            }

        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public bool Delete(Guid id, bool cacheOnly = false)
        {
            throw new NotImplementedException();
        }

        public bool Deposit(Guid id, decimal amountC, bool cacheOnly = false)
        {
            throw new NotImplementedException();
        }

        public bool Exist(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<AccountEntity> GetAll()
        {
            string sql = @"SELECT * FROM ACCOUNT";

            var result = new List<AccountEntity>();

            using (var connection = DataContext.CreateConnection())
            {
                connection.Open();

                IEnumerable<AccountEntity> accountItens = connection.Query<AccountEntity>(sql);

                foreach (var item in accountItens)
                {
                    result.Add(item);
                }

                return result;
            }


        }

        public List<AccountEntity> GetById(Guid Id)
        {
            string sql = @"SELECT * FROM ACCOUNT WHERE AccountId = @AccountId";

            var result = new List<AccountEntity>();

            using (var connection = DataContext.CreateConnection())
            {
                connection.Open();

                var param = new DynamicParameters();
                param.Add(":AccountId");

                IEnumerable<AccountEntity> accountItens = connection.Query<AccountEntity>(sql, param);

                foreach (var item in accountItens)
                {
                    result.Add(item);
                }

                return result;
            }
        }

        public int Withdrawal(Guid id, decimal amount)
        {
            throw new NotImplementedException();
        }

        List<AccountEntity> IAccountRepository.GetAllAccountHistory()
        {
            string sql = @"SELECT * FROM ACCOUNT_HISTORY";

            var result = new List<AccountEntity>();

            using (var connection = DataContext.CreateConnection())
            {
                connection.Open();

                IEnumerable<AccountEntity> accountItens = connection.Query<AccountEntity>(sql);

                foreach (var item in accountItens)
                {
                    result.Add(item);
                }

                return result;
            }
        }
    }
}
