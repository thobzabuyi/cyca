using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class PopulationGroupModel
    {
        public Population_Group GetSpecificPopulationGroup(int populationGroupId)
        {
            Population_Group populationGroup;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var populationGroupList = (from r in dbContext.Population_Groups
                                           where r.Population_Group_Id.Equals(populationGroupId)
                                           select r).ToList();

                populationGroup = (from r in populationGroupList
                                   select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return populationGroup;
        }

        public List<Population_Group> GetListOfPopulationGroups()
        {
            List<Population_Group> populationGroups;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var populationGroupList = (from r in dbContext.Population_Groups
                                               select r).ToList();

                    populationGroups = (from r in populationGroupList
                                        select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return populationGroups;
        }
    }
}
