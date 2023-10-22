using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthClaim.DAL.Models
{
    public class EmpClaim:BaseModel
    {
        
        public bool? IsSpecailDisease { get; set; }
        public double? HospitalTotalBill { get; set; }
        public DateTime? ClaimRequetsDate { get; set; }
        public double? ClaimAmount { get; set; }
        public DateTime? ClaimApprovedDate { get; set; }
        public double? ClaimApprovedAmount { get; set; }
    }
}
