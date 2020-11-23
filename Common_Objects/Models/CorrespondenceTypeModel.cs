using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class CorrespondenceTypeModel
    {
        public CPR_Correspondence_Letter GetSpecificCorrespondenceType(int correspondenceTypeId)
        {
            CPR_Correspondence_Letter correspondenceType;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var correspondenceTypeList = (from r in dbContext.CPR_Correspondence_Letter
                                              where r.CPR_Correspondence_Letter_Id.Equals(correspondenceTypeId)
                                              select r).ToList();

                correspondenceType = (from r in correspondenceTypeList
                                      select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return correspondenceType;
        }

        public List<CPR_Correspondence_Letter> GetListOfCorrespondenceTypes()
        {
            List<CPR_Correspondence_Letter> correspondenceTypes;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var correspondenceTypeList = (from r in dbContext.CPR_Correspondence_Letter
                                                  select r).ToList();

                    correspondenceTypes = (from r in correspondenceTypeList
                                           select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return correspondenceTypes;
        }
    }
}
