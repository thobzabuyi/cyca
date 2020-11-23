using Common_Objects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class VEPServices
    {
        public int serviceid { get; set; }
        public VEP_Services GetSpecificServicese(int serviceId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_Services.SingleOrDefault(a => a.ServiceTypeId == serviceId);
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public VEP_Services GetServiceById(int serviceId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_Services.Find(serviceId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public VEP_ServiceType GetServiceType(int serviceId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_ServiceType.Find(serviceId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<VEP_Services> GetListOfSpecificServicese(int caseId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_Services.Where(a => a.CaseId == caseId).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<VEP_Services> GetSpecificServices(int caseId)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_Services.Where(a => a.CaseId == caseId).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<VEP_Services> GetListOfServiceType()
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                return dbContext.VEP_Services.ToList();
            }
            catch (Exception)
            {
                return null;
            }

        }

        public void CreateService(VEPServicesViewModel model)
        {
            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                VEP_Services servicesTable = new VEP_Services();

                servicesTable.CaseId = model.Caseid;
                servicesTable.ServiceId = model.Serviceid;
                //servicesTable.PersonId = model.clientID;
                servicesTable.ServiceTypeId = model.ServiceTypeid;
                servicesTable.ServiceNotes = model.ServiceNotes.Trim();
                servicesTable.DateCreated = DateTime.Now;
                //Value 1 means record is not active(Not deleted)
                servicesTable.isActive = 1;

                dbContext.VEP_Services.Add(servicesTable);
                dbContext.SaveChanges();

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {

                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }

        //public void CreateReferral(VEPReferalsViewModel vmodel)
        //{
        //    var dbContext = new SDIIS_DatabaseEntities();
        //    try
        //    {
        //        var OldObj = (from c in dbContext.VEP_Referals
        //                      where c.CaseId == (vmodel.Caseid)
        //                      select c).FirstOrDefault();
        //        if (OldObj != null)
        //        {
        //            OldObj.CreateDate = vmodel.CreateDate;
        //            OldObj.Createdby = vmodel.Createdby;
        //            OldObj.FromDepartment = vmodel.FromDepartment;
        //            OldObj.ToDepartment = vmodel.ToDepartment;
        //            OldObj.Notes = vmodel.Notes;
        //            dbContext.SaveChanges();
        //        }
        //        else
        //        {
        //            var NewObj = new VEP_Referals();
        //            NewObj.CreateDate = vmodel.CreateDate;
        //            NewObj.Createdby = vmodel.Createdby;
        //            NewObj.FromDepartment = vmodel.FromDepartment;
        //            NewObj.ToDepartment = vmodel.ToDepartment;
        //            NewObj.Notes = vmodel.Notes;
        //            NewObj.CaseId = vmodel.Caseid;
        //            dbContext.VEP_Referals.Add(NewObj);
        //            dbContext.SaveChanges();
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //public VEPReferalsViewModel GetReferral(int Id)
        //{
        //     SDIIS_DatabaseEntities dbContext = new SDIIS_DatabaseEntities();

        //     var DataOnDB = (from a in dbContext.VEP_Referals
        //                    where a.CaseId == Id
        //                    select a).FirstOrDefault();
        //    VEPReferalsViewModel Model = new VEPReferalsViewModel();

        //    Model.CreateDate = DataOnDB.CreateDate;
        //    Model.Createdby = DataOnDB.Createdby;
        //    Model.FromDepartment = DataOnDB.FromDepartment;
        //    Model.ToDepartment = DataOnDB.ToDepartment;
        //    Model.Notes = DataOnDB.Notes;
        //    return Model;

        //}



    }
}
