using Common_Objects.Models;
using System.Collections.Generic;

namespace Common_Objects.ViewModels
{
    public class CPRCorrespondenceViewModel
    {
        public int Person_Id { get; set; }
        public List<CPR_Correspondence> Correspondence { get; set; }
    }
}
