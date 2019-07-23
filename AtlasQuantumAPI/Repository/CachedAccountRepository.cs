using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using AtlasQuantumAPI.Domain;

namespace AtlasQuantumAPI.Repository
{
    public class CachedAccountRepository : IAccountRepository
    {
        private static ConcurrentDictionary<Guid, AccountEntity> cache;

        private readonly IAccountRepository repository;
        public readonly Serilog.ILogger logger;

        public CachedAccountRepository(IAccountRepository repository)
        {
            this.repository = repository;
            //this.logger = logger;
        }

        public bool Add(AccountEntity acc, bool cacheOnly = false)
        {

            if (!repository.Add(acc, cacheOnly))
            {
                logger.Information("AccountId added to cache but failed to add to the database. AccountId: {@AccountId}", acc.AccountId);
                return false;
            }
            else
            {
                cache.TryAdd(acc.AccountId, acc);
            }
            return true;

        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public bool Delete(Guid id, bool cacheOnly = false)
        {
            throw new NotImplementedException();
        }

        public bool Exist(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<AccountEntity> GetAll()
        {
            if (cache == null || cache.Count == 0)
            {
                var entities = repository.GetAll();
                cache = new ConcurrentDictionary<Guid, AccountEntity>(entities.ToDictionary(e => e.AccountId));
            }

            return cache.Select(x => x.Value).ToList();

        }

        public List<AccountEntity> GetById(Guid Id)
        {
            return cache.Where(x => x.Value.AccountId == Id).Select(x => x.Value).ToList();
        }

        public int Withdrawal(Guid id, decimal amount)
        {
            decimal lastMove = 0;
            AccountEntity ae;
            var acc = cache.Where(x => x.Value.AccountId == id).Select(x => x.Value).ToList();

            cache.TryGetValue(id, out ae);

            if (acc.Count > 0)
            {

                if (amount > ae.Amount) //if the amout to withdrawal is > that the Account Amount, return false and Logg a fail            
                    //logger.Information($"Cashout {amount} value is greater than account value {ae.Amount}");
                    return 2;

                lastMove = amount;
                ae.Amount -= amount; //Summary amount to amount cached
                ae.Ballance = ae.Amount;

                if (cache.TryUpdate(id, ae, ae))
                {
                    if (repository.CachedWithdrawal(acc[0].AccountId, (ae.Amount), lastMove, false))
                    {
                        return 1;
                    }
                    else
                    {
                        //Restore Amount if is not possible Update the database
                        ae.Amount += amount;
                        ae.Ballance = ae.Amount;
                        cache.TryUpdate(id, ae, ae);
                        return 0;
                    }
                }
                return 0;

            }

            return 0;
        }

        public List<AccountEntity> GetAllAccountHistory()
        {
            if (cache == null)
            {
                var entities = repository.GetAllAccountHistory();
                cache = new ConcurrentDictionary<Guid, AccountEntity>(entities.ToDictionary(e => e.AccountId));
            }

            return cache.Select(x => x.Value).ToList();
        }

        public bool Deposit(Guid id, decimal amount, bool cacheOnly = false)
        {
            decimal lastMove = 0;
            AccountEntity ae;
            AccountHistoryEntity aeh;
            var acc = cache.Where(x => x.Value.AccountId == id).Select(x => x.Value).ToList();

            cache.TryGetValue(id, out ae);

            if (acc.Count > 0)
            {
                lastMove = amount;
                ae.Amount += amount; //Summary amount to amount cached
                ae.Ballance = ae.Amount;

                if (cache.TryUpdate(id, ae, ae))
                {
                    if (repository.CachedDeposit(acc[0].AccountId, (ae.Amount), lastMove, false))
                    {
                        return true;
                    }
                    else
                    {
                        //Restore Amount if is not possible Update the database
                        ae.Amount -= amount;
                        ae.Ballance = ae.Amount;
                        cache.TryUpdate(id, ae, ae);
                        return false;
                    }
                }
                return false;

            }

            return false;
        }

        public bool CachedDeposit(Guid id, decimal amount, decimal lastMove, bool cacheOnly = false)
        {
            return repository.CachedDeposit(id, amount, lastMove, cacheOnly);
            
        }

        public bool CachedWithdrawal(Guid id, decimal amount, decimal lastMove, bool cacheOnly = false)
        {
           
            return repository.CachedWithdrawal(id, amount, lastMove, cacheOnly);
        }
    }
}
