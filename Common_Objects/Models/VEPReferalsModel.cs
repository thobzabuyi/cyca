using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
   public class VEPReferalsModel
    {
        public int referalid { get; set; }
        public VEP_Referals GetSpecificReferals(int referalId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_Referals.SingleOrDefault(a => a.ReferalsId == referalId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<VEP_Referals> GetListOfSpecificReferals(int caseId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_Referals.Where(a => a.CaseId == caseId).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<VEP_Referals> GetListOfReferalType()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_Referals.ToList();
            }
            catch (Exception)
            {
                return null;
            }

        }

        public VEP_Referals CreateReferrals(VEP_Referals model)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                VEP_Referals referalTable = new VEP_Referals();

                referalTable.CaseId = model.CaseId;
                referalTable.FromDepartment = 1;
                referalTable.ToDepartment = Convert.ToInt32(model.ToDepartment);
                referalTable.Notes = model.Notes;
                referalTable.Createdby = model.Createdby;
                referalTable.CreateDate = DateTime.Now;

                dbContext.VEP_Referals.Add(referalTable);
                dbContext.SaveChanges();

                return model;
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return null;
            }
        }

        public VEP_Referals GetSpecificReferral(int referalId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            
            try
            {
                return dbContext.VEP_Referals.Find(referalId);
            }
            catch(Exception)
            {
                return null;
            }
        }

        public VEP_Referals EditReferralDetails(int referralId,string referralNotes,int? referralTypeId)
        {
            VEP_Referals editReferralDetails;
            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editReferralDetails = (from p in dbContext.VEP_Referals
                                           where p.ReferalsId.Equals(referralId)
                                           select p).FirstOrDefault();

                    if (editReferralDetails == null) return null;

                    editReferralDetails.ReferalsId = referralId;
                    editReferralDetails.Notes = referralNotes.Trim();
                    editReferralDetails.ToDepartment = referralTypeId.Value;

                    dbContext.SaveChanges();
                }
                catch(Exception)
                {
                    return null;
                }
            }

            return editReferralDetails;
        }
    }
}
