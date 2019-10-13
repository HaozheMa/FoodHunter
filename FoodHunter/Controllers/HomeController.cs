using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoodHunter.Utils;
using FoodHunter.Models;
using System.IO;

namespace FoodHunter.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            EmailSender es = new EmailSender();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Send_Email()
        {
            return View(new SendEmailViewModel());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Send_Email(SendEmailViewModel model, HttpPostedFileBase postedFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    String toEmail = model.ToEmail;
                    String subject = model.Subject;
                    String contents = model.Contents;

                    string imagePath = "";
                    if (postedFile != null)
                    {
                        var myUniqueFileName = string.Format(@"{0}", Guid.NewGuid());
                        imagePath = myUniqueFileName;
                        //TryValidateModel(image);

                        string serverPath = Server.MapPath("~/Uploads/");
                        string fileExtension = Path.GetExtension(postedFile.FileName);
                        string filePath = imagePath + fileExtension;
                        imagePath = filePath;
                        try
                        {
                            postedFile.SaveAs(serverPath + imagePath);
                        }
                        catch
                        {
                            ViewBag.Result = "file save failed.";
                            return View();
                        }
                    }
                    
                        EmailSender es = new EmailSender();
                        es.Send(toEmail, subject, contents, imagePath);

                        ViewBag.Result = "Email has been send.";

                        ModelState.Clear();

                        return View(new SendEmailViewModel());
                   
                }
                catch
                {
                    ViewBag.Result = "email not been sended";
                    return View();
                }
               
            }
            else
            {
                ViewBag.Result = "Email model has error.";
                return View();
            }

            return View();
        }



    }

}