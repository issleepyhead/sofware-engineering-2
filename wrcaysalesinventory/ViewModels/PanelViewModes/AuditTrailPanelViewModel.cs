using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class AuditTrailPanelViewModel : BaseViewModel<AuditLogModel>
    {
        private DataService _ds;
        public AuditTrailPanelViewModel(DataService ds)
        {
            _ds = ds;
            DataList = _ds.GetAuditLogList();
        }
    }
}
