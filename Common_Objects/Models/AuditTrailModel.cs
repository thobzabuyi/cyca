using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class AuditTrailModel
    {
        public apl_AuditTrial CreateAuditTrail(string taskperformed, string module, string username, string serviceoffice, string organisation)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var aplAuditTrial = new apl_AuditTrial()
            {
                taskperformed = taskperformed,
                datecaptured = DateTime.Now,
                module = module,
                username = username,
                serviceoffice = serviceoffice,
                organisation = organisation
                
            };

            try
            {
                var newaplAuditTrial = dbContext.apl_AuditTrial.Add(aplAuditTrial);

                dbContext.SaveChanges();

                return newaplAuditTrial;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<apl_AuditTrial> GetListOfAuditTrails()
        {
            List<apl_AuditTrial> audittrails;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var audittrailsList = (from r in dbContext.apl_AuditTrial                                            
                                            select r).ToList();

                    audittrails = (from r in audittrailsList
                                   select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return audittrails;
        }

        public List<apl_AuditTrial> GetListOfAuditTrails(string SearchModule, string SearchUsername, string SearchServiceoffice, string SearchOrganisation, DateTime? SearchDateCaptured, string SearchTaskPerformed)
        {
            List<apl_AuditTrial> audittrails;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var audittrailsList = (from r in dbContext.apl_AuditTrial
                                           select r).ToList();
                    if ((SearchModule != null && SearchModule != "")
                    ||
                    (SearchUsername != null && SearchUsername != "")
                    ||
                    (SearchServiceoffice != null && SearchServiceoffice != "")
                    ||
                    (SearchOrganisation != null && SearchOrganisation != "")
                    ||
                    (SearchDateCaptured != null && SearchDateCaptured.ToString() != "")
                    ||
                    (SearchTaskPerformed != null && SearchTaskPerformed != "")
                    )
                    {
                        audittrailsList = (from r in dbContext.apl_AuditTrial
                                           where r.module.Contains(SearchModule) || SearchModule == ""
                                           where r.username.Contains(SearchUsername) || SearchUsername == ""
                                           where r.serviceoffice.Contains(SearchServiceoffice) || SearchServiceoffice == ""
                                           where r.organisation.Contains(SearchOrganisation) || SearchOrganisation == ""
                                           where r.datecaptured==(SearchDateCaptured) || SearchDateCaptured.ToString() == ""
                                           where r.taskperformed.Contains(SearchTaskPerformed) || SearchTaskPerformed == ""
                                           select r).ToList();
                    }
                    audittrails = (from r in audittrailsList                                   
                                   select r).ToList();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {

                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message1 = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message1, raise);
                        }
                    }
                    throw raise;
                }
                //catch (Exception)
                //{
                //    return null;
                //}
            }

            return audittrails;

            
        }

        public void InsertAuditTrail(string username, string taskperformed, string module)
        {
            var serviceOffice = new ServiceOfficeModel();
            var organisation = new OrganizationModel();
            var auditTrail = new AuditTrailModel();

            var userServiceOffice = serviceOffice.GetUserSpecificServiceOffice(username);
            var userOrganisation = organisation.GetUserSpecificOrganisation(username);
            var userAuditTrail = auditTrail.CreateAuditTrail(taskperformed, module, username, userServiceOffice, userOrganisation);
        }

    }
}
