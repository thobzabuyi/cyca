using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class JobPositionModel
    {
        public Job_Position GetSpecificJobPosition(int job_positionId)
        {
            Job_Position jobPosition;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var jobPositionList = (from r in dbContext.Job_Positions
                                       where r.Job_Position_Id.Equals(job_positionId)
                                       select r).ToList();

                jobPosition = (from r in jobPositionList
                               select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return jobPosition;
        }

        public List<Job_Position> GetListOfJobPositions()
        {
            List<Job_Position> jobPositions;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var jobPositionList = (from r in dbContext.Job_Positions
                                           select r).ToList();

                    jobPositions = (from r in jobPositionList
                                    select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return jobPositions;
        }

        
    }
}