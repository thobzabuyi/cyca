using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects.Models
{
    public class IntakeSendUserEmail
    {
        private SDIIS_DatabaseEntities _db = new SDIIS_DatabaseEntities();

        private User GetUserDetails(int Id)
        {
            var User = _db.Users.Find(Id);         
            return User;
        }

        public void SendEmailToUser(int User_Id)
        {
            #region send Email

            #region SendMessage
            SmtpClient mailUser = new SmtpClient();
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();

            var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            var message = new System.Net.Mail.MailMessage();
            string RecipientAddress = _db.Users.Find(User_Id).Email_Address;
            var dateCreated = DateTime.Now;
            message.To.Add(new MailAddress(Convert.ToString(RecipientAddress)));  // replace with valid value 

            var MessageSubject = "Captured Employee on SDICMS".ToUpper();
            message.Subject = Convert.ToString(MessageSubject);

            var MessageContent = "You are herewith informed that your user details was captured onto the Social Development Integrated"
                + "<br/> Case Management System (SDICMS): "
                + "<br/>"
                + "<br/> User Name: " + GetUserDetails(User_Id).User_Name
                + "<br/> Password: " + GetUserDetails(User_Id).Password
                + "<br/> First Name: " + GetUserDetails(User_Id).First_Name
                + "<br/> Last Name: " + GetUserDetails(User_Id).Last_Name
                + "<br/>"
                + "<br/> On: " + dateCreated.ToLongDateString()
                + "<br/>"
                + "<br/>"
                + "<br/> Message submitted via the SDICMS.";

            var SendFromWhom = "Department of Social Development: SDICMS";
            message.Body = string.Format(body, Convert.ToString(SendFromWhom), Convert.ToString("dsd.gov.za"), Convert.ToString(MessageContent));
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                try
                {
                    smtp.SendMailAsync(message);
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {

                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message1 = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message1, raise);
                        }
                    }
                    throw raise;
                }

            }

            #endregion

            #endregion

        }
    }
}
