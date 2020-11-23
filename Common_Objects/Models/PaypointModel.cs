using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class PaypointModel
    {
        public Paypoint GetSpecificPaypoint(int paypointId)
        {
            Paypoint paypoint;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var paypointList = (from r in dbContext.Paypoints
                                    where r.Paypoint_Id.Equals(paypointId)
                                    select r).ToList();

                paypoint = (from r in paypointList
                            select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return paypoint;
        }

        public List<Paypoint> GetListOfPayPoints()
        {
            List<Paypoint> paypoints;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var paypointList = (from r in dbContext.Paypoints
                                        select r).ToList();

                    paypoints = (from r in paypointList
                                 select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return paypoints;
        }
    }
}