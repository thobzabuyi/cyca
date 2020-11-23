using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common_Objects
{
    public class Mailer
    {
        public static bool SendMail(string toName, string toEmail, string subject, string body)
        {
            var fromAddress = new MailAddress("no-reply@dsd.gov.za", "DSD Mailer (Test)");
            var toAddress = new MailAddress(toEmail, toName);

            var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body, IsBodyHtml = true };

            //if (!string.IsNullOrEmpty(attachmentFile))
            //{
            //    var attachment = new System.Net.Mail.Attachment(attachmentFile, MediaTypeNames.Application.Octet);

            //    ContentDisposition disposition = attachment.ContentDisposition;
            //    disposition.CreationDate = File.GetCreationTime(attachmentFile);
            //    disposition.ModificationDate = File.GetLastWriteTime(attachmentFile);
            //    disposition.ReadDate = File.GetLastAccessTime(attachmentFile);
            //    message.Attachments.Add(attachment);
            //}

            var smtpClient = new SmtpClient();

            try
            {
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                var Test = ex.Message;
                return false;
            }
        }
    }
}
