using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
       // 

    public class ACMChildrensCourtModel
    {

        //Create


            //Return a List
        public List<ACM_ChildrensCourtReport> GetListOfChildrensCourtReport()
        {
            List<ACM_ChildrensCourtReport> listOfACMChildrensCourtReport;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var ACMChildrensCourtReports = (from e in dbContext.ACM_ChildrensCourtReport
                                      select e).ToList();

             listOfACMChildrensCourtReport = (from e in ACMChildrensCourtReports
                                                 select e).ToList();
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return null;
            }

            return listOfACMChildrensCourtReport;

        }
    }
}
