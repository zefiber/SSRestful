using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using Database;

namespace SSService
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public partial class SSService : ISSService, ISSPublic
    {
        List<Security> _securityList;
        DbAccess _db = Resource.Instance._Database;
        public SSService()
        {
            _securityList = _db.LoadSecurityTable();
           // _db.GetTableInformation("security");
            
        }

                
        public Security GetSecurityById(int sec_id)
        {
            return _securityList.First(a => a.ss_id == sec_id);
        }

        public List<Security> GetSimilarSymbol(string sec_sym)
        {
            List<Security> ret=  _securityList.Where(o => o.symbol.Contains(sec_sym))
                    .OrderBy(m => m.symbol.StartsWith(sec_sym)
                                     ? (m.symbol == sec_sym ? 0 : 1)
                                     : 2)
                    .Take(10)
                    .ToList<Security>();
            return ret;

        }

        

    }
}
