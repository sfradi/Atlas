using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtlasQuantumAPI.Domain
{
    public class AccountHistoryEntity
    {
        
        public Guid AccountId { get; set; }

        public Guid TransitionId { get; set; }

        public string Type { get; set; }

        public decimal Amount { get; set; }

        public Guid ReceivedFrom { get; set; }

        public DateTime OccurDate { get; set; }

    }
}
