using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class NisisSiteEAModel
    {
        public NISIS_Site_EA GetSpecificNisisSiteEA(int nisisSiteEAId)
        {
            NISIS_Site_EA nisisSiteEA;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisSiteEAList = (from r in dbContext.NISIS_Site_EA_Items
                                       where r.NISIS_Site_EA_Id.Equals(nisisSiteEAId)
                                       select r).ToList();

                nisisSiteEA = (from r in nisisSiteEAList
                               select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return nisisSiteEA;
        }

        public NISIS_Site_EA GetSpecificNisisSiteEA(string nisisSiteEACode)
        {
            NISIS_Site_EA nisisSiteEA;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisSiteEAList = (from r in dbContext.NISIS_Site_EA_Items
                                       where r.EA_Code.Equals(nisisSiteEACode)
                                       select r).ToList();

                nisisSiteEA = (from r in nisisSiteEAList
                               select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return nisisSiteEA;
        }

        public List<NISIS_Site_EA> GetListOfNisisSiteEAs(bool showInActive, bool showDeleted)
        {
            List<NISIS_Site_EA> nisisSiteEAs;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisSiteEAList = (from x in dbContext.NISIS_Site_EA_Items
                                       where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                       where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                       select x).ToList();

                nisisSiteEAs = (from x in nisisSiteEAList
                                select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return nisisSiteEAs;
        }

        public List<NISIS_Site_EA> GetListOfNisisSiteEAs(bool showInActive, bool showDeleted, int nisisSiteId)
        {
            List<NISIS_Site_EA> nisisSiteEAs;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisSiteEAList = (from x in dbContext.NISIS_Site_EA_Items
                                       where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                       where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                       where x.NISIS_Site_Id.Equals(nisisSiteId)
                                       select x).ToList();

                nisisSiteEAs = (from x in nisisSiteEAList
                                select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return nisisSiteEAs;
        }

        public NISIS_Site_EA CreateSiteEA(int siteId, string EACode, string communityName, string createdBy, DateTime createdDate, bool isActive, bool isDeleted)
        {

            NISIS_Site_EA newSiteEA;

            var dbContext = new SDIIS_DatabaseEntities();

            var siteEA = new NISIS_Site_EA()
            {
                NISIS_Site_Id = siteId,
                EA_Code = EACode,
                Community_Name = communityName,
                Created_By = createdBy,
                Date_Created = createdDate,
                Is_Active = isActive,
                Is_Deleted = isDeleted
            };

            try
            {
                newSiteEA = dbContext.NISIS_Site_EA_Items.Add(siteEA);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var exMsg = ex.Message;
                return null;
            }

            return newSiteEA;
        }

        public NISIS_Site_EA EditSiteEA(int siteEAId, int siteId, string EACode, string communityName, string modifiedBy, DateTime dateLastModified, bool isActive, bool isDeleted)
        {
            NISIS_Site_EA editSiteEA;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                editSiteEA = (from x in dbContext.NISIS_Site_EA_Items
                              where x.NISIS_Site_EA_Id.Equals(siteEAId)
                              select x).FirstOrDefault();

                if (editSiteEA == null) return null;

                editSiteEA.NISIS_Site_Id = siteId;
                editSiteEA.EA_Code = EACode;
                editSiteEA.Community_Name = communityName;
                editSiteEA.Modified_By = modifiedBy;
                editSiteEA.Date_Last_Modified = dateLastModified;
                editSiteEA.Is_Active = isActive;
                editSiteEA.Is_Deleted = isDeleted;

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }

            return editSiteEA;
        }

        public NISIS_Site_EA SetSiteEAIsActive(int siteEAId, bool isActive)
        {
            NISIS_Site_EA editSiteEA;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editSiteEA = (from x in dbContext.NISIS_Site_EA_Items
                                  where x.NISIS_Site_EA_Id.Equals(siteEAId)
                                  select x).FirstOrDefault();

                    if (editSiteEA == null) return null;

                    editSiteEA.Is_Active = isActive;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editSiteEA;
        }

        public NISIS_Site_EA SetSiteEAIsDeleted(int siteEAId, bool isDeleted)
        {
            NISIS_Site_EA editSiteEA;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editSiteEA = (from x in dbContext.NISIS_Site_EA_Items
                                  where x.NISIS_Site_EA_Id.Equals(siteEAId)
                                  select x).FirstOrDefault();

                    if (editSiteEA == null) return null;

                    editSiteEA.Is_Deleted = isDeleted;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editSiteEA;
        }
    }
}
