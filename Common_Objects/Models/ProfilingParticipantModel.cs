using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ProfilingParticipantModel
    {
        public Profiling_Participant GetSpecificProfilingParticipant(int profilingParticipantId)
        {
            Profiling_Participant profilingParticipant;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var profilingParticipantList = (from x in dbContext.Profiling_Participants
                                                where x.Profiling_Participant_Id.Equals(profilingParticipantId)
                                                select x).ToList();

                profilingParticipant = (from r in profilingParticipantList
                                        select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return profilingParticipant;
        }

        public List<Profiling_Participant> GetListOfProfilingParticipants(bool showInActive, bool showDeleted)
        {
            List<Profiling_Participant> profilingParticipants;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var profilingParticipantList = (from x in dbContext.Profiling_Participants
                                                select x).ToList();

                profilingParticipants = (from x in profilingParticipantList
                                      select x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return profilingParticipants;
        }

        public Profiling_Participant CreateProfilingParticipant(int profilingInstanceId, int personId, int relationshipToHeadId, int educationLevelId, string createdBy, DateTime dateCreated)
        {
            Profiling_Participant newProfilingParticipant;

            var dbContext = new SDIIS_DatabaseEntities();

            var profilingParticipant = new Profiling_Participant()
            {
                Profiling_Instance_Id = profilingInstanceId,
                Person_Id = personId,
                Relation_To_Head_Id = relationshipToHeadId,
                Education_Level_Id = educationLevelId,
                Created_By = createdBy,
                Date_Created = dateCreated
            };

            try
            {
                newProfilingParticipant = dbContext.Profiling_Participants.Add(profilingParticipant);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var exMsg = ex.Message;
                return null;
            }

            return newProfilingParticipant;
        }

        public Profiling_Participant EditProfilingParticipant(int profilingParticipantId, int profilingInstanceId, int personId, int relationshipToHeadId, int educationLevelId, string modifiedBy, DateTime? dateLastModified)
        {
            Profiling_Participant editProfilingParticipant;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                editProfilingParticipant = (from x in dbContext.Profiling_Participants
                                            where x.Profiling_Participant_Id.Equals(profilingParticipantId)
                                            select x).FirstOrDefault();

                if (editProfilingParticipant == null) return null;

                editProfilingParticipant.Profiling_Instance_Id = profilingInstanceId;
                editProfilingParticipant.Person_Id = personId;
                editProfilingParticipant.Relation_To_Head_Id = relationshipToHeadId;
                editProfilingParticipant.Education_Level_Id = educationLevelId;
                editProfilingParticipant.Modified_By = modifiedBy;
                editProfilingParticipant.Date_Last_Modified = dateLastModified;

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }

            return editProfilingParticipant;
        }

        public bool RemoveProfilingParticipant(int profilingParticipantId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var participantToDelete = (from x in dbContext.Profiling_Participants
                                           where x.Profiling_Participant_Id.Equals(profilingParticipantId)
                                           select x).FirstOrDefault();

                if (participantToDelete == null) return false;

                dbContext.Profiling_Participants.Remove(participantToDelete);

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
