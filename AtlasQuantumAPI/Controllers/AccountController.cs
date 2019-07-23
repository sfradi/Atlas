using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtlasQuantumAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SimpleInjector; 
using AtlasQuantumAPI.Repository;
using Newtonsoft.Json.Linq;
using System.Globalization; 
using Newtonsoft.Json;

namespace AtlasQuantumAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private static Container container;
        private static Serilog.ILogger logger;
             
        [HttpGet]
        public ActionResult<IEnumerable<JObject>> Get()
        {

            try
            {
                string accountInfo = string.Empty;
                JObject[] result = null;
                int count = 0;

                var cachedAccount = Startup.container.GetInstance<CachedAccountRepository>();
                var allAccounts = cachedAccount.GetAll();
                result = new JObject[allAccounts.Count()];

                foreach (var conta in allAccounts.ToList())
                {
                    result[count] = JObject.Parse("{\"AccountNumber\": \"" + conta.AccountId.ToString() + "\", \"Person Name\": \"" + conta.PersonName + "\"}");
                    count++;
                }

                return result;


            }
            catch (Exception ex)
            {
                logger.Error(ex, "Unable to show result.");
                throw;
            }
            
        }
              
        [HttpGet("{id}")]
        public ActionResult<JObject> Get(Guid id)
        {
            try
            {
                string accountInfo = string.Empty;
                JObject result = new JObject();
                var cachedAccount = Startup.container.GetInstance<CachedAccountRepository>();
                var singleAccount = cachedAccount.GetById(id);


                foreach (var conta in singleAccount.ToList())
                {
                    // result = "AccountNumber: " + conta.AccountId + " ::: Person Name: " + conta.PersonName;
                    result = JObject.Parse("{\"AccountNumber\": \"" + conta.AccountId.ToString() + "\", \"Person Name\": \"" + conta.PersonName + "\" , \"Amount\": \"R$ " + conta.Amount.ToString("F2", CultureInfo.InvariantCulture) + "\" , \"Ballance\": \"R$ " + conta.Ballance + "\" }");

                }


                return result;

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Unable to build the cache.");
                throw;
            }



        }
                
        [HttpPut("{name}/{amount}")]    
        public ActionResult<string> Put(string name, decimal amount)
        {
       
            try
            {
                var cachedAccount = Startup.container.GetInstance<CachedAccountRepository>();
                var result = cachedAccount.Add(new Domain.AccountEntity { AccountId = Guid.NewGuid(), PersonName = name, Amount = amount, Ballance = amount });

                if (result == true)
                {
                    return "Account has been created.";
                }
                else
                {
                    return "Fail to create the account.";
                }
                               
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Unable to build the cache.");
                throw;
            }
                       
        }

        [Route("addUser")]
        [HttpPost]
        public ActionResult<string> Post(JObject json)
        {
                 
            var res = JsonConvert.DeserializeObject<dynamic>(json.ToString());

            #region [DATA VALIDATION]
            if (res.name == "" || res.name == null)
            { return "Insert a Account Person Name."; }

            if (Convert.ToDecimal(res.amount) < 0)
            { return "Insert a valid Amount Value."; }
            #endregion

            try
            {
                var cachedAccount = Startup.container.GetInstance<CachedAccountRepository>();
                var result = cachedAccount.Add(new Domain.AccountEntity { AccountId = Guid.NewGuid(), PersonName = res.name, Amount = res.amount, Ballance = res.amount });

                if (result == true)
                {
                    return "Account has been created.";
                }
                else
                {
                    return "Fail to create the account.";
                }
                               
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Unable to build the cache.");
                throw;
            }



        }                         
       
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
        }
    }
}
