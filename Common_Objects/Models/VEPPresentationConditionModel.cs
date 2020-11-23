using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class VEPPresentationConditionModel
    {
        public int Create(int selected_ConditionId, int CaseId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var victimRecord = new VEP_VictimsConditions();

                victimRecord.Caseid = CaseId;
                victimRecord.PresentationConditionID = selected_ConditionId;

                dbContext.VEP_VictimsConditions.Add(victimRecord);
                dbContext.SaveChanges();

                return victimRecord.Id;
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return -1;
            }
        }

        public VEP_PresentationCondition GetSpecificPresentationCondition(int presentationConditionID)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_PresentationCondition.SingleOrDefault(a => a.Id == presentationConditionID);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<VEP_PresentationCondition> GetSelectedPresentationCondition(int presentationConditionId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var query = (from pt in dbContext.VEP_PresentationCondition
                             join ttab in dbContext.VEP_VictimsConditions on pt.Id equals ttab.PresentationConditionID
                             where ttab.Caseid == presentationConditionId
                             select pt).ToList();

                return query;
                //return dbContext.VEP_VictimizationType.Where(a => a. == victimizationTypeId).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<VEP_PresentationCondition> GetListOfPresentationCondition()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_PresentationCondition.ToList();
            }
            catch (Exception)
            {
                return null;
            }

        }

        public bool AddPresentationConditionToCase(int CaseId, List<int> conditionsIds)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var conditionsToEdit = dbContext.VEP_VictimsConditions.Where(x => x.Caseid.Equals(CaseId));

                dbContext.VEP_VictimsConditions.RemoveRange(conditionsToEdit);
                dbContext.SaveChanges();
               
                foreach (var roleId in conditionsIds)
                {
                    var newCondition = new VEP_VictimsConditions()
                    {
                        Caseid = CaseId,
                        PresentationConditionID = roleId
                    };
                
                    dbContext.VEP_VictimsConditions.Add(newCondition);
                    dbContext.SaveChanges();
                }
                
                return true;
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return false;
            }
        }
    }
}
