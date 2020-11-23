using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
   public class PCMCourtAdminModel
    {
        public bool IsIntAss(int Intake_Assessment_Id)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                return db.PCM_Recommendation.Where(o => o.Intake_Assessment_Id.Equals(Intake_Assessment_Id)).Any();
            }
        }

        //public PCMCourtAdminViewModel GetRecId(int IntAssId)
        //{
        //    PCMCourtAdminViewModel vm = new PCMCourtAdminViewModel();

        //    using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
        //    {
        //        try
        //        {
        //            int? id = (from r in db.PCM_Recommendation
        //                       where (r.Intake_Assessment_Id == IntAssId)
        //                       select r.Recommendation_Type_Id).FirstOrDefault();

        //            PCM_Recommendation rr = db.PCM_Recommendation.Find(id);

        //            if(rr != null)
        //            {
        //                vm.Intake_Assessment_Id = db.PCM_Recommendation.Find(rr.Intake_Assessment_Id).Intake_Assessment_Id;
        //                vm.Recommendation_Type_Id = rr.Recommendation_Type_Id;
        //            }
        //        }
        //        catch
        //        {

        //        }
        //        return vm;
        //    }
        //}

        public int? GetRecId(int IntAssId)
        {
            using (SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities())
            {
                return (from r in db.PCM_Recommendation
                        where (r.Intake_Assessment_Id == IntAssId)
                        select r.Recommendation_Type_Id).FirstOrDefault();
            }
        
        }

    }
}
