using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class NatureOfEmploymentModel
    {
        public Nature_of_Employment GetSpecificNatureOfEmployment(int natureOfEmploymentId)
        {
            Nature_of_Employment natureOfEmployment;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var natureOfEmploymentList = (from r in dbContext.Nature_of_Employment_Items
                                              where r.Nature_of_Employment_Id.Equals(natureOfEmploymentId)
                                              select r).ToList();

                natureOfEmployment = (from r in natureOfEmploymentList
                                      select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return natureOfEmployment;
        }

        public List<Nature_of_Employment> GetListOfNatureOfEmploymentItems()
        {
            List<Nature_of_Employment> natureOfEmploymentItems;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var natureOfEmploymentList = (from r in dbContext.Nature_of_Employment_Items
                                              select r).ToList();

                natureOfEmploymentItems = (from r in natureOfEmploymentList
                                           select r).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return natureOfEmploymentItems;
        }
    }
}
