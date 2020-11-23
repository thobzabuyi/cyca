using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class QuestionnaireQuestionTypeModel
    {
        public Questionnaire_Question_Type GetSpecificQestionnaireQuestionType(int questionnaireQuestionTypeId)
        {
            Questionnaire_Question_Type questionnaireQuestionType;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var questionnaireQuestionTypeList = (from x in dbContext.Questionnaire_Question_Types
                                                     where x.Questionnaire_Question_Type_Id.Equals(questionnaireQuestionTypeId)
                                                     select x).ToList();

                questionnaireQuestionType = (from x in questionnaireQuestionTypeList
                                             select x).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return questionnaireQuestionType;
        }

        public List<Questionnaire_Question_Type> GetListOfQuestionnaireQuestionTypes()
        {
            List<Questionnaire_Question_Type> questionnaireQuestionTypes;

            using (var dbContext = new SDIIS_DatabaseEntities())
            {
                try
                {
                    var questionnaireQuestionTypeList = (from x in dbContext.Questionnaire_Question_Types
                                                         select x).ToList();

                    questionnaireQuestionTypes = (from x in questionnaireQuestionTypeList
                                                  select x).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return questionnaireQuestionTypes;
        }
    }
}
