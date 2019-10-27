using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace FoodHunter.Utils
{
    public class EmailSender
    {
        // Please use your API KEY here.
        private const String API_KEY = "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";

        public void Send(string toEmail, String subject, String contents, String filename)
        {
            var to_addr = new List<EmailAddress>();
           if (toEmail.Contains(";"))
            {
               var addressList = toEmail.Split(';');
                 foreach (String s in addressList)
                {
                    to_addr.Add(new EmailAddress(s, ""));
                }
            }

            
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("noreply@localhost.com", "Food Hunter");
            //var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, to_addr, subject, plainTextContent, htmlContent);
            //string serverPath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/");
            // Attachment attachment = new Attachment();
            // attachment.Filename = serverPath;
            //msg.Attachments.Add(attachment);
            if (!filename.Equals(""))
            {
                byte[] bytes = File.ReadAllBytes(System.Web.HttpContext.Current.Server.MapPath(@"~/Uploads/" + filename));
                string fileContentsAsBase64 = Convert.ToBase64String(bytes);

                var attachment = new Attachment
                {
                    Filename = filename,
                    Type = "application/jpeg",
                    Content = fileContentsAsBase64
                };
                msg.AddAttachment(attachment);
            }
           // msg.Attachments.Add(attachment);
            var response = client.SendEmailAsync(msg);
        }

    }
}