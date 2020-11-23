using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class CommunityProfilingAnswerModel
    {
        public Community_Profiling_Answer GetSpecificCommunityProfilingAnswer(int profilingAnswerId)
        {
            Community_Profiling_Answer communityProfilingAnswer;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var profilingAnswerList = (from x in dbContext.Community_Profiling_Answers
                                           where x.Profiling_Answer_Id.Equals(profilingAnswerId)
                                           select x).ToList();

                communityProfilingAnswer = (from x in profilingAnswerList
                                            select x).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return communityProfilingAnswer;
        }

        public List<Community_Profiling_Answer> GetListOfCommunityProfilingAnswers()
        {
            List<Community_Profiling_Answer> communityProfilingAnswers;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var profilingAnswersList = (from x in dbContext.Community_Profiling_Answers
                                            select x).ToList();

                communityProfilingAnswers = (from x in profilingAnswersList
                                             select x).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return communityProfilingAnswers;
        }

        public Community_Profiling_Answer CreateCommunityProfilingAnswer(int profilingInstanceId, int questionnaireQuestionId, int questionnaireColumnId, int? householdMemberId, string answerValue, string createdBy, DateTime createdDate, string modifiedBy, DateTime? dateLastModified)
        {
            Community_Profiling_Answer newCommunityProfilingAnswer;

            var dbContext = new SDIIS_DatabaseEntities();

            var profilingAnswer = new Community_Profiling_Answer()
            {
                Profiling_Instance_Id = profilingInstanceId,
                Questionnaire_Question_Id = questionnaireQuestionId,
                Questionnaire_Question_Column_Id = questionnaireColumnId,
                Answer_Value = answerValue,
                Created_By = createdBy,
                Date_Created = createdDate,
                Modified_By = modifiedBy,
                Date_Last_Modified = dateLastModified
            };

            try
            {
                newCommunityProfilingAnswer = dbContext.Community_Profiling_Answers.Add(profilingAnswer);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var exMsg = ex.Message;
                return null;
            }

            return newCommunityProfilingAnswer;
        }

        public Community_Profiling_Answer SaveCommunityProfilingAnswer(int questionnaireQuestionId, int questionnaireColumnId, int profilingInstanceId, string answerValue, string userName)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var communityProfilingAnswer = new Community_Profiling_Answer() { Questionnaire_Question_Id = questionnaireQuestionId, Questionnaire_Question_Column_Id = questionnaireColumnId, Profiling_Instance_Id = profilingInstanceId, Answer_Value = answerValue };

            try
            {
                // Does this question answer already exist in the database?
                var existingQuestion = (from x in dbContext.Community_Profiling_Answers
                                        where x.Questionnaire_Question_Id.Equals(questionnaireQuestionId)
                                        where x.Questionnaire_Question_Column_Id.Equals(questionnaireColumnId)
                                        where x.Profiling_Instance_Id.Equals(profilingInstanceId)
                                        select x).FirstOrDefault();

                if (existingQuestion == null)
                {
                    // Answer does not exist, add it
                    communityProfilingAnswer.Created_By = userName;
                    communityProfilingAnswer.Date_Created = DateTime.Now;

                    var newProfilingAnswer = dbContext.Community_Profiling_Answers.Add(communityProfilingAnswer);

                    dbContext.SaveChanges();

                    return newProfilingAnswer;
                }
                else
                {
                    existingQuestion.Modified_By = userName;
                    existingQuestion.Date_Last_Modified = DateTime.Now;
                    existingQuestion.Answer_Value = answerValue;

                    dbContext.SaveChanges();

                    return existingQuestion;
                }
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return null;
            }
        }
    }
}
