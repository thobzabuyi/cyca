using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ReceptionActionTakenModel
    {
        public Reception_Action_Taken GetSpecificReceptionActionTakenType(int receptionActionTakenId)
        {
            Reception_Action_Taken receptionActionTaken;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var receptionActionTakenItems = (from r in dbContext.Reception_Action_Taken_Items
                                                 where r.Reception_Action_Taken_Id.Equals(receptionActionTakenId)
                                                 select r).ToList();

                //agent = PopulateAdditionalItems(agents, dbContext).FirstOrDefault();

                receptionActionTaken = (from r in receptionActionTakenItems
                                        select r).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }

            return receptionActionTaken;
        }

        public List<Reception_Action_Taken> GetListOfReceptionActionTakenItems()
        {
            List<Reception_Action_Taken> listOfReceptionActionTakenItems;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var receptionActionTakenItems = (from r in dbContext.Reception_Action_Taken_Items
                                                 select r).ToList();

                //listOfAgents = PopulateAdditionalItems(agents, dbContext);

                listOfReceptionActionTakenItems = (from r in receptionActionTakenItems
                                                   select r).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return listOfReceptionActionTakenItems;
        }
    }
}
