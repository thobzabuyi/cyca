using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class ProfilingAnswerModel
    {
        public Profiling_Answer GetSpecificProfilingAnswer(int profilingAnswerId)
        {
            Profiling_Answer profilingAnswer;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var profilingAnswerList = (from x in dbContext.Profiling_Answers
                                           where x.Profiling_Answer_Id.Equals(profilingAnswerId)
                                           select x).ToList();

                profilingAnswer = (from x in profilingAnswerList
                                   select x).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return profilingAnswer;
        }

        public List<Profiling_Answer> GetListOfProfilingAnswers()
        {
            List<Profiling_Answer> profilingAnswers;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var profilingAnswersList = (from x in dbContext.Profiling_Answers
                                                select x).ToList();

                    profilingAnswers = (from x in profilingAnswersList
                                        select x).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return profilingAnswers;
        }

        public List<Profiling_Answer> GetListOfProfilingAnswers(List<int> profilingInstanceIds)
        {
            List<Profiling_Answer> profilingAnswers;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var profilingAnswersList = (from x in dbContext.Profiling_Answers
                                            where profilingInstanceIds.Contains(x.Profiling_Instance_Id)
                                            select x).ToList();

                profilingAnswers = (from x in profilingAnswersList
                                    select x).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return profilingAnswers;
        }

        public Profiling_Answer CreateProfilingAnswer(int profilingInstanceId, int questionnaireQuestionId, int questionnaireColumnId, int? householdMemberId, string answerValue, string createdBy, DateTime createdDate, string modifiedBy, DateTime? dateLastModified)
        {
            Profiling_Answer newProfilingAnswer;

            var dbContext = new SDIIS_DatabaseEntities();

            var profilingAnswer = new Profiling_Answer()
            {
                Profiling_Instance_Id = profilingInstanceId,
                Questionnaire_Question_Id = questionnaireQuestionId,
                Questionnaire_Question_Column_Id = questionnaireColumnId,
                Household_Member_Number = householdMemberId,
                Answer_Value = answerValue,
                Created_By = createdBy,
                Date_Created = createdDate,
                Modified_By = modifiedBy,
                Date_Last_Modified = dateLastModified
            };

            try
            {
                newProfilingAnswer = dbContext.Profiling_Answers.Add(profilingAnswer);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var exMsg = ex.Message;
                return null;
            }

            return newProfilingAnswer;
        }

        public Profiling_Answer SaveProfilingAnswer(int questionnaireQuestionId, int questionnaireColumnId, int profilingInstanceId, int? householdMemberNumber, string answerValue, string userName)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            var profilingAnswer = new Profiling_Answer() { Questionnaire_Question_Id = questionnaireQuestionId, Questionnaire_Question_Column_Id = questionnaireColumnId, Profiling_Instance_Id = profilingInstanceId, Household_Member_Number = householdMemberNumber, Answer_Value = answerValue };

            try
            {
                // Does this question answer already exist in the database?
                var existingQuestions = (from x in dbContext.Profiling_Answers
                                         where x.Questionnaire_Question_Id.Equals(questionnaireQuestionId)
                                         where x.Questionnaire_Question_Column_Id.Equals(questionnaireColumnId)
                                         where x.Profiling_Instance_Id.Equals(profilingInstanceId)
                                         select x).ToList();

                if (householdMemberNumber != null)
                    existingQuestions.RemoveAll(x => x.Household_Member_Number != householdMemberNumber);

                if (existingQuestions.Count == 0)
                {
                    // Answer does not exist, add it
                    profilingAnswer.Created_By = userName;
                    profilingAnswer.Date_Created = DateTime.Now;

                    var newProfilingAnswer = dbContext.Profiling_Answers.Add(profilingAnswer);

                    dbContext.SaveChanges();

                    return newProfilingAnswer;
                }
                else if (existingQuestions.Count == 1)
                {
                    var updateAnswerId = existingQuestions.First().Profiling_Answer_Id;

                    var editQuestion = (from x in dbContext.Profiling_Answers
                                        where x.Profiling_Answer_Id.Equals(updateAnswerId)
                                        select x).First();

                    editQuestion.Modified_By = userName;
                    editQuestion.Date_Last_Modified = DateTime.Now;
                    editQuestion.Answer_Value = answerValue;

                    dbContext.SaveChanges();

                    return editQuestion;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Profiling_Answer EditProfilingAnswer(int profilingAnswerId, int questionnaireQuestionId, int questionnaireColumnId, int profilingInstanceId, int? householdMemberNumber, string answerValue, string modifiedBy, DateTime? dateLastModified)
        {
            Profiling_Answer editProfilingAnswer;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                editProfilingAnswer = (from x in dbContext.Profiling_Answers
                                       where x.Profiling_Answer_Id.Equals(profilingAnswerId)
                                       select x).FirstOrDefault();

                if (editProfilingAnswer == null) return null;

                editProfilingAnswer.Questionnaire_Question_Id = questionnaireQuestionId;
                editProfilingAnswer.Questionnaire_Question_Column_Id = questionnaireColumnId;
                editProfilingAnswer.Profiling_Instance_Id = profilingInstanceId;
                editProfilingAnswer.Household_Member_Number = householdMemberNumber;
                editProfilingAnswer.Answer_Value = answerValue;
                editProfilingAnswer.Modified_By = modifiedBy;
                editProfilingAnswer.Date_Last_Modified = dateLastModified;

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }

            return editProfilingAnswer;
        }

    }
}
