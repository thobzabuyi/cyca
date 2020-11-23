using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.ViewModels
{
    public class PCMDSessionOutcomeViewModel
    {
        public int DSession_Id { get; set; }
        public int? Intake_Assessment_Id { get; set; }
        public string Current_Module_Attended { get; set; }
        public string Session_Attend { get; set; }
        public string Session_Date { get; set; }
        public string Name_of_the_Facilitator { get; set; }
        public string Name_of_Co_Facilitator { get; set; }
        public string Process_Notes { get; set; }
        public string Next_Session_Date { get; set; }
        public string Compliance { get; set; }


        //uploading file documents fields

        public int File_Id { get; set; }
        public string Module_Name { get; set; }
        public string Description { get; set; }
        public string File_Doc { get; set;}
        public string fileName { get; set; }

        //PCM_D_Diversion_Outcome
        public int Diversion_Outotcome_Id { get; set; }
        public DateTime? Court_Date { get; set; }
        public string Remand { get; set; }
        public string Reason_Remand { get; set; }
        public DateTime? Next_Court_Date { get; set; }
        public string Court_Outcome { get; set; }
        public string Case_Status { get; set; }


    }
}
