using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Common_Objects.Models
{
    public class pcmreferralmodelcs
    {
        public List<PCMReferralsViewModel> GetPCMReferralsList()
        {
            List<PCMReferralsViewModel> rVM = new List<PCMReferralsViewModel>();
            SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();

            var rList = (from pf in db.PCM_Referrals
                         join pcd in db.PCM_Case_Details
                         on pf.PCM_Case_Id equals pcd.PCM_Case_Id
                         select new { pf.Referrals_Id, pf.PCM_Case_Id, pf.Client_Employee_Details_ID, pf.theClerk, pf.theParticular, pf.Type_Referral_Id }).ToList();

            foreach (var item in rList)
            {
                PCMReferralsViewModel objR = new PCMReferralsViewModel();

                objR.Referrals_Id = item.Referrals_Id;
                objR.PCM_Case_Id = item.PCM_Case_Id;
                objR.Client_Employee_Details_ID = item.Client_Employee_Details_ID;
                objR.theClerk = item.theClerk;
                objR.theParticular = item.theParticular;
                //objR.Type_Referral_Id = item.Type_Referral_Id;

                rVM.Add(objR);
            }

            return rVM;
        }
    }
}
