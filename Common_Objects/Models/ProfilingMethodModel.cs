using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ProfilingMethodModel
    {
        public NISIS_Profiling_Method GetSpecificProfilingMethod(int profilingMethodId)
        {
            NISIS_Profiling_Method profilingMethod;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var profilingMethodList = (from r in dbContext.NISIS_Profiling_Method_Items
                                           where r.NISIS_Profiling_Method_Id.Equals(profilingMethodId)
                                           select r).ToList();

                profilingMethod = (from r in profilingMethodList
                                   select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return profilingMethod;
        }

        public List<NISIS_Profiling_Method> GetListOfProfilingMethods()
        {
            List<NISIS_Profiling_Method> profilingMethods;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var profilingMethodList = (from x in dbContext.NISIS_Profiling_Method_Items
                                           select x).ToList();

                profilingMethods = (from x in profilingMethodList
                                    select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return profilingMethods;
        }
    }
}
