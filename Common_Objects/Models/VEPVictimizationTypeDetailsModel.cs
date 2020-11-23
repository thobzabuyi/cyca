using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
   public  class VEPVictimizationTypeDetailsModel
    {
        public int Create(int selected_VictimizationId, int CaseId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var victimRecord = new VEP_VictimizationTypeDetails();

                victimRecord.Case_Id = CaseId;
                victimRecord.VictimizationType = selected_VictimizationId;

                dbContext.VEP_VictimizationTypeDetails.Add(victimRecord);
                dbContext.SaveChanges();

                return victimRecord.Case_Id;
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return -1;
            }
        }

        public int Delete(int CaseId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var victimRecord = dbContext.VEP_VictimizationTypeDetails.Where(a => a.Case_Id == CaseId);
                dbContext.VEP_VictimizationTypeDetails.RemoveRange(victimRecord);
                dbContext.SaveChanges();

                return 1;
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return -1;
            }
        }

        public VEP_VictimizationTypeDetails GetSpecificVictimizationTypeDetails(int victimizationTypeDetailsId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_VictimizationTypeDetails.SingleOrDefault(a => a.Id == victimizationTypeDetailsId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<VEP_VictimizationTypeDetails> GetVictimizationTypeDetailsList(int victimizationTypeDetailsId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {               
                return dbContext.VEP_VictimizationTypeDetails.Where(a => a.Case_Id == victimizationTypeDetailsId).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }       
    }
}
