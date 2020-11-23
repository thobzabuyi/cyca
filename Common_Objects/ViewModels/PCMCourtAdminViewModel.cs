using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
   public class PCMCourtAdminViewModel
    {
        public int? Recommendation_Id { get; set; }
        public int? Recommendation_Type_Id { get; set; }
        public int? Placement_Type_Id { get; set; }
        public string Comments_For_Recommendation { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Date_Created { get; set; }
        public int Modified_By { get; set; }
        public DateTime Date_Modified { get; set; }
        public int? Intake_Assessment_Id { get; set; }

        
    }
}
