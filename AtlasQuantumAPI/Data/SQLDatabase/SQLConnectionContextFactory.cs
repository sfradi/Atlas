using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AtlasQuantumAPI.Data.SQLDatabase
{
    public class SQLConnectionContextFactory : IDataContext
    {
        private static SQLConnectionContextFactory factory;

        public static SQLConnectionContextFactory Instance
        {
            get
            {
                if (factory == null)
                {
                    factory = new SQLConnectionContextFactory();
                }

                return factory;
            }
        }
        
        public IDbConnection CreateConnection()
        {
            var configString = ConfigurationManager.ConnectionStrings["SqlServer"];

            return new SqlConnection(configString.ConnectionString);
        }

    }
}
