using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wrcaysalesinventory.Data.Models
{
    public class AuditLogModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Event { get; set; }
        public string Description { get; set; }
        public string DatePerformed { get; set; }
        public string TimePerformed { get; set; }
    }
}
