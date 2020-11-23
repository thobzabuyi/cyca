using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ProfilingToolTypeModel
    {
        public Profiling_Tool_Type GetSpecificProfilingTool(int profilingToolTypeId)
        {
            Profiling_Tool_Type profilingToolType;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var profilingToolTypeList = (from r in dbContext.Profiling_Tool_Types
                                             where r.Profiling_Tool_Type_Id.Equals(profilingToolTypeId)
                                             select r).ToList();

                profilingToolType = (from r in profilingToolTypeList
                                     select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return profilingToolType;
        }

        public List<Profiling_Tool_Type> GetListOfProfilingToolTypes()
        {
            List<Profiling_Tool_Type> profilingToolTypeTypes;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var profilingToolTypeList = (from x in dbContext.Profiling_Tool_Types
                                             select x).ToList();

                profilingToolTypeTypes = (from x in profilingToolTypeList
                                          select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return profilingToolTypeTypes;
        }
    }
}
