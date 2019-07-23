
using System.Data;

namespace AtlasQuantumAPI.Data
{
    public interface IDataContext
    {
        IDbConnection CreateConnection();
    }
}
