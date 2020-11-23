using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ActionTakenModel
    {
        public Action_Taken GetSpecificActionTaken(int actionTakenId)
        {
            Action_Taken actionTaken;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var actionTakenItemsList = (from r in dbContext.Action_Taken_Items
                                       where r.Action_Taken_Id.Equals(actionTakenId)
                                       select r).ToList();

                actionTaken = (from r in actionTakenItemsList
                               select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return actionTaken;
        }

        public List<Action_Taken> GetListOfActionTakenItems()
        {
            List<Action_Taken> actionTakenItems;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var actionTakenItemsList = (from r in dbContext.Action_Taken_Items
                                            select r).ToList();

                actionTakenItems = (from r in actionTakenItemsList
                                    select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return actionTakenItems;
        }

        public List<Action_Taken> GetListOfActionTakenItems(int incidentId)
        {
            List<Action_Taken> actionTakenItems;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var actionTakenItemsList = (from r in dbContext.Action_Taken_Items
                                            where r.Incident_Id.Equals(incidentId)
                                            select r).ToList();

                actionTakenItems = (from r in actionTakenItemsList
                                    select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return actionTakenItems;
        }

        public Action_Taken CreateActionTaken(int incidentId, int officeTypeId, DateTime? dateActionTakenNoted, string actionTakenDescription, string wayFormwardDescription)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var actionTaken = new Action_Taken() { Incident_Id = incidentId, Office_Type_Id = officeTypeId, Date_Action_Taken_Noted = dateActionTakenNoted, Action_Taken_Description = actionTakenDescription, Way_Forward_Description = wayFormwardDescription };

            try
            {
                var newActionTaken = dbContext.Action_Taken_Items.Add(actionTaken);

                dbContext.SaveChanges();

                return newActionTaken;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Action_Taken EditActionTaken(int actionTakenId, int incidentId, int officeTypeId, DateTime? dateActionTakenNoted, string actionTakenDescription, string wayFormwardDescription)
        {
            Action_Taken editActionTaken;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editActionTaken = (from a in dbContext.Action_Taken_Items
                                       where a.Action_Taken_Id.Equals(actionTakenId)
                                       select a).FirstOrDefault();

                    if (editActionTaken == null) return null;

                    editActionTaken.Incident_Id = incidentId;
                    editActionTaken.Office_Type_Id = officeTypeId;
                    editActionTaken.Date_Action_Taken_Noted = dateActionTakenNoted;
                    editActionTaken.Action_Taken_Description = actionTakenDescription;
                    editActionTaken.Way_Forward_Description = wayFormwardDescription;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editActionTaken;
        }
    }
}
