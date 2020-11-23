using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class IncomeRangeModel
    {
        public Income_Range GetSpecificIncomeRange(int incomeRangeId)
        {
            Income_Range incomeRange;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var incomeRangeList = (from r in dbContext.Income_Ranges
                                       where r.Income_Range_Id.Equals(incomeRangeId)
                                       select r).ToList();

                incomeRange = (from r in incomeRangeList
                               select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return incomeRange;
        }

        public List<Income_Range> GetListOfIncomeRanges()
        {
            List<Income_Range> incomeRanges;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var incomeRangeList = (from r in dbContext.Income_Ranges
                                           select r).ToList();

                    incomeRanges = (from r in incomeRangeList
                                    select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return incomeRanges;
        }
    }
}
