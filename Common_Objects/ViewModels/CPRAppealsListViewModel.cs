using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common_Objects.Models;
using Common_Objects.ViewModels;

namespace Common_Objects.ViewModels
{
    public class CPRAppealsListViewModel
    {
        private SDIIS_DatabaseEntities _db = new SDIIS_DatabaseEntities();
        public List<CPR_APPEALS> Appeal { get; set; }
        public List<CPR_Unsuitability> UnsuitablePersonsList { get; set; }
        public string Search_Surname { get; set; }
        public string Search_Name { get; set; }
        public string Search_RecNumber { get; set; }

        public List<CPR_APPEALS> GetCPR_Appeals()
        {
            return (from a in _db.CPR_APPEALS
                    where a.IsRemoved != true
                    select a).ToList();
        }

        public CPR_Unsuitability_Findings GetCPR_Unsuitability_Finding(int Id)
        {
            return _db.CPR_Unsuitability_Findings.Where(x => x.Unsuitability_Id == Id).FirstOrDefault();
        }

        public CPR_APPEALS GetCPR_Appeal(int Person_Id, CPR_Unsuitability_Findings Object)
        {
            return _db.CPR_APPEALS.Where(x => x.Person_Id == Person_Id && Object.CPR_Unsuitability.IsRemoved != true).FirstOrDefault(); ;
        }

        public CPR_APPEALS GetCPR_Appeal_1(int Person_Id, CPR_Unsuitability_Ruiling Object)
        {
            return _db.CPR_APPEALS.Where(x => x.Person_Id == Person_Id && Object.CPR_Unsuitability.IsRemoved != true).FirstOrDefault(); ;
        }

        public CPR_Unsuitability_Ruiling GetCPR_Unsuitability_Ruiling(int Unsuitablity_Id)
        {
            return _db.CPR_Unsuitability_Ruiling.Where(x => x.Unsuitability_Id == Unsuitablity_Id).FirstOrDefault();
        }

        
    }
}
