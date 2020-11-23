using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class InformantModel
    {
        private SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        public CPR_Informant GetSpecificCPRInformant(int cprInformantId)
        {
            CPR_Informant cprInformant;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var cprInformantList = (from r in dbContext.CPR_Informants
                                        where r.CPR_Informant_Id.Equals(cprInformantId)
                                        select r).ToList();

                cprInformant = (from r in cprInformantList
                                select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return cprInformant;
        }

        public List<CPR_Informant> GetListOfCPRInformants()
        {
            List<CPR_Informant> cprInformants;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var cprInformantList = (from r in dbContext.CPR_Informants
                                        select r).ToList();

                cprInformants = (from r in cprInformantList
                                 select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return cprInformants;
        }

        public List<CPR_Informant> GetListOfCPRInformants(int incidentId)
        {
            List<CPR_Informant> cprInformants;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var cprInformantList = (from r in dbContext.CPR_Informants
                                        where r.Incident_Id == incidentId
                                        select r).ToList();

                cprInformants = (from r in cprInformantList
                                 select r).ToList();
            }
            catch (Exception ex)
            {
                string exVal = ex.Message;
                return null;
            }

            return cprInformants;
        }

        public CPR_Informant CreateCPRInformant(int incidentId, int personId, int? districtId, int? informantCapacityTypeId, int? childRelationshipId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var cprInformant = new CPR_Informant() { Incident_Id = incidentId, Person_Id = personId, District_Id = districtId, Informant_Capacity_Type_Id = 1, Relationship_Type_Id = childRelationshipId };

            try
            {
                var newCPRInformant = dbContext.CPR_Informants.Add(cprInformant);

                dbContext.SaveChanges();

                return newCPRInformant;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public CPR_Informant EditCPRInformant(int cprInformantId, int incidentId, int personId, int? districtId, int? informantCapacityTypeId, int? childRelationshipId)
        {
            CPR_Informant editCPRInformant;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editCPRInformant = (from a in dbContext.CPR_Informants
                                        where a.CPR_Informant_Id.Equals(cprInformantId)
                                        select a).FirstOrDefault();

                    if (editCPRInformant == null) return null;

                    editCPRInformant.Incident_Id = incidentId;
                    editCPRInformant.Person_Id = personId;
                    editCPRInformant.District_Id = districtId;
                    editCPRInformant.Informant_Capacity_Type_Id = informantCapacityTypeId;
                    editCPRInformant.Relationship_Type_Id = childRelationshipId;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editCPRInformant;
        }

        public int GetProvinceId(int? Id)
        {
            if (Id != null)
            {
                return db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(Id).Local_Municipality_Id).District_Municipality_Id).Province_Id;
            }
            else return 0;
        }

        public int GetDistrictId(int? Id)
        {
            if (Id != null)
            {
                return db.Districts.Find(db.Local_Municipalities.Find(db.Towns.Find(Id).Local_Municipality_Id).District_Municipality_Id).District_Id;
            }
            else return 0;
        }

        public int GetLocalMunicipalityId(int? Id)
        {
            if (Id != null)
            {
                return db.Local_Municipalities.Find(db.Towns.Find(Id).Local_Municipality_Id).Local_Municipality_Id;
            }
            else return 0;
        }

        public int GetTownId(int? Id)
        {
            if (Id != null)
            {
                return db.Towns.Find(Id).Town_Id;
            }
            else return 0;
        }
    }
}
