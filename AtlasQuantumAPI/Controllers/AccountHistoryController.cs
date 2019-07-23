using System;
using System.Collections.Generic;
using System.Linq;
using AtlasQuantumAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SimpleInjector;
using Newtonsoft.Json;

namespace AtlasQuantumAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountHistoryController : ControllerBase
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

                var cachedAccount = Startup.container.GetInstance<CachedAccountHistoryRepository>();
                var allAccounts = cachedAccount.GetAll();
                result = new JObject[allAccounts.Count()];

                foreach (var conta in allAccounts.ToList())
                {
                    result[count] = JObject.Parse("{\"AccountNumber\": \"" + conta.AccountId.ToString() + "\", \"Amount\": \"" + conta.Amount + "\", \"Date\": \"" + conta.OccurDate + "\", \"Type\": \"" + conta.Type + "\", \"Received From\": \"" + conta.ReceivedFrom + "\"}");
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

        [Route("list")]
        [HttpGet]
        public ActionResult<IEnumerable<JObject>> Get(JObject json)
        {
            var res = JsonConvert.DeserializeObject<dynamic>(json.ToString());

            try
            {
                string accountInfo = string.Empty;
                JObject[] result = null;
                int count = 0;

                var cachedAccount = Startup.container.GetInstance<CachedAccountHistoryRepository>();
                var singleAccount = cachedAccount.GetById(Guid.Parse(Convert.ToString(res.id)));
                result = new JObject[singleAccount.Count];

                foreach (var conta in singleAccount)
                {
                                 
                    result[count] = JObject.Parse("{\"AccountNumber\": \"" + conta.AccountId.ToString() + "\", \"Amount\": \"" + conta.Amount + "\", \"Date\": \"" + conta.OccurDate + "\", \"Type\": \"" + conta.Type + "\", \"Received From\": \"" + conta.ReceivedFrom + "\"}");
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
                     
        [HttpPost]
        public ActionResult<string> Post(JObject json)
        {
            Guid accId;
            decimal amount;
            int result = 0;

            var res = JsonConvert.DeserializeObject<dynamic>(json.ToString());


            #region [DATA VALIDATIONS]
            try
            {

                accId = Guid.Parse(Convert.ToString(res.accountId));
            }
            catch (Exception ex)
            {
                return "Invalid Account Number.";
            }


            try
            {
                amount = decimal.Parse(Convert.ToString(res.amount));
            }
            catch (Exception ex)
            {
                return "Invalid Amount Value.";
            }
            #endregion


            try
            {
                var cachedAccount = Startup.container.GetInstance<CachedAccountRepository>();

                if (res.type == "D")
                {
                    if (amount < Convert.ToDecimal("0,01"))
                        return "Deposit should be greater than 0.00";

                    result = Convert.ToInt32(cachedAccount.Deposit(accId, amount));

                    var ret = (result == 1) ? "Occurred an account deposit." : "There was an error in account deposit.";

                    return ret;

                }
                else if (res.type == "W")
                {
                    if (amount < Convert.ToDecimal("0,01"))
                        return "Cashout should be greater than 0.00";

                    result = cachedAccount.Withdrawal(accId, amount);

                    switch (result)
                    {
                        case 0:
                            return "There was an account withdrawal error";
                            break;
                        case 1:
                            return "Occurred an account Withdrawal.";
                            break;
                        case 2:
                            return "Cashout value is greater than account value";
                            break;

                    }


                }

                return "";

            }
            catch (Exception ex)
            {

                throw;
            }

        }


    }
}
