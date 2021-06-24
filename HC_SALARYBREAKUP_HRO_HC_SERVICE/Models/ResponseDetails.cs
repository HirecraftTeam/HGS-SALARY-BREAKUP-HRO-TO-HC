using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_SALARYBREAKUP_HRO_HC_SERVICE.Models
{
    public class SalaryHead
    {
        public string HeadName { get; set; }
        public Int64 HeadAmountM { get; set; }
        public Int64 HeadAmountA { get; set; }
    }

    public class SalaryHeadGroup
    {
        public string HeadGroupName { get; set; }
        public List<SalaryHead> SalaryHeads { get; set; }
    }

    public class ResponseDetails
    {
        public object Id { get; set; }
        public int ProcessStatus { get; set; }
        public List<SalaryHeadGroup> SalaryHeadGroups { get; set; }
        public object FlexibleCompensationPlan { get; set; }
        public string ErrorDetails { get; set; }
    }

    public class responseDetails
    {
        public string ID { get; set; }
        public string TransformStatus { get; set; }
        public string TAMSProcessStatus { get; set; }
        public string Message { get; set; }
    }

}
