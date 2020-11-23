using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class NisisListingModel
    {
        public NISIS_Listing GetSpecificNisisListing(int nisisListingId)
        {
            NISIS_Listing nisisListing;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisListingList = (from r in dbContext.NISIS_Listing_Items
                                        where r.NISIS_Listing_Id.Equals(nisisListingId)
                                        select r).ToList();

                nisisListing = (from r in nisisListingList
                                select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return nisisListing;
        }

        public List<NISIS_Listing> GetListOfNisisListings(bool showInActive, bool showDeleted)
        {
            List<NISIS_Listing> nisisListings;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisListingList = (from x in dbContext.NISIS_Listing_Items
                                        where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                        where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                        select x).ToList();

                nisisListings = (from x in nisisListingList
                                 select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return nisisListings;
        }

        public List<NISIS_Listing> GetListOfNisisListings(bool showInActive, bool showDeleted, int eaSegmentId)
        {
            List<NISIS_Listing> nisisListings;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisListingList = (from x in dbContext.NISIS_Listing_Items
                                        where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                        where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                        where x.Segment_Id.Equals(eaSegmentId)
                                        select x).ToList();

                nisisListings = (from x in nisisListingList
                                 select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return nisisListings;
        }

        public List<NISIS_Listing> GetListOfNisisListings(bool showInActive, bool showDeleted, List<int> provinceIds)
        {
            List<NISIS_Listing> nisisListings;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisListingList = (from x in dbContext.NISIS_Listing_Items
                                        where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                        where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                        where provinceIds.Contains(x.NISIS_Site_EA_Segment.NISIS_Site_EA.NISIS_Site.Ward.Local_Municipality.District.Province_Id)
                                        select x).ToList();

                nisisListings = (from x in nisisListingList
                                 select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return nisisListings;
        }

        public List<NISIS_Listing> GetListOfNisisListingsByUser(bool showInActive, bool showDeleted, int createdByUserId)
        {
            List<NISIS_Listing> nisisListings;

            var dbContext = new SDIIS_DatabaseEntities();

            var userItem = (from x in dbContext.Users where x.User_Id.Equals(createdByUserId) select x).First();

            try
            {
                var nisisListingList = (from x in dbContext.NISIS_Listing_Items
                                        where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                        where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                        where x.Created_By.Equals(userItem.User_Name)
                                        select x).ToList();

                nisisListings = (from x in nisisListingList
                                 select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return nisisListings;
        }

        public NISIS_Listing CreateListing(int eaSegmentId, int? listingMethodId, int? capturedByUserId, string householdHeadFirstName, string householdHeadLastName, 
            string householdHeadMiddleName, string streetAddress, string houseOtherNumber, int? structureTypeId, string structureDescription, 
            string queriesOrComments, string createdBy, DateTime createdDate, bool isActive, bool isDeleted)
        {

            NISIS_Listing newSite;

            var dbContext = new SDIIS_DatabaseEntities();

            var site = new NISIS_Listing()
            {
                Segment_Id = eaSegmentId,
                Listing_Method_Id = listingMethodId,
                Captured_By_User_Id = capturedByUserId,
                Household_Head_First_Name = (structureTypeId != null && structureTypeId == (int)StructureTypeEnum.PrivateDwelling) ? householdHeadFirstName : null,
                Household_Head_Last_Name = (structureTypeId != null && structureTypeId == (int)StructureTypeEnum.PrivateDwelling) ? householdHeadLastName : null,
                Household_Head_Middle_Name = (structureTypeId != null && structureTypeId == (int)StructureTypeEnum.PrivateDwelling) ? householdHeadMiddleName : null,
                Street_Address = streetAddress,
                House_Other_Number = houseOtherNumber,
                Structure_Type_Id = structureTypeId,
                Structure_Description = structureDescription,
                Queries_or_Comments = queriesOrComments,
                QA_Status_Item_Id = (int)QAStatusEnum.NotSet,
                Created_By = createdBy,
                Created_Date = createdDate,
                Is_Active = isActive,
                Is_Deleted = isDeleted
            };

            try
            {
                newSite = dbContext.NISIS_Listing_Items.Add(site);
                dbContext.SaveChanges();

                // Rebuild record number column
                var listings = dbContext.NISIS_Listing_Items.Where(x => x.Segment_Id.Equals(eaSegmentId)).OrderBy(y => y.Segment_Id).ToList();

                var index = 1;
                var duIndex = 1;
                foreach (var listing in listings)
                {
                    if (listing.Structure_Type_Id.Equals((int)StructureTypeEnum.PrivateDwelling))
                    {
                        listing.Dwelling_Unit_Number = duIndex;
                        duIndex++;
                    }

                    listing.Record_Number = index;
                    index++;
                }

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var exMsg = ex.Message;
                return null;
            }

            return newSite;
        }

        public NISIS_Listing EditListing(int listingId, int eaSegmentId, int? listingMethodId, int? capturedByUserId, string householdHeadFirstName, string householdHeadLastName,
            string householdHeadMiddleName, string streetAddress, string houseOtherNumber, int? structureTypeId, string structureDescription,
            string queriesOrComments, string modifiedBy, DateTime? dateLastModified, bool isActive, bool isDeleted)
        {
            NISIS_Listing editSite;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                editSite = (from x in dbContext.NISIS_Listing_Items
                            where x.NISIS_Listing_Id.Equals(listingId)
                            select x).FirstOrDefault();

                if (editSite == null) return null;

                editSite.Segment_Id = eaSegmentId;
                editSite.Listing_Method_Id = listingMethodId;
                editSite.Captured_By_User_Id = capturedByUserId;
                editSite.Household_Head_First_Name = (structureTypeId != null && structureTypeId == (int)StructureTypeEnum.PrivateDwelling) ? householdHeadFirstName : null;
                editSite.Household_Head_Last_Name = (structureTypeId != null && structureTypeId == (int)StructureTypeEnum.PrivateDwelling) ? householdHeadLastName : null;
                editSite.Household_Head_Middle_Name = (structureTypeId != null && structureTypeId == (int)StructureTypeEnum.PrivateDwelling) ? householdHeadMiddleName : null;
                editSite.Street_Address = streetAddress;
                editSite.House_Other_Number = houseOtherNumber;
                editSite.Structure_Type_Id = structureTypeId;
                editSite.Structure_Description = structureDescription;
                editSite.Queries_or_Comments = queriesOrComments;
                editSite.Modified_By = modifiedBy;
                editSite.Modified_Date = dateLastModified;
                editSite.Is_Active = isActive;
                editSite.Is_Deleted = isDeleted;

                dbContext.SaveChanges();

                // Rebuild record number column
                var listings = dbContext.NISIS_Listing_Items.Where(x => x.Segment_Id.Equals(eaSegmentId)).OrderBy(y => y.Segment_Id).ToList();

                var index = 1;
                var duIndex = 1;
                foreach (var listing in listings)
                {
                    if (listing.Structure_Type_Id.Equals((int)StructureTypeEnum.PrivateDwelling))
                    {
                        listing.Dwelling_Unit_Number = duIndex;
                        duIndex++;
                    }

                    listing.Record_Number = index;
                    index++;
                }

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }

            return editSite;
        }

        public NISIS_Listing SetSiteListingQAStatus(int siteListingId, int qaStatusId)
        {
            NISIS_Listing editListing;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                editListing = (from x in dbContext.NISIS_Listing_Items
                               where x.NISIS_Listing_Id.Equals(siteListingId)
                               select x).FirstOrDefault();

                if (editListing == null) return null;

                editListing.QA_Status_Item_Id = qaStatusId;

                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }

            return editListing;
        }

        public NISIS_Listing SetListingIsActive(int siteId, bool isActive)
        {
            NISIS_Listing editSite;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editSite = (from x in dbContext.NISIS_Listing_Items
                                where x.NISIS_Listing_Id.Equals(siteId)
                                select x).FirstOrDefault();

                    if (editSite == null) return null;

                    editSite.Is_Active = isActive;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editSite;
        }

        public NISIS_Listing SetListingIsDeleted(int siteId, bool isDeleted)
        {
            NISIS_Listing editSite;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editSite = (from x in dbContext.NISIS_Listing_Items
                                where x.NISIS_Listing_Id.Equals(siteId)
                                select x).FirstOrDefault();

                    if (editSite == null) return null;

                    editSite.Is_Deleted = isDeleted;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editSite;
        }
    }
}
