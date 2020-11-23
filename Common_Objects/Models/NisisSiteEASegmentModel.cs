using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class NisisSiteEASegmentModel
    {
        public NISIS_Site_EA_Segment GetSpecificNisisSiteEASegment(int nisisSiteEASegmentId)
        {
            NISIS_Site_EA_Segment nisisSiteEASegment;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisSiteEASegmentList = (from r in dbContext.NISIS_Site_EA_Segment_Items
                                              where r.Segment_Id.Equals(nisisSiteEASegmentId)
                                              select r).ToList();

                nisisSiteEASegment = (from r in nisisSiteEASegmentList
                                      select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return nisisSiteEASegment;
        }

        public List<NISIS_Site_EA_Segment> GetListOfNisisSiteEASegments(bool showInActive, bool showDeleted)
        {
            List<NISIS_Site_EA_Segment> nisisSiteEASegments;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisSiteEASegmentList = (from x in dbContext.NISIS_Site_EA_Segment_Items
                                              where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                              where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                              orderby x.Segment_Id
                                              select x).ToList();

                nisisSiteEASegments = (from x in nisisSiteEASegmentList
                                       select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return nisisSiteEASegments;
        }

        public List<NISIS_Site_EA_Segment> GetListOfNisisSiteEASegments(bool showInActive, bool showDeleted, int nisisSiteEAId)
        {
            List<NISIS_Site_EA_Segment> nisisSiteEASegments;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var nisisSiteEASegmentList = (from x in dbContext.NISIS_Site_EA_Segment_Items
                                              where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                              where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                              where x.NISIS_Site_EA_Id.Equals(nisisSiteEAId)
                                              orderby x.Segment_Id
                                              select x).ToList();

                nisisSiteEASegments = (from x in nisisSiteEASegmentList
                                       select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return nisisSiteEASegments;
        }

        public NISIS_Site_EA_Segment CreateSiteEASegment(int siteEAId, string boundaryDescription, string listingStartingPointDescription, string listingRouteDescription, string createdBy, DateTime createdDate, bool isActive, bool isDeleted)
        {

            NISIS_Site_EA_Segment newSiteEASegment;

            var dbContext = new SDIIS_DatabaseEntities();

            var siteEASegment = new NISIS_Site_EA_Segment()
            {
                NISIS_Site_EA_Id = siteEAId,
                Boundary_Description = boundaryDescription,
                Listing_Start_Point_Description = listingStartingPointDescription,
                Listing_Route = listingRouteDescription,
                Created_By = createdBy,
                Date_Created = createdDate,
                Is_Active = isActive,
                Is_Deleted = isDeleted
            };

            try
            {
                newSiteEASegment = dbContext.NISIS_Site_EA_Segment_Items.Add(siteEASegment);
                dbContext.SaveChanges();

                // Rebuild index column
                var segments = dbContext.NISIS_Site_EA_Segment_Items.Where(x => x.NISIS_Site_EA_Id.Equals(siteEAId)).OrderBy(y => y.Segment_Id).ToList();

                var index = 1;
                foreach (var segment in segments)
                {
                    segment.Segment_Number = index;
                    index++;
                }

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var exMsg = ex.Message;
                return null;
            }

            return newSiteEASegment;
        }

        public NISIS_Site_EA_Segment EditSiteEASegment(int siteEASegmentId, int siteEAId, string boundaryDescription, string listingStartingPointDescription, string listingRouteDescription, string modifiedBy, DateTime dateLastModified, bool isActive, bool isDeleted)
        {
            NISIS_Site_EA_Segment editSiteEASegment;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                editSiteEASegment = (from x in dbContext.NISIS_Site_EA_Segment_Items
                                     where x.Segment_Id.Equals(siteEASegmentId)
                                     select x).FirstOrDefault();

                if (editSiteEASegment == null) return null;

                editSiteEASegment.NISIS_Site_EA_Id = siteEAId;
                editSiteEASegment.Boundary_Description = boundaryDescription;
                editSiteEASegment.Listing_Start_Point_Description = listingStartingPointDescription;
                editSiteEASegment.Listing_Route = listingRouteDescription;
                editSiteEASegment.Modified_By = modifiedBy;
                editSiteEASegment.Date_Last_Modified = dateLastModified;
                editSiteEASegment.Is_Active = isActive;
                editSiteEASegment.Is_Deleted = isDeleted;

                dbContext.SaveChanges();

                // Rebuild index column
                var segments = dbContext.NISIS_Site_EA_Segment_Items.Where(x => x.NISIS_Site_EA_Id.Equals(siteEAId)).OrderBy(y => y.Segment_Id).ToList();

                var index = 1;
                foreach (var segment in segments)
                {
                    segment.Segment_Number = index;
                    index++;
                }

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }

            return editSiteEASegment;
        }

        public NISIS_Site_EA_Segment SetSiteEASegmentIsActive(int siteEASegmentId, bool isActive)
        {
            NISIS_Site_EA_Segment editSiteEASegment;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editSiteEASegment = (from x in dbContext.NISIS_Site_EA_Segment_Items
                                         where x.NISIS_Site_EA_Id.Equals(siteEASegmentId)
                                         select x).FirstOrDefault();

                    if (editSiteEASegment == null) return null;

                    editSiteEASegment.Is_Active = isActive;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editSiteEASegment;
        }

        public NISIS_Site_EA_Segment SetSiteEASegmentIsDeleted(int siteEASegmentId, bool isDeleted)
        {
            NISIS_Site_EA_Segment editSiteEASegment;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editSiteEASegment = (from x in dbContext.NISIS_Site_EA_Segment_Items
                                         where x.Segment_Id.Equals(siteEASegmentId)
                                         select x).FirstOrDefault();

                    if (editSiteEASegment == null) return null;

                    editSiteEASegment.Is_Deleted = isDeleted;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editSiteEASegment;
        }

    }
}
