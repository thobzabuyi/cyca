using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class QuestionnaireQuestionModel
    {
        public Questionnaire_Question GetSpecificQestionnaireQuestion(int questionnaireQuestionId)
        {
            Questionnaire_Question questionnaireQuestion;

            var dbContext = new SDIIS_DatabaseEntities();
            try
            {
                var questionnaireQuestionList = (from x in dbContext.Questionnaire_Questions
                                                 where x.Questionnaire_Question_Id.Equals(questionnaireQuestionId)
                                                 select x).ToList();

                questionnaireQuestion = (from x in questionnaireQuestionList
                                         select x).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return questionnaireQuestion;
        }

        public List<Questionnaire_Question> GetListOfQuestionnaireQuestions(bool showInActive, bool showDeleted)
        {
            List<Questionnaire_Question> questionnaireQuestions;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var questionnaireQuestionList = (from x in dbContext.Questionnaire_Questions
                                                 where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                                 where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                                 select x).ToList();

                questionnaireQuestions = (from x in questionnaireQuestionList
                                          select x).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return questionnaireQuestions;
        }

        public List<Questionnaire_Question> GetListOfQuestionnaireQuestions(bool showInActive, bool showDeleted, int sectionId)
        {
            List<Questionnaire_Question> questionnaireQuestions;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var questionnaireQuestionList = (from x in dbContext.Questionnaire_Questions
                                                 where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                                 where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                                 where x.Questionnaire_Section_Id.Equals(sectionId)
                                                 select x).ToList();

                questionnaireQuestions = (from x in questionnaireQuestionList
                                          select x).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return questionnaireQuestions;
        }

        public Questionnaire_Question CreateQuestionnaireQuestion(int sectionId, string questionText, int? dependsOnQuestionId, int? dependsOnOptionId, bool isRequired, bool isActive, bool isDeleted, DateTime dateCreated, string createdBy)
        {
            Questionnaire_Question newQuestionnaireQuestion;

            var dbContext = new SDIIS_DatabaseEntities();

            var questionnaireQuestion = new Questionnaire_Question
            {
                Questionnaire_Section_Id = sectionId,
                Sort_Order = 9999,
                Question_Text = questionText,
                Depends_on_Question_Id = dependsOnQuestionId,
                Depends_on_Question_Option_Id = dependsOnOptionId,
                Is_Required = isRequired,
                Is_Active = isActive,
                Is_Deleted = isDeleted,
                Date_Created = dateCreated,
                Created_By = createdBy
            };

            try
            {
                newQuestionnaireQuestion = dbContext.Questionnaire_Questions.Add(questionnaireQuestion);
                dbContext.SaveChanges();

                var sortQuestions = dbContext.Questionnaire_Questions.Where(w => w.Questionnaire_Section_Id.Equals(sectionId)).OrderBy(x => x.Sort_Order).ToList();

                var sortOrder = 1;
                foreach (var s in sortQuestions)
                {
                    if (s.Is_Active)
                    {
                        s.Sort_Order = sortOrder;
                        sortOrder += 2;
                    }
                    else
                    {
                        s.Sort_Order = 9999;
                    }
                }

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }

            return newQuestionnaireQuestion;
        }

        public Questionnaire_Question EditQuestionnaireQuestion(int questionId, int sectionId, string questionText, int? dependsOnQuestionId, int? dependsOnOptionId, bool isRequired, bool isActive, bool isDeleted, DateTime? dateLastModified, string modifiedBy)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editQuestionnaireQuestion = (from x in dbContext.Questionnaire_Questions
                                                 where x.Questionnaire_Question_Id.Equals(questionId)
                                                 select x).FirstOrDefault();

                if (editQuestionnaireQuestion == null) return null;

                editQuestionnaireQuestion.Questionnaire_Section_Id = sectionId;
                editQuestionnaireQuestion.Question_Text = questionText;
                editQuestionnaireQuestion.Depends_on_Question_Id = dependsOnQuestionId;
                editQuestionnaireQuestion.Depends_on_Question_Option_Id = dependsOnOptionId;
                editQuestionnaireQuestion.Is_Required = isRequired;
                editQuestionnaireQuestion.Is_Active = isActive;
                editQuestionnaireQuestion.Is_Deleted = isDeleted;
                editQuestionnaireQuestion.Date_Last_Modified = dateLastModified;
                editQuestionnaireQuestion.Modified_By = modifiedBy;

                dbContext.SaveChanges();

                var sortQuestions = dbContext.Questionnaire_Questions.Where(w => w.Questionnaire_Section_Id.Equals(sectionId)).OrderBy(x => x.Sort_Order).ToList();

                var sortOrder = 1;
                foreach (var s in sortQuestions)
                {
                    if (s.Is_Active)
                    {
                        s.Sort_Order = sortOrder;
                        sortOrder += 2;
                    }
                    else
                    {
                        s.Sort_Order = 9999;
                    }
                }

                dbContext.SaveChanges();

                return editQuestionnaireQuestion;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Questionnaire_Question SetQuestionnaireQuestionIsActive(int questionId, bool isActive)
        {
            Questionnaire_Question editQuestion;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editQuestion = (from x in dbContext.Questionnaire_Questions
                                where x.Questionnaire_Question_Id.Equals(questionId)
                                select x).FirstOrDefault();

                    if (editQuestion == null) return null;

                    editQuestion.Is_Active = isActive;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editQuestion;
        }

        public Questionnaire_Question SetQuestionnaireQuestionIsDeleted(int questionId, bool isDeleted)
        {
            Questionnaire_Question editQuestion;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editQuestion = (from x in dbContext.Questionnaire_Questions
                                    where x.Questionnaire_Question_Id.Equals(questionId)
                                    select x).FirstOrDefault();

                    if (editQuestion == null) return null;

                    editQuestion.Is_Deleted = isDeleted;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editQuestion;
        }

        public Questionnaire_Question MoveQuestionnaireQuestionUp(int questionId)
        {
            Questionnaire_Question editQuestion;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editQuestion = (from x in dbContext.Questionnaire_Questions
                                    where x.Questionnaire_Question_Id.Equals(questionId)
                                    select x).FirstOrDefault();

                    if (editQuestion == null) return null;

                    editQuestion.Sort_Order = editQuestion.Sort_Order - 3;

                    var section = editQuestion.Questionnaire_Section;

                    var sortOrder = 1;
                    foreach (var s in section.Questionnaire_Questions.OrderBy(x => x.Sort_Order))
                    {
                        if (s.Is_Active)
                        {
                            s.Sort_Order = sortOrder;
                            sortOrder += 2;
                        }
                        else
                        {
                            s.Sort_Order = 9999;
                        }
                    }

                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return editQuestion;
        }

        public Questionnaire_Question MoveQuestionnaireQuestionDown(int questionId)
        {
            Questionnaire_Question editQuestion;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editQuestion = (from x in dbContext.Questionnaire_Questions
                                    where x.Questionnaire_Question_Id.Equals(questionId)
                                    select x).FirstOrDefault();

                    if (editQuestion == null) return null;

                    editQuestion.Sort_Order = editQuestion.Sort_Order + 3;

                    var section = editQuestion.Questionnaire_Section;

                    var sortOrder = 1;
                    foreach (var s in section.Questionnaire_Questions.OrderBy(x => x.Sort_Order))
                    {
                        if (s.Is_Active)
                        {
                            s.Sort_Order = sortOrder;
                            sortOrder += 2;
                        }
                        else
                        {
                            s.Sort_Order = 9999;
                        }
                    }

                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return editQuestion;
        }
    }
}
