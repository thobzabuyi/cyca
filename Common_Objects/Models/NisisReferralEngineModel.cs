using System;
using System.Linq;

namespace Common_Objects.Models
{
    public class NisisReferralEngineModel
    {
        public NISIS_Referral_Engine GetSpecificReferralEngineItem(int referralEngineId)
        {
            NISIS_Referral_Engine referralEngineItem;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var referralEngineItems = (from x in dbContext.NISIS_Referral_Engine_Items
                                           where x.NISIS_Referral_Engine_Id.Equals(referralEngineId)
                                           select x).ToList();

                referralEngineItem = (from x in referralEngineItems
                                      select x).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return referralEngineItem;
        }

        public NISIS_Referral_Engine GetSpecificReferralEngineItemByQuestion(int questionId)
        {
            NISIS_Referral_Engine referralEngineItem;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var referralEngineItems = (from x in dbContext.NISIS_Referral_Engine_Items
                                           where x.NISIS_Questionnaire_Question_Id.Equals(questionId)
                                           select x).ToList();

                referralEngineItem = (from x in referralEngineItems
                                      select x).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return referralEngineItem;
        }

        public NISIS_Referral_Engine GetSpecificReferralEngineItemByQuestion(int questionId, int optionId)
        {
            NISIS_Referral_Engine referralEngineItem;

            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var referralEngineItems = (from x in dbContext.NISIS_Referral_Engine_Items
                                           where x.NISIS_Questionnaire_Question_Id.Equals(questionId)
                                           where x.NISIS_Questionnaire_Answer_Value.Equals(optionId.ToString())
                                           select x).ToList();

                referralEngineItem = (from x in referralEngineItems
                                      select x).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }

            return referralEngineItem;
        }

        public NISIS_Referral_Engine CreateReferralEngineItem(int questionId, string questionAnswerValue, int serviceId)
        {
            NISIS_Referral_Engine newReferralEngineItem;

            var dbContext = new SDIIS_DatabaseEntities();

            var referralEngineItem = new NISIS_Referral_Engine
            {
                NISIS_Questionnaire_Question_Id = questionId,
                NISIS_Questionnaire_Answer_Value = questionAnswerValue,
                NISIS_Service_Id = serviceId,
            };

            try
            {
                newReferralEngineItem = dbContext.NISIS_Referral_Engine_Items.Add(referralEngineItem);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return null;
            }

            return newReferralEngineItem;
        }

        public NISIS_Referral_Engine EditReferralEngineItem(int referralEngineItemId, int questionId, string questionAnswerValue, int serviceId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var editReferralEngineItem = (from x in dbContext.NISIS_Referral_Engine_Items
                                              where x.NISIS_Referral_Engine_Id.Equals(referralEngineItemId)
                                              select x).FirstOrDefault();

                if (editReferralEngineItem == null) return null;

                editReferralEngineItem.NISIS_Questionnaire_Question_Id = questionId;
                editReferralEngineItem.NISIS_Questionnaire_Answer_Value = questionAnswerValue;
                editReferralEngineItem.NISIS_Service_Id = serviceId;

                dbContext.SaveChanges();

                return editReferralEngineItem;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool DeleteReferralEngineItem(int questionId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var referralItem = (from x in dbContext.NISIS_Referral_Engine_Items
                                    where x.NISIS_Questionnaire_Question_Id.Equals(questionId)
                                    select x).ToList();

                dbContext.NISIS_Referral_Engine_Items.RemoveRange(referralItem);
                dbContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteReferralEngineItemForOption(int questionId, int optionId)
        {
            var dbContext = new SDIIS_DatabaseEntities();

            try
            {
                var referralItem = (from x in dbContext.NISIS_Referral_Engine_Items
                                    where x.NISIS_Questionnaire_Question_Id.Equals(questionId)
                                    where x.NISIS_Questionnaire_Answer_Value.Equals(optionId.ToString())
                                    select x).ToList();

                dbContext.NISIS_Referral_Engine_Items.RemoveRange(referralItem);
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