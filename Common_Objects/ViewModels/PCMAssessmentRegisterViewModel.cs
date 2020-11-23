using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class PCMAssessmentRegisterViewModel
    {
        public int Assesment_Register_Id { get; set; }
        public int? PCM_Case_Id { get; set; }
        public int? Assessed_By { get; set; }
        public DateTime? Assessment_Date { get; set; }
        public DateTime? Assessment_Time { get; set; }
        public int? Form_Of_Notification_Id { get; set; }
        public int? Town_Id { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Date_Created { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Date_Modified { get; set; }
    }
}
