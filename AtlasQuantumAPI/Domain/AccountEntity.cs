using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtlasQuantumAPI.Domain
{
    public class AccountEntity
    {

        public Guid AccountId { get; set; }

        public string PersonName { get; set; }

        public decimal Amount { get; set; }

        public decimal Ballance { get; set; }
 


    }
}
