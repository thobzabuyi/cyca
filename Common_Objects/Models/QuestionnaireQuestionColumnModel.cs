using System;
using System.Collections.Generic;
using System.Linq;

namespace Common_Objects.Models
{
    public class QuestionnaireQuestionColumnModel
    {
        public Questionnaire_Question_Column GetSpecificQestionnaireQuestionColumn(int questionnaireQuestionColumnId)
        {
            Questionnaire_Question_Column questionnaireQuestionColumn;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var questionnaireQuestionColumnList = (from x in dbContext.Questionnaire_Question_Columns
                                                       where x.Question_Column_Id.Equals(questionnaireQuestionColumnId)
                                                       select x).ToList();

                questionnaireQuestionColumn = (from x in questionnaireQuestionColumnList
                                               select x).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return questionnaireQuestionColumn;
        }

        public List<Questionnaire_Question_Column> GetListOfQuestionnaireQuestionColumns(bool showInActive, bool showDeleted)
        {
            List<Questionnaire_Question_Column> questionnaireQuestionColumns;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var questionnaireQuestionColumnsList = (from x in dbContext.Questionnaire_Question_Columns
                                                        where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                                        where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                                        select x).ToList();

                questionnaireQuestionColumns = (from x in questionnaireQuestionColumnsList
                                                select x).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return questionnaireQuestionColumns;
        }

        public Questionnaire_Question_Column CreateQuestionnaireQuestionColumn(int questionId, string columnHeading, int questionTypeId, bool isPerOption, bool isActive, bool isDeleted, DateTime dateCreated, string createdBy)
        {
            Questionnaire_Question_Column newQuestionnaireQuestionColumn;

            var dbContext = new SDIIS_DatabaseEntities();

            var questionnaireQuestionColumn = new Questionnaire_Question_Column
            {
                Questionnaire_Question_Id = questionId,
                Column_Id = 9999, // Make sure it's placed last in the list
                Column_Heading = columnHeading,
                Questionnaire_Question_Type_Id = questionTypeId,
                Is_Per_Option = isPerOption,
                Is_Active = isActive,
                Is_Deleted = isDeleted,
                Date_Created = dateCreated,
                Created_By = createdBy
            };

            try
            {
                newQuestionnaireQuestionColumn = dbContext.Questionnaire_Question_Columns.Add(questionnaireQuestionColumn);
                dbContext.SaveChanges();

                // Reset Column Ids
                var options = (from x in dbContext.Questionnaire_Question_Columns
                               where x.Questionnaire_Question_Id.Equals(questionId)
                               select x).OrderBy(x => x.Question_Column_Id).ToList();

                var index = 1;
                foreach (var o in options)
                {
                    o.Column_Id = index;
                    if (o.Question_Column_Id.Equals(newQuestionnaireQuestionColumn.Question_Column_Id)) newQuestionnaireQuestionColumn.Column_Id = index;

                    index++;
                }

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }

            return newQuestionnaireQuestionColumn;
        }

        public Questionnaire_Question_Column EditQuestionnaireQuestionColumn(int questionColumnId, int questionId, string columnHeading, int questionTypeId, bool isPerOption, bool isActive, bool isDeleted, DateTime? dateLastModified, string modifiedBy)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editQuestionnaireQuestionColumn = (from x in dbContext.Questionnaire_Question_Columns
                                                       where x.Question_Column_Id.Equals(questionColumnId)
                                                       select x).FirstOrDefault();

                if (editQuestionnaireQuestionColumn == null) return null;

                editQuestionnaireQuestionColumn.Column_Heading = columnHeading;
                editQuestionnaireQuestionColumn.Questionnaire_Question_Type_Id = questionTypeId;
                editQuestionnaireQuestionColumn.Is_Per_Option = isPerOption;
                editQuestionnaireQuestionColumn.Is_Active = isActive;
                editQuestionnaireQuestionColumn.Is_Deleted = isDeleted;
                editQuestionnaireQuestionColumn.Date_Last_Modified = dateLastModified;
                editQuestionnaireQuestionColumn.Modified_By = modifiedBy;

                dbContext.SaveChanges();

                // Reset Column Ids
                var columns = (from x in dbContext.Questionnaire_Question_Columns
                               where x.Questionnaire_Question_Id.Equals(questionId)
                               select x).OrderBy(x => x.Question_Column_Id).ToList();

                var index = 1;
                foreach (var c in columns)
                {
                    c.Column_Id = index;
                    if (c.Question_Column_Id.Equals(editQuestionnaireQuestionColumn.Question_Column_Id)) editQuestionnaireQuestionColumn.Column_Id = index;

                    index++;
                }

                dbContext.SaveChanges();

                return editQuestionnaireQuestionColumn;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Questionnaire_Question_Column SetQuestionnaireQuestionColumnIsActive(int questionColumnId, bool isActive)
        {
            Questionnaire_Question_Column editQuestionColumn;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editQuestionColumn = (from x in dbContext.Questionnaire_Question_Columns
                                          where x.Question_Column_Id.Equals(questionColumnId)
                                          select x).FirstOrDefault();

                    if (editQuestionColumn == null) return null;

                    editQuestionColumn.Is_Active = isActive;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editQuestionColumn;
        }

        public Questionnaire_Question_Column SetQuestionnaireQuestionColumnIsDeleted(int questionColumnId, bool isDeleted)
        {
            Questionnaire_Question_Column editQuestionColumn;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editQuestionColumn = (from x in dbContext.Questionnaire_Question_Columns
                                          where x.Question_Column_Id.Equals(questionColumnId)
                                          select x).FirstOrDefault();

                    if (editQuestionColumn == null) return null;

                    editQuestionColumn.Is_Deleted = isDeleted;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editQuestionColumn;
        }

        public bool DeleteQuestionColumns(int questionId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var columns = (from x in dbContext.Questionnaire_Question_Columns
                               where x.Questionnaire_Question_Id.Equals(questionId)
                               select x).ToList();

                dbContext.Questionnaire_Question_Columns.RemoveRange(columns);
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
