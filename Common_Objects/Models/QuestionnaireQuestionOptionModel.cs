using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class QuestionnaireQuestionOptionModel
    {
        public Questionnaire_Question_Option GetSpecificQestionnaireQuestionOption(int questionnaireQuestionOptionId)
        {
            Questionnaire_Question_Option questionnaireQuestionOption;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var questionnaireQuestionOptionList = (from x in dbContext.Questionnaire_Question_Options
                                                       where x.Question_Option_Id.Equals(questionnaireQuestionOptionId)
                                                       select x).ToList();

                questionnaireQuestionOption = (from x in questionnaireQuestionOptionList
                                               select x).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return questionnaireQuestionOption;
        }

        public List<Questionnaire_Question_Option> GetListOfQuestionnaireQuestionOptions(bool showInActive, bool showDeleted)
        {
            List<Questionnaire_Question_Option> questionnaireQuestionOptions;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var questionnaireQuestionOptionsList = (from x in dbContext.Questionnaire_Question_Options
                                                        where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                                        where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                                        select x).ToList();

                questionnaireQuestionOptions = (from x in questionnaireQuestionOptionsList
                                                select x).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return questionnaireQuestionOptions;
        }

        public List<Questionnaire_Question_Option> GetListOfQuestionnaireQuestionOptions(bool showInActive, bool showDeleted, int questionId)
        {
            List<Questionnaire_Question_Option> questionnaireQuestionOptions;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var questionnaireQuestionOptionsList = (from x in dbContext.Questionnaire_Question_Options
                                                        where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                                        where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                                        where x.Questionnaire_Question_Id.Equals(questionId)
                                                        select x).ToList();

                questionnaireQuestionOptions = (from x in questionnaireQuestionOptionsList
                                                select x).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return questionnaireQuestionOptions;
        }

        public Questionnaire_Question_Option CreateQuestionnaireQuestionOption(int questionId, string optionText, bool isActive, bool isDeleted, DateTime dateCreated, string createdBy)
        {
            Questionnaire_Question_Option newQuestionnaireQuestionOption;

            var dbContext = new SDIIS_DatabaseEntities();

            var questionnaireQuestionOption = new Questionnaire_Question_Option
            {
                Questionnaire_Question_Id = questionId,
                Option_Text = optionText,
                Is_Active = isActive,
                Is_Deleted = isDeleted,
                Date_Created = dateCreated,
                Created_By = createdBy
            };

            try
            {
                newQuestionnaireQuestionOption = dbContext.Questionnaire_Question_Options.Add(questionnaireQuestionOption);
                dbContext.SaveChanges();

                // Reset Option Ids
                var options = (from x in dbContext.Questionnaire_Question_Options
                               where x.Questionnaire_Question_Id.Equals(questionId)
                               select x).OrderBy(x => x.Question_Option_Id).ToList();

                var index = 1;
                foreach (var o in options)
                {
                    o.Option_Id = index;
                    if (o.Question_Option_Id.Equals(newQuestionnaireQuestionOption.Question_Option_Id)) newQuestionnaireQuestionOption.Option_Id = index;

                    index++;
                }

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }

            return newQuestionnaireQuestionOption;
        }

        public Questionnaire_Question_Option EditQuestionnaireQuestionOption(int questionOptionId, int questionId, string optionText, bool isActive, bool isDeleted, DateTime? dateLastModified, string modifiedBy)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editQuestionnaireQuestionOption = (from x in dbContext.Questionnaire_Question_Options
                                                       where x.Question_Option_Id.Equals(questionOptionId)
                                                       select x).FirstOrDefault();

                if (editQuestionnaireQuestionOption == null) return null;

                editQuestionnaireQuestionOption.Option_Text = optionText;
                editQuestionnaireQuestionOption.Is_Active = isActive;
                editQuestionnaireQuestionOption.Is_Deleted = isDeleted;
                editQuestionnaireQuestionOption.Date_Last_Modified = dateLastModified;
                editQuestionnaireQuestionOption.Modified_by = modifiedBy;

                dbContext.SaveChanges();

                // Reset Option Ids
                var options = (from x in dbContext.Questionnaire_Question_Options
                               where x.Questionnaire_Question_Id.Equals(questionId)
                               select x).OrderBy(x => x.Question_Option_Id).ToList();

                var index = 1;
                foreach (var o in options)
                {
                    o.Option_Id = index;
                    if (o.Question_Option_Id.Equals(editQuestionnaireQuestionOption.Question_Option_Id)) editQuestionnaireQuestionOption.Option_Id = index;

                    index++;
                }

                dbContext.SaveChanges();

                return editQuestionnaireQuestionOption;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Questionnaire_Question_Option SetQuestionnaireQuestionOptionIsActive(int questionOptionId, bool isActive)
        {
            Questionnaire_Question_Option editQuestionOption;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editQuestionOption = (from x in dbContext.Questionnaire_Question_Options
                                          where x.Question_Option_Id.Equals(questionOptionId)
                                          select x).FirstOrDefault();

                    if (editQuestionOption == null) return null;

                    editQuestionOption.Is_Active = isActive;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editQuestionOption;
        }

        public Questionnaire_Question_Option SetQuestionnaireQuestionOptionIsDeleted(int questionOptionId, bool isDeleted)
        {
            Questionnaire_Question_Option editQuestionOption;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editQuestionOption = (from x in dbContext.Questionnaire_Question_Options
                                          where x.Question_Option_Id.Equals(questionOptionId)
                                          select x).FirstOrDefault();

                    if (editQuestionOption == null) return null;

                    editQuestionOption.Is_Deleted = isDeleted;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editQuestionOption;
        }

        public bool DeleteQuestionOptions(int questionId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var options = (from x in dbContext.Questionnaire_Question_Options
                               where x.Questionnaire_Question_Id.Equals(questionId)
                               select x).ToList();

                dbContext.Questionnaire_Question_Options.RemoveRange(options);
                dbContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
