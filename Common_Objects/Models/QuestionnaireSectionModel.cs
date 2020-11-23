using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class QuestionnaireSectionModel
    {
        public Questionnaire_Section GetSpecificQestionnaireSection(int questionnaireSectionId)
        {
            Questionnaire_Section questionnaireSection;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var questionnaireSectionList = (from x in dbContext.Questionnaire_Sections
                                                where x.Questionnaire_Section_Id.Equals(questionnaireSectionId)
                                                select x).ToList();

                questionnaireSection = (from x in questionnaireSectionList
                                        select x).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return questionnaireSection;
        }

        public List<Questionnaire_Section> GetListOfQuestionnaireSections(bool showInActive, bool showDeleted)
        {
            List<Questionnaire_Section> questionnaireSections;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var questionnaireSectionList = (from x in dbContext.Questionnaire_Sections
                                                    where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                                    where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                                    select x).ToList();

                    questionnaireSections = (from x in questionnaireSectionList
                                             select x).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return questionnaireSections;
        }

        public List<Questionnaire_Section> GetListOfQuestionnaireSections(bool showInActive, bool showDeleted, int questionnaireId)
        {
            List<Questionnaire_Section> questionnaireSections;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var questionnaireSectionList = (from x in dbContext.Questionnaire_Sections
                                                    where x.Is_Active.Equals(true) || x.Is_Active.Equals(!showInActive)
                                                    where x.Is_Deleted.Equals(false) || x.Is_Deleted.Equals(showDeleted)
                                                    where x.Questionnaire_Id.Equals(questionnaireId)
                                                    select x).ToList();

                    questionnaireSections = (from x in questionnaireSectionList
                                             select x).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return questionnaireSections;
        }

        public Questionnaire_Section CreateQuestionnaireSection(int questionnaireId, string sectionName, string sectionHeading, bool isPerPerson, bool isActive, bool isDeleted, DateTime dateCreated, string createdBy)
        {
            Questionnaire_Section newQuestionnaireSection;

            var dbContext = new SDIIS_DatabaseEntities();

            var questionnaireSection = new Questionnaire_Section()
            {
                Questionnaire_Id = questionnaireId,
                Section_Name = sectionName,
                Section_Header = sectionHeading,
                Is_Per_Person = isPerPerson,
                Sort_Order = 9999,
                Is_Active = isActive,
                Is_Deleted = isDeleted,
                Date_Created = dateCreated,
                Created_By = createdBy
            };

            try
            {
                newQuestionnaireSection = dbContext.Questionnaire_Sections.Add(questionnaireSection);
                dbContext.SaveChanges();

                var sortSections = dbContext.Questionnaire_Sections.Where(w => w.Questionnaire_Id.Equals(questionnaireId)).OrderBy(o => o.Sort_Order).ToList();

                var sortOrder = 1;
                foreach (var s in sortSections)
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

            return newQuestionnaireSection;
        }

        public Questionnaire_Section EditQuestionnaireSection(int questionnaireSectionId, int questionnaireId, string sectionName, string sectionHeading, bool isPerPerson, bool isActive, bool isDeleted, DateTime? dateLastModified, string modifiedBy)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editQuestionnaireSection = (from x in dbContext.Questionnaire_Sections
                                                where x.Questionnaire_Section_Id.Equals(questionnaireSectionId)
                                                select x).FirstOrDefault();

                if (editQuestionnaireSection == null) return null;

                editQuestionnaireSection.Questionnaire_Id = questionnaireId;
                editQuestionnaireSection.Section_Name = sectionName;
                editQuestionnaireSection.Section_Header = sectionHeading;
                editQuestionnaireSection.Is_Per_Person = isPerPerson;
                editQuestionnaireSection.Is_Active = isActive;
                editQuestionnaireSection.Is_Deleted = isDeleted;
                editQuestionnaireSection.Date_Last_Modified = dateLastModified;
                editQuestionnaireSection.Modified_By = modifiedBy;

                dbContext.SaveChanges();

                var sortSections = dbContext.Questionnaire_Sections.Where(w => w.Questionnaire_Id.Equals(questionnaireId)).OrderBy(o => o.Sort_Order).ToList();

                var sortOrder = 1;
                foreach (var s in sortSections)
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

                return editQuestionnaireSection;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Questionnaire_Section SetQuestionnaireSectionIsActive(int sectionId, bool isActive)
        {
            Questionnaire_Section editSection;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editSection = (from x in dbContext.Questionnaire_Sections
                                   where x.Questionnaire_Section_Id.Equals(sectionId)
                                   select x).FirstOrDefault();

                    if (editSection == null) return null;

                    editSection.Is_Active = isActive;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editSection;
        }

        public Questionnaire_Section SetQuestionnaireSectionIsDeleted(int sectionId, bool isDeleted)
        {
            Questionnaire_Section editSection;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editSection = (from x in dbContext.Questionnaire_Sections
                                   where x.Questionnaire_Section_Id.Equals(sectionId)
                                   select x).FirstOrDefault();

                    if (editSection == null) return null;

                    editSection.Is_Deleted = isDeleted;

                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return editSection;
        }

        public Questionnaire_Section MoveQuestionnaireSectionUp(int sectionId)
        {
            Questionnaire_Section editSection;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editSection = (from x in dbContext.Questionnaire_Sections
                                   where x.Questionnaire_Section_Id.Equals(sectionId)
                                   select x).FirstOrDefault();

                    if (editSection == null) return null;

                    editSection.Sort_Order = editSection.Sort_Order - 3;

                    var questionnaire = editSection.Questionnaire;

                    var sortOrder = 1;
                    foreach (var s in questionnaire.Questionnaire_Sections.OrderBy(x => x.Sort_Order))
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

            return editSection;
        }

        public Questionnaire_Section MoveQuestionnaireSectionDown(int sectionId)
        {
            Questionnaire_Section editSection;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    editSection = (from x in dbContext.Questionnaire_Sections
                                   where x.Questionnaire_Section_Id.Equals(sectionId)
                                   select x).FirstOrDefault();

                    if (editSection == null) return null;

                    editSection.Sort_Order = editSection.Sort_Order + 3;

                    var questionnaire = editSection.Questionnaire;

                    var sortOrder = 1;
                    foreach (var s in questionnaire.Questionnaire_Sections.OrderBy(x => x.Sort_Order))
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

            return editSection;
        }
    }
}
