using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class QuestionnaireModel
    {
        public Questionnaire GetSpecificQestionnaire(int questionnaireId)
        {
            Questionnaire questionnaire;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var questionnaireList = (from r in dbContext.Questionnaires
                                         where r.Questionnaire_Id.Equals(questionnaireId)
                                         select r).ToList();

                questionnaire = (from r in questionnaireList
                                 select r).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return questionnaire;
        }

        public List<Questionnaire> GetListOfQuestionnaires(bool showInActive, bool showDeleted)
        {
            List<Questionnaire> questionnaires;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var questionnaireList = (from x in dbContext.Questionnaires
                                             where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                             where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                             select x).ToList();

                    questionnaires = (from r in questionnaireList
                                      select r).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return questionnaires;
        }

        public Questionnaire CreateQuestionnaire(int profilingToolId, string description, bool isActive, bool isDeleted, DateTime dateCreated, string createdBy)
        {
            Questionnaire newQuestionnaire;

            var dbContext = new SDIIS_DatabaseEntities();

            var questionnaire = new Questionnaire()
            {
                Profiling_Tool_Id = profilingToolId,
                Description = description,
                Is_Active = isActive,
                Is_Deleted = isDeleted,
                Date_Created = dateCreated,
                Created_By = createdBy
            };

            try
            {
                newQuestionnaire = dbContext.Questionnaires.Add(questionnaire);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }

            return newQuestionnaire;
        }

        public Questionnaire EditQuestionnaire(int questionnaireId, int profilingToolId, string description, bool isActive, bool isDeleted, DateTime? dateLastModified, string modifiedBy)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editQuestionnaire = (from x in dbContext.Questionnaires
                                         where x.Questionnaire_Id.Equals(questionnaireId)
                                         select x).FirstOrDefault();

                if (editQuestionnaire == null) return null;

                editQuestionnaire.Profiling_Tool_Id = profilingToolId;
                editQuestionnaire.Description = description;
                editQuestionnaire.Is_Active = isActive;
                editQuestionnaire.Is_Deleted = isDeleted;
                editQuestionnaire.Date_Last_Modified = dateLastModified;
                editQuestionnaire.Modified_By = modifiedBy;

                dbContext.SaveChanges();

                return editQuestionnaire;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
