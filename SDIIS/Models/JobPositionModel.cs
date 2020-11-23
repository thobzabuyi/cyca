using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDIIS.Models
{
    public class JobPositionModel
    {
        public Job_Position GetSpecificJobPosition(int job_positionId)
        {
            Job_Position job_position;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var job_positionList = (from r in dbContext.Job_Positions
                                        where r.Job_Position_Id.Equals(job_positionId)
                                        select r).ToList();

                job_position = (from r in job_positionList
                                select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return job_position;
        }

        public List<Job_Position> GetListOfJobPositions()
        {
            List<Job_Position> job_positions;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var job_positionList = (from r in dbContext.Job_Positions
                                            select r).ToList();

                    job_positions = (from r in job_positionList
                                     select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return job_positions;
        }
    }
}