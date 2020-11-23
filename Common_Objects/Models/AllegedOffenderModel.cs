using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class AllegedOffenderModel
    {
        private SDIIS_DatabaseEntities db = new SDIIS_DatabaseEntities();
        public Alleged_Offender GetSpecificAllegedOffender(int allegedOffenderId)
        {
            Alleged_Offender allegedOffender;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var allegedOffenderList = (from r in dbContext.Alleged_Offenders
                                           where r.Alleged_Offender_Id.Equals(allegedOffenderId)
                                           select r).ToList();

                allegedOffender = (from r in allegedOffenderList
                                   select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return allegedOffender;
        }

        public Alleged_Offender GetSpecificAllegedOffenderByPerson(int personId)
        {
            Alleged_Offender allegedOffender;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var allegedOffenderList = (from r in dbContext.Alleged_Offenders
                                           where r.Person_Id.Equals(personId)
                                           select r).ToList();

                allegedOffender = (from r in allegedOffenderList
                                   select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return allegedOffender;
        }

        public List<Alleged_Offender> GetListOfAllegedOffenders()
        {
            List<Alleged_Offender> allegedOffenders;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var allegedOffenderList = (from r in dbContext.Alleged_Offenders
                                           select r).ToList();

                allegedOffenders = (from r in allegedOffenderList
                                    select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return allegedOffenders;
        }

        public List<Alleged_Offender> GetListOfAllegedOffenders(int incidentId)
        {
            List<Alleged_Offender> allegedOffenders;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var allegedOffenderList = (from r in dbContext.Alleged_Offenders
                                           where r.Incident_Id == incidentId
                                           select r).ToList();

                allegedOffenders = (from r in allegedOffenderList
                                    select r).ToList();
            }
            catch (Exception ex)
            {
                string exVal = ex.Message;
                return null;
            }

            return allegedOffenders;
        }

        public Alleged_Offender CreateAllegedOffender(int? incidentId, int? personId, bool isKnownOffender, int? childRelationshipTypeId, int? occupationId, string passport,
            string driversLicense, int? workAddressId, string whereAbouts)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var allegedOffender = new Alleged_Offender() { Incident_Id = incidentId, Person_Id = personId, Is_Known_Offender = isKnownOffender, Child_Relationship_Type_Id = childRelationshipTypeId, Occupation_Id = occupationId, Passport = passport, Drivers_License = driversLicense, Work_Address_Id = workAddressId, Whereabouts = whereAbouts };

            try
            {
                var newAllegedOffender = dbContext.Alleged_Offenders.Add(allegedOffender);

                dbContext.SaveChanges();

                return newAllegedOffender;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Alleged_Offender EditAllegedOffender(int allegedOffenderId, int? incidentId, int? personId, bool isKnownOffender, int? childRelationshipTypeId, int? occupationId, string passport,
            string driversLicense, int? workAddressId, string whereAbouts)
        {
            Alleged_Offender editAllegedOffender;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editAllegedOffender = (from a in dbContext.Alleged_Offenders
                                           where a.Alleged_Offender_Id.Equals(allegedOffenderId)
                                           select a).FirstOrDefault();

                    if (editAllegedOffender == null) return null;

                    editAllegedOffender.Incident_Id = incidentId;
                    editAllegedOffender.Person_Id = personId;
                    editAllegedOffender.Is_Known_Offender = isKnownOffender;
                    editAllegedOffender.Child_Relationship_Type_Id = childRelationshipTypeId;
                    editAllegedOffender.Occupation_Id = occupationId;
                    editAllegedOffender.Passport = passport;
                    editAllegedOffender.Drivers_License = driversLicense;
                    editAllegedOffender.Work_Address_Id = workAddressId;
                    editAllegedOffender.Whereabouts = whereAbouts;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editAllegedOffender;
        }

        public int GetProvinceId(int? TownId)
        {
            int ProvId = (from a in db.Districts
                          join b in db.Local_Municipalities on a.District_Id equals b.District_Municipality_Id
                          join c in db.Towns on b.Local_Municipality_Id equals c.Local_Municipality_Id
                          where c.Town_Id == TownId
                          select a.Province_Id).FirstOrDefault();
            return ProvId;

        }
        public int GetDistrictId(int? TownId)
        {
            var db = new SDIIS_DatabaseEntities();
            int DisId = (from a in db.Districts
                         join b in db.Local_Municipalities on a.District_Id equals b.District_Municipality_Id
                         join c in db.Towns on b.Local_Municipality_Id equals c.Local_Municipality_Id
                         where c.Town_Id== TownId
                         select a.District_Id).FirstOrDefault();
            return DisId;
        }
        public int GetLocalMunicipalityId(int? TownId)
        {
            var db = new SDIIS_DatabaseEntities();
            int LMunId = db.Towns.Find(TownId).Local_Municipality_Id;
            return LMunId;
        }

        public int GetFirstTown()
        {
            var db = new SDIIS_DatabaseEntities();
            int FTown = (from k in db.Towns
                         select k.Town_Id).FirstOrDefault();
            return FTown;

        }

        public List<Town> GetAllTowns()
        {
            return (db.Towns).ToList();
        }
    }
}
