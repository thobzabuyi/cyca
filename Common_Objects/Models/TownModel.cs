using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class TownModel
    {
        public Town GetSpecificTown(int townId)
        {
            Town town;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var townList = (from r in dbContext.Towns
                                where r.Town_Id.Equals(townId)
                                select r).ToList();

                town = (from r in townList
                        select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return town;
        }

        public List<Town> GetListOfTowns()
        {
            List<Town> towns;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var townList = (from r in dbContext.Towns
                                orderby r.Description
                                select r).ToList();

                towns = (from r in townList
                         select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return towns;
        }



        public List<Town> GetListOfTowns(int localMunicipalityId)
        {
            List<Town> towns;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var townList = (from r in dbContext.Towns
                                    where r.Local_Municipality_Id.Equals(localMunicipalityId)
                                    orderby r.Description
                                    select r).ToList();

                    towns = (from r in townList
                             select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return towns;
        }

        public List<CPR_Unsuitability_Forum> GetListOfForums()
        {

            List<CPR_Unsuitability_Forum> forums;

            var dbContext = new SDIIS_DatabaseEntities();

            try

            {
                var forumList = (from f in dbContext.CPR_Unsuitability_Forum
                                 orderby f.Forum_Name
                                 select f).ToList();
                forums = (from f in forumList
                          select f).ToList();
            }

            catch (Exception)
            {
                return null;
            }
            return forums;
        }

        public List<CPR_Unsuitability_Forum> GetListOfForums(int ForumNumber)
        {
            List<CPR_Unsuitability_Forum> forums;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var forumList = (from r in dbContext.CPR_Unsuitability_Forum
                                    where r.Forum_Id.Equals(ForumNumber)
                                    orderby r.Forum_Name
                                    select r).ToList();

                    forums = (from r in forumList
                              select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return forums;
        }

        public List<Town> GetListOfTowns_1()
        {
            List<Town> towns;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var townList = (from r in dbContext.Towns
                                orderby r.Description
                                select r).ToList();

                towns = (from r in townList
                         select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return towns;
        }

        //public List<Town> GetListOfTowns_1(int localMunicipalityId)
        //{
        //    List<Town> towns;

        //    using (var dbContext = new SDIIS_DatabaseEntities())
        //    {
        //        try
        //        {
        //            var townList = (from r in dbContext.Districts
        //                            join s in dbContext.Local_Municipalities on r.District_Id equals s.Local_Municipality_Id
        //                            join t in dbContext.Towns on s.Local_Municipality_Id equals t.Local_Municipality_Id
        //                            where s.Municipality_Id.Equals(localMunicipalityId)
        //                            orderby t.Description
        //                            select t).ToList();

        //            towns = (from r in townList
        //                     select r).ToList();
        //        }
        //        catch (Exception)
        //        {
        //            return null;
        //        }
        //    }

        //    return towns;
        //}
    }
}
