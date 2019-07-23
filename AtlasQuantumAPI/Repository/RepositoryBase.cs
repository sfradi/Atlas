using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtlasQuantumAPI.Data;
using Dapper;
 


namespace AtlasQuantumAPI.Repository
{
    public class RepositoryBase
    {

        protected virtual IDataContext DataContext { get; }

        protected RepositoryBase(IDataContext context)
        {
         
            DataContext = context;
        }


    }
}
