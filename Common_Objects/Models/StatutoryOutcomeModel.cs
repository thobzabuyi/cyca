using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class StatutoryOutcomeModel
    {
        public Statutory_Outcome_Item GetSpecificStatutoryOutcomeItem(int statutoryOutcomeItemId)
        {
            Statutory_Outcome_Item statutoryOutcomeItem;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var statutoryOutcomeItemList = (from r in dbContext.Statutory_Outcome_Items
                                                where r.Statutory_Outcome_Item_Id.Equals(statutoryOutcomeItemId)
                                                select r).ToList();

                statutoryOutcomeItem = (from r in statutoryOutcomeItemList
                                        select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return statutoryOutcomeItem;
        }

        public List<Statutory_Outcome_Item> GetListOfStatutoryOutcomeItems()
        {
            List<Statutory_Outcome_Item> statutoryOutcomeItems;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var statutoryOutcomeItemList = (from r in dbContext.Statutory_Outcome_Items
                                                    select r).ToList();

                    statutoryOutcomeItems = (from r in statutoryOutcomeItemList
                                             select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return statutoryOutcomeItems;
        }
    }
}
