using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ReceptionRegisterModel
    {
        public Reception_Register GetSpecificReceptionRegister(int receptionRegisterId)
        {
            Reception_Register receptionRegister;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var receptionRegisterItems = (from r in dbContext.Reception_Registers
                                              where r.Reception_Register_Id.Equals(receptionRegisterId)
                                              select r).ToList();

                //agent = PopulateAdditionalItems(agents, dbContext).FirstOrDefault();

                receptionRegister = (from r in receptionRegisterItems
                                     select r).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }

            return receptionRegister;
        }

        public List<Reception_Register> GetListOfReceptionRegisterItems(bool showInActive, bool showDeleted)
        {
            List<Reception_Register> listOfReceptionRegisterItems;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var receptionRegisterItems = (from r in dbContext.Reception_Registers
                                              where r.Is_Active || r.Is_Active.Equals(!showInActive)
                                              where !r.Is_Deleted || r.Is_Deleted.Equals(showDeleted)
                                              select r).ToList();

                //listOfAgents = PopulateAdditionalItems(agents, dbContext);

                listOfReceptionRegisterItems = (from r in receptionRegisterItems
                                                select r).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return listOfReceptionRegisterItems;
        }

        public Reception_Register CreateNewReceptionRegister(int? personId, string reasonForVisit, int? receptionVisitTypeId, DateTime? visitDate, int? receptionActionTakenId, DateTime dateCreated, string createdBy, bool isActive, bool isDeleted)
        {
            Reception_Register newReceptionRegister;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                var receptionRegister = new Reception_Register()
                {
                    Person_Id = personId,
                    Reason_For_Visit = reasonForVisit,
                    Reception_Visit_Type_Id = receptionVisitTypeId,
                    Visit_Date = visitDate,
                    Reception_Action_Taken_Id = receptionActionTakenId,
                    Date_Created = dateCreated,
                    Created_By = createdBy,
                    Is_Active = isActive,
                    Is_Deleted = isDeleted
                };

                try
                {
                    newReceptionRegister = dbContext.Reception_Registers.Add(receptionRegister);
                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return newReceptionRegister;
        }

        public Reception_Register EditReceptionRegister(int receptionRegisterId, int? personId, string reasonForVisit, int? receptionVisitTypeId, DateTime? visitDate, int? receptionActionTakenId, DateTime dateLastModified, string modifiedBy, bool isActive, bool isDeleted)
        {
            Reception_Register editReceptionRegister;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editReceptionRegister = (from r in dbContext.Reception_Registers
                                  where r.Reception_Register_Id.Equals(receptionRegisterId)
                                  select r).FirstOrDefault();

                    if (editReceptionRegister == null) return null;

                    editReceptionRegister.Person_Id = personId;
                    editReceptionRegister.Reason_For_Visit = reasonForVisit;
                    editReceptionRegister.Reception_Visit_Type_Id = receptionVisitTypeId;
                    editReceptionRegister.Visit_Date = visitDate;
                    editReceptionRegister.Reception_Action_Taken_Id = receptionActionTakenId;
                    editReceptionRegister.Date_Last_Modified = dateLastModified;
                    editReceptionRegister.Modified_By = modifiedBy;
                    editReceptionRegister.Is_Active = isActive;
                    editReceptionRegister.Is_Deleted = isDeleted;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editReceptionRegister;
        }
    }
}
