using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class FirstReporterModel
    {
        public CPR_First_Reporter GetSpecificCPRFirstReporter(int cprFirstReporterId)
        {
            CPR_First_Reporter cprFirstReporter;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var cprFirstReporterList = (from r in dbContext.CPR_First_Reporters
                                            where r.CPR_First_Reporter_Id.Equals(cprFirstReporterId)
                                            select r).ToList();

                cprFirstReporter = (from r in cprFirstReporterList
                                    select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return cprFirstReporter;
        }

        public List<CPR_First_Reporter> GetListOfCPRFirstReporters()
        {
            List<CPR_First_Reporter> cprFirstReporters;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var cprFirstReporterList = (from r in dbContext.CPR_First_Reporters
                                            select r).ToList();

                cprFirstReporters = (from r in cprFirstReporterList
                                     select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return cprFirstReporters;
        }

        public List<CPR_First_Reporter> GetListOfCPRFirstReporters(int incidentId)
        {
            List<CPR_First_Reporter> cprFirstReporters;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var cprFirstReporterList = (from r in dbContext.CPR_First_Reporters
                                            where r.Incident_Id == incidentId
                                            select r).ToList();

                cprFirstReporters = (from r in cprFirstReporterList
                                     select r).ToList();
            }
            catch (Exception ex)
            {
                string exVal = ex.Message;
                return null;
            }

            return cprFirstReporters;
        }

        public CPR_First_Reporter CreateCPRFirstReporter(int incidentId, int? socialWorkerId, int? personId, int? districtId, int? childRelationTypeId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var cprFirstReporter = new CPR_First_Reporter() { Incident_Id = incidentId, Social_Worker_Id = socialWorkerId, Person_Id = personId, District_Id = districtId, Child_Relationship_Type_Id = childRelationTypeId };

            try
            {
                var newCPRFirstReporter = dbContext.CPR_First_Reporters.Add(cprFirstReporter);

                dbContext.SaveChanges();

                return newCPRFirstReporter;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public CPR_First_Reporter EditCPRFirstReporter(int cprFirstReporterId, int incidentId, int personId, int? districtId, int? childRelationTypeId)
        {
            CPR_First_Reporter editCPRFirstReporter;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editCPRFirstReporter = (from a in dbContext.CPR_First_Reporters
                                            where a.CPR_First_Reporter_Id.Equals(cprFirstReporterId)
                                            select a).FirstOrDefault();

                    if (editCPRFirstReporter == null) return null;

                    editCPRFirstReporter.Incident_Id = incidentId;
                    editCPRFirstReporter.Person_Id = personId;
                    editCPRFirstReporter.District_Id = districtId;
                    editCPRFirstReporter.Child_Relationship_Type_Id = childRelationTypeId;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editCPRFirstReporter;
        }
    }
}
