using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class InterviewTypeModel
    {
       public List<ACM_Interview_Type> GetListOfInterviewTypes()
        {
            List<ACM_Interview_Type> interviewtypes;

           using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var interviewTypeList = (from i in dbContext.ACM_Interview_Type select i).ToList();
                   interviewtypes = (from i in interviewTypeList select i).ToList();
                }
               catch (Exception)
                {
                   return null;
               }
           }
          return interviewtypes;
       }

    }
}
