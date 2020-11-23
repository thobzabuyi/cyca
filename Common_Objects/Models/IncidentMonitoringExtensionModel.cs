using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class IncidentMonitoringExtensionModel
    {
        public Incident_Monitoring_Normal_Extension GetSpecificNormalMonitoringExtensionItem(int monitoringExtensionItemId)
        {
            Incident_Monitoring_Normal_Extension normalMonitoringExtensionItem;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var normalMonitoringExtensionList = (from x in dbContext.Incident_Monitoring_Normal_Extensions
                                                     where x.Monitoring_Extension_Id.Equals(monitoringExtensionItemId)
                                                     select x).ToList();

                normalMonitoringExtensionItem = (from x in normalMonitoringExtensionList
                                                 select x).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return normalMonitoringExtensionItem;
        }

        public Incident_Monitoring_Form36_Extension GetSpecificForm36MonitoringExtensionItem(int monitoringExtensionItemId)
        {
            Incident_Monitoring_Form36_Extension form36MonitoringExtensionItem;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var form36MonitoringExtensionList = (from x in dbContext.Incident_Monitoring_Form36_Extensions
                                                     where x.Monitoring_Extension_Id.Equals(monitoringExtensionItemId)
                                                     select x).ToList();

                form36MonitoringExtensionItem = (from x in form36MonitoringExtensionList
                                                 select x).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return form36MonitoringExtensionItem;
        }

        public Incident_Monitoring_ChildrensCourt_Extension GetSpecificChildrensCourtMonitoringExtensionItem(int monitoringExtensionItemId)
        {
            Incident_Monitoring_ChildrensCourt_Extension childrensCourtMonitoringExtensionItem;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var childrensCourtMonitoringExtensionList = (from x in dbContext.Incident_Monitoring_ChildrensCourt_Extensions
                                                             where x.Monitoring_Extension_Id.Equals(monitoringExtensionItemId)
                                                             select x).ToList();

                childrensCourtMonitoringExtensionItem = (from x in childrensCourtMonitoringExtensionList
                                                         select x).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return childrensCourtMonitoringExtensionItem;
        }

        public Incident_Monitoring_CriminalCourt_Extension GetSpecificCriminalCourtMonitoringExtensionItem(int monitoringExtensionItemId)
        {
            Incident_Monitoring_CriminalCourt_Extension criminalCourtMonitoringExtensionItem;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var criminalCourtMonitoringExtensionList = (from x in dbContext.Incident_Monitoring_CriminalCourt_Extensions
                                                            where x.Monitoring_Extension_Id.Equals(monitoringExtensionItemId)
                                                            select x).ToList();

                criminalCourtMonitoringExtensionItem = (from x in criminalCourtMonitoringExtensionList
                                                        select x).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return criminalCourtMonitoringExtensionItem;
        }

        public List<Incident_Monitoring_Normal_Extension> GetListOfNormalMonitoringExtensionItems()
        {
            List<Incident_Monitoring_Normal_Extension> normalMonitoringExtensionItems;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var normalMonitoringExtensionList = (from x in dbContext.Incident_Monitoring_Normal_Extensions
                                                     select x).ToList();

                normalMonitoringExtensionItems = (from x in normalMonitoringExtensionList
                                                 select x).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return normalMonitoringExtensionItems;
        }

        public List<Incident_Monitoring_Normal_Extension> GetListOfNormalMonitoringExtensionItems(int incidentMonitoringId)
        {
            List<Incident_Monitoring_Normal_Extension> normalMonitoringExtensionItems;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var normalMonitoringExtensionList = (from x in dbContext.Incident_Monitoring_Normal_Extensions
                                                     where x.Incident_Monitoring_Id.Equals(incidentMonitoringId)
                                                     select x).ToList();

                normalMonitoringExtensionItems = (from x in normalMonitoringExtensionList
                                                  select x).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return normalMonitoringExtensionItems;
        }

        public List<Incident_Monitoring_Form36_Extension> GetListOfForm36MonitoringExtensionItems()
        {
            List<Incident_Monitoring_Form36_Extension> form36MonitoringExtensionItems;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var form36MonitoringExtensionList = (from x in dbContext.Incident_Monitoring_Form36_Extensions
                                                     select x).ToList();

                form36MonitoringExtensionItems = (from x in form36MonitoringExtensionList
                                                  select x).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return form36MonitoringExtensionItems;
        }

        public List<Incident_Monitoring_Form36_Extension> GetListOfForm36MonitoringExtensionItems(int incidentMonitoringId)
        {
            List<Incident_Monitoring_Form36_Extension> form36MonitoringExtensionItems;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var form36MonitoringExtensionList = (from x in dbContext.Incident_Monitoring_Form36_Extensions
                                                     where x.Incident_Monitoring_Id.Equals(incidentMonitoringId)
                                                     select x).ToList();

                form36MonitoringExtensionItems = (from x in form36MonitoringExtensionList
                                                  select x).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return form36MonitoringExtensionItems;
        }

        public List<Incident_Monitoring_ChildrensCourt_Extension> GetListOfChildrensCourtMonitoringExtensionItems()
        {
            List<Incident_Monitoring_ChildrensCourt_Extension> childrensCourtMonitoringExtensionItems;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var childrensCourtMonitoringExtensionList = (from x in dbContext.Incident_Monitoring_ChildrensCourt_Extensions
                                                             select x).ToList();

                childrensCourtMonitoringExtensionItems = (from x in childrensCourtMonitoringExtensionList
                                                          select x).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return childrensCourtMonitoringExtensionItems;
        }

        public List<Incident_Monitoring_ChildrensCourt_Extension> GetListOfChildrensCourtMonitoringExtensionItems(int incidentMonitoringId)
        {
            List<Incident_Monitoring_ChildrensCourt_Extension> childrensCourtMonitoringExtensionItems;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var childrensCourtMonitoringExtensionList = (from x in dbContext.Incident_Monitoring_ChildrensCourt_Extensions
                                                             where x.Incident_Monitoring_Id.Equals(incidentMonitoringId)
                                                             select x).ToList();

                childrensCourtMonitoringExtensionItems = (from x in childrensCourtMonitoringExtensionList
                                                          select x).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return childrensCourtMonitoringExtensionItems;
        }

        public List<Incident_Monitoring_CriminalCourt_Extension> GetListOfCriminalCourtMonitoringExtensionItems()
        {
            List<Incident_Monitoring_CriminalCourt_Extension> criminalCourtMonitoringExtensionItems;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var criminalCourtMonitoringExtensionList = (from x in dbContext.Incident_Monitoring_CriminalCourt_Extensions
                                                            select x).ToList();

                criminalCourtMonitoringExtensionItems = (from x in criminalCourtMonitoringExtensionList
                                                         select x).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return criminalCourtMonitoringExtensionItems;
        }

        public List<Incident_Monitoring_CriminalCourt_Extension> GetListOfCriminalCourtMonitoringExtensionItems(int incidentMonitoringId)
        {
            List<Incident_Monitoring_CriminalCourt_Extension> criminalCourtMonitoringExtensionItems;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var criminalCourtMonitoringExtensionList = (from x in dbContext.Incident_Monitoring_CriminalCourt_Extensions
                                                            where x.Incident_Monitoring_Id.Equals(incidentMonitoringId)
                                                            select x).ToList();

                criminalCourtMonitoringExtensionItems = (from x in criminalCourtMonitoringExtensionList
                                                         select x).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return criminalCourtMonitoringExtensionItems;
        }

        public Incident_Monitoring_Normal_Extension CreateNormalMonitoringExtension(int incidentMonitoringId, DateTime extensionDate)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var normalMonitoringExtensionItem = new Incident_Monitoring_Normal_Extension()
            {
                Incident_Monitoring_Id = incidentMonitoringId,
                Extension_Date = extensionDate
            };

            try
            {
                var newNormalMonitoringExtensionItem = dbContext.Incident_Monitoring_Normal_Extensions.Add(normalMonitoringExtensionItem);

                dbContext.SaveChanges();

                return newNormalMonitoringExtensionItem;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Incident_Monitoring_Form36_Extension CreateForm36MonitoringExtension(int incidentMonitoringId, DateTime extensionDate)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var form36MonitoringExtensionItem = new Incident_Monitoring_Form36_Extension()
            {
                Incident_Monitoring_Id = incidentMonitoringId,
                Extension_Date = extensionDate
            };

            try
            {
                var newForm36MonitoringExtensionItem = dbContext.Incident_Monitoring_Form36_Extensions.Add(form36MonitoringExtensionItem);

                dbContext.SaveChanges();

                return newForm36MonitoringExtensionItem;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Incident_Monitoring_ChildrensCourt_Extension CreateChildrensCourtMonitoringExtension(int incidentMonitoringId, DateTime extensionDate)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var childrensCourtMonitoringExtensionItem = new Incident_Monitoring_ChildrensCourt_Extension()
            {
                Incident_Monitoring_Id = incidentMonitoringId,
                Extension_Date = extensionDate
            };

            try
            {
                var newChildrensCourtMonitoringExtensionItem = dbContext.Incident_Monitoring_ChildrensCourt_Extensions.Add(childrensCourtMonitoringExtensionItem);

                dbContext.SaveChanges();

                return newChildrensCourtMonitoringExtensionItem;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Incident_Monitoring_CriminalCourt_Extension CreateCriminalCourtMonitoringExtension(int incidentMonitoringId, DateTime extensionDate)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var criminalCourtMonitoringExtensionItem = new Incident_Monitoring_CriminalCourt_Extension()
            {
                Incident_Monitoring_Id = incidentMonitoringId,
                Extension_Date = extensionDate
            };

            try
            {
                var newCriminalCourtMonitoringExtensionItem = dbContext.Incident_Monitoring_CriminalCourt_Extensions.Add(criminalCourtMonitoringExtensionItem);

                dbContext.SaveChanges();

                return newCriminalCourtMonitoringExtensionItem;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
