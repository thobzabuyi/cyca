using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ReceptionVisitTypeModel
    {
        public Reception_Visit_Type GetSpecificReceptionVisitType(int receptionVisitTypeId)
        {
            Reception_Visit_Type receptionVisitType;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var receptionVisitTypeItems = (from r in dbContext.Reception_Visit_Types
                                               where r.Reception_Visit_Type_Id.Equals(receptionVisitTypeId)
                                               select r).ToList();

                //agent = PopulateAdditionalItems(agents, dbContext).FirstOrDefault();

                receptionVisitType = (from r in receptionVisitTypeItems
                                      select r).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }

            return receptionVisitType;
        }

        public List<Reception_Visit_Type> GetListOfReceptionVisitTypes()
        {
            List<Reception_Visit_Type> listOfReceptionVisitTypes;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var receptionVisitTypes = (from r in dbContext.Reception_Visit_Types
                                           select r).ToList();

                //listOfAgents = PopulateAdditionalItems(agents, dbContext);

               listOfReceptionVisitTypes = (from r in receptionVisitTypes
                                            select r).ToList(); ;
            }
            catch (Exception ex)
            {
                return null;
            }

            return listOfReceptionVisitTypes;
        }
    }
}
