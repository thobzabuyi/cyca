using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class CourtOutcomeModel
    {
        public Court_Outcome_Item GetSpecificCourtOutcomeItem(int courtOutcomeItemId)
        {
            Court_Outcome_Item courtOutcomeItem;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var courtOutcomeItemList = (from r in dbContext.Court_Outcome_Items
                                            where r.Court_Outcome_Id.Equals(courtOutcomeItemId)
                                            select r).ToList();

                courtOutcomeItem = (from r in courtOutcomeItemList
                                    select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return courtOutcomeItem;
        }

        public Court_Outcome_Item GetCourtOutcomeItemHeading(int courtOutcomeItemId)
        {
            Court_Outcome_Item courtOutcomeHeadingItem;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var courtOutcomeItemList = (from r in dbContext.Court_Outcome_Items
                                            where r.Court_Outcome_Id.Equals(courtOutcomeItemId)
                                            select r).ToList();

                var courtOutcomeItem = (from r in courtOutcomeItemList
                                        select r).FirstOrDefault();

                courtOutcomeHeadingItem = (from r in dbContext.Court_Outcome_Items
                                           where r.Parent_Id == courtOutcomeItem.Parent_Id && r.Is_Heading
                                           select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return courtOutcomeHeadingItem;
        }

        public List<Court_Outcome_Item> GetListOfCourtOutcomeItems()
        {
            List<Court_Outcome_Item> courtOutcomeItems;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var courtOutcomeItemList = (from r in dbContext.Court_Outcome_Items
                                            select r).ToList();

                courtOutcomeItems = (from r in courtOutcomeItemList
                                     select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return courtOutcomeItems;
        }
    }
}
