using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace Common_Objects.Models
{
    public class AbuseIndicatorModel
    {
        private SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        public int Create(int selected_abuseIndicatorId, int Unsuitability_Id, string userName)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var abuseIndicatorRecord = new CPR_Unsuitability_Incident_Abuse_Indicator();

                abuseIndicatorRecord.CPR_Unsuitability_Id = Unsuitability_Id;
                abuseIndicatorRecord.Abuse_Indicator_Id = selected_abuseIndicatorId;
                abuseIndicatorRecord.CreatedTimeStamp = DateTime.Now;
                abuseIndicatorRecord.Created_By = (from a in dbContext.Users
                                                  where a.User_Name== userName
                                                  select a.User_Id).FirstOrDefault();

                dbContext.CPR_Unsuitability_Incident_Abuse_Indicator.Add(abuseIndicatorRecord);
                dbContext.SaveChanges();

                return abuseIndicatorRecord.CPR_Unsuitability_Incident_Abuse_Indicator_Id;
            }
            //catch (Exception ex)
            //{
            //    return -1;
            //}
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        //Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        var msg = validationError.PropertyName + " Error: " + validationError.ErrorMessage;

                    }
                }
            }
            return -1;
        }


        public int Delete(int id)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var abuseIndicatorRecord = dbContext.CPR_Unsuitability_Incident_Abuse_Indicator.Where(a => a.CPR_Unsuitability_Id == id);
                dbContext.CPR_Unsuitability_Incident_Abuse_Indicator.RemoveRange(abuseIndicatorRecord);
                dbContext.SaveChanges();

                return 1;
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return -1;
            }
        }

        public List<CPR_Unsuitability_Incident_Abuse_Indicator> GetSelectedListOfAbuseIndicators(int unsuitability_Id)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var query = (from pt in  dbContext.CPR_Unsuitability_Incident_Abuse_Indicator 
                             where pt.CPR_Unsuitability_Id == unsuitability_Id
                             select pt).ToList();

                return query;
            }
            catch (Exception)
            {
                return null;
            }
        }

        
        public Abuse_Indicator GetSpecificAbuseIndicator(int abuseIndicatorId)
        {
            Abuse_Indicator abuseIndicator;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var abuseIndicatorList = (from r in dbContext.Abuse_Indicators
                                          where r.Abuse_Indicator_Id.Equals(abuseIndicatorId)
                                          select r).ToList();

                abuseIndicator = (from r in abuseIndicatorList
                                  select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return abuseIndicator;
        }

        public List<Abuse_Indicator> GetListOfAbuseIndicators()
        {
            List<Abuse_Indicator> abuseIndicators;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var abuseIndicatorList = (from r in dbContext.Abuse_Indicators
                                              select r).ToList();

                    abuseIndicators = (from r in abuseIndicatorList
                                       select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return abuseIndicators;
        }

        public List<Abuse_Indicator> GetListOfAbuseIndicators(int primaryAbuseTypeId)
        {
            List<Abuse_Indicator> abuseIndicators;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var abuseIndicatorList = (from r in dbContext.Abuse_Indicators
                                              where r.Abuse_Type_Id == primaryAbuseTypeId
                                              select r).ToList();

                    abuseIndicators = (from r in abuseIndicatorList
                                       select r).ToList();

                }
                catch (Exception ex)
                {
                    var Test = ex.Message;
                    return null;
                }
            }

            return abuseIndicators;
        }

        public List<CPR_Unsuitability_Incident_Abuse_Indicator> GetListOfAbuseIndicatorsForCPR(int unsuitability_ID)
        {
            List<CPR_Unsuitability_Incident_Abuse_Indicator> abuseIndicators;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var abuseIndicatorList = (from r in dbContext.CPR_Unsuitability_Incident_Abuse_Indicator
                                              where r.CPR_Unsuitability_Id == unsuitability_ID
                                              select r).ToList();

                    abuseIndicators = (from r in abuseIndicatorList
                                       select r).ToList();
                }
                catch (Exception ex)
                {
                    var Test = ex.Message;
                    return null;
                }
            }

            return abuseIndicators;
        }

        public int GetLocalMunicipalityId(string TownId)
        {
            if (TownId != null && TownId != "")
            {
                int newTownId = Convert.ToInt32(TownId);
                return db.Towns.Find(newTownId).Local_Municipality_Id;
            }
            else return 0;
        }

        public int GetDistrictId(string TownId)
        {
            if (TownId != null && TownId != "")
            {
                int newTownId = Convert.ToInt32(TownId);
                int LocalMunId = db.Towns.Find(newTownId).Local_Municipality_Id;
                return db.Local_Municipalities.Find(LocalMunId).District_Municipality_Id;
            }
            else return 0;
        }

        public int GetProvinceId(string TownId)
        {
            if (TownId != null && TownId != "")
            {
                int newTownId = Convert.ToInt32(TownId);
                int LocalMunId = db.Towns.Find(newTownId).Local_Municipality_Id;
                int DisId = db.Local_Municipalities.Find(LocalMunId).District_Municipality_Id;
                return db.Districts.Find(DisId).Province_Id;
            }
            else return 0;
        }


    }
}
