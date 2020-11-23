using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ProfilingToolModel
    {
        public Profiling_Tool GetSpecificProfilingTool(int profilingToolId)
        {
            Profiling_Tool profilingTool;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var profilingToolList = (from r in dbContext.Profiling_Tools
                                         where r.Profiling_Tool_Id.Equals(profilingToolId)
                                         select r).ToList();

                profilingTool = (from r in profilingToolList
                                 select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return profilingTool;
        }

        public List<Profiling_Tool> GetListOfProfilingTools(bool showInActive, bool showDeleted)
        {
            List<Profiling_Tool> profilingTools;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var profilingToolList = (from x in dbContext.Profiling_Tools
                                         where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                         where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                         select x).ToList();

                profilingTools = (from x in profilingToolList
                                  select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return profilingTools;
        }

        public List<Profiling_Tool> GetListOfProfilingTools(bool showInActive, bool showDeleted, int profilingToolTypeId)
        {
            List<Profiling_Tool> profilingTools;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var profilingToolList = (from x in dbContext.Profiling_Tools
                                         where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                         where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                         where x.Profiling_Tool_Type_Id.Equals(profilingToolTypeId)
                                         select x).ToList();

                profilingTools = (from x in profilingToolList
                                  select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return profilingTools;
        }

        public Profiling_Tool CreateProfilingTool(int profilingToolTypeId, string name, string version, string description, int? ownerOrganizationId,
            int? contactPersonId, DateTime? introductionDate, bool isDeprecated, DateTime? deprecationDate, bool isActive, bool isDeleted,
            DateTime dateCreated, string createdBy)
        {
            Profiling_Tool newProfilingTool;

            var dbContext = new SDIIS_DatabaseEntities();

            var profilingTool = new Profiling_Tool()
            {
                Profiling_Tool_Type_Id = profilingToolTypeId,
                Name = name,
                Version = version,
                Description = description,
                Owner_Organization_Id = ownerOrganizationId,
                Contact_Person_id = contactPersonId,
                Introduction_Date = introductionDate,
                IsDeprecated = isDeprecated,
                Deprecation_Date = deprecationDate,
                Is_Active = isActive,
                Is_Deleted = isDeleted,
                Date_Created = dateCreated,
                Created_By = createdBy
            };

            try
            {
                newProfilingTool = dbContext.Profiling_Tools.Add(profilingTool);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }

            return newProfilingTool;
        }

        public Profiling_Tool EditProfilingTool(int profilingToolId, int profilingToolTypeId, string name, string version, string description, int? ownerOrganizationId,
            int? contactPersonId, DateTime? introductionDate, bool isDeprecated, DateTime? deprecationDate, bool isActive, bool isDeleted,
            DateTime? dateLastModified, string modifiedBy)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editProfilingTool = (from i in dbContext.Profiling_Tools
                                         where i.Profiling_Tool_Id.Equals(profilingToolId)
                                         select i).FirstOrDefault();

                if (editProfilingTool == null) return null;

                editProfilingTool.Profiling_Tool_Type_Id = profilingToolTypeId;
                editProfilingTool.Name = name;
                editProfilingTool.Version = version;
                editProfilingTool.Description = description;
                editProfilingTool.Owner_Organization_Id = ownerOrganizationId;
                editProfilingTool.Contact_Person_id = contactPersonId;
                editProfilingTool.Introduction_Date = introductionDate;
                editProfilingTool.IsDeprecated = isDeprecated;
                editProfilingTool.Deprecation_Date = deprecationDate;
                editProfilingTool.Is_Active = isActive;
                editProfilingTool.Is_Deleted = isDeleted;
                editProfilingTool.Date_Last_Modified = dateLastModified;
                editProfilingTool.Modified_By = modifiedBy;

                dbContext.SaveChanges();

                return editProfilingTool;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
