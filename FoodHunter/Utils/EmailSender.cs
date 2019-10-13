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

        public void Send(String toEmail, String subject, String contents, String filename)
        {
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("noreply@localhost.com", "Food Hunter");
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents;
            var htmlContent = "<p>" + contents + "</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
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