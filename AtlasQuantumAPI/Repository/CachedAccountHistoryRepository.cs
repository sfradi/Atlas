using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AtlasQuantumAPI.Domain;
using Serilog;

namespace AtlasQuantumAPI.Repository
{
    public class CachedAccountHistoryRepository : IAccountHistoryRepository
    {
        private static ConcurrentDictionary<Guid, AccountHistoryEntity> cache;

        private readonly IAccountHistoryRepository repository;
        public readonly ILogger logger;

        public CachedAccountHistoryRepository(IAccountHistoryRepository repository)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public bool Add(AccountHistoryEntity acc, bool cacheOnly = false)
        {
            return cache.TryAdd(acc.TransitionId, acc);
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
            if (cache == null || cache.Count == 0)
            {
                var entities = repository.GetAll();
                cache = new ConcurrentDictionary<Guid, AccountHistoryEntity>(entities.ToDictionary(e => e.TransitionId));
            }

            return cache.Select(x => x.Value).ToList();
        }

        public List<AccountHistoryEntity> GetAllAccountHistory()
        {
            throw new NotImplementedException();
        }

        public List<AccountHistoryEntity> GetById(Guid Id)
        {
            if (cache == null)
            {
                var entities = repository.GetAll();
                cache = new ConcurrentDictionary<Guid, AccountHistoryEntity>(entities.ToDictionary(e => e.TransitionId));
            }


            return cache.Where(x => x.Value.AccountId == Id).Select(x => x.Value).ToList();
        }

        public bool Withdraw(Guid id, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
