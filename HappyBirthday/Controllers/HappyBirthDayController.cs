using HappyBirthday.Models;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HappyBirthday.Controllers
{
    public class HappyBirthDayController : Controller
    {
        public ActionResult Index()
        {
            var model = new ProfileViewModel();
            CreatModel(model);
            return View(model);
        }



        public ActionResult UploadWindow()
        {
            var logo = Session["ImageSrc"] as Logo;

            var image = new ProfileImage();
            if (logo != null)
            {
                image.ImageScr = logo.FilePath;
            }


            return PartialView("_UploadLogo", image);
        }


        [HttpPost]
        public ActionResult DeleteLogo()
        {

           
            var image = new ProfileImage();
            Session["ImageSrc"] = null;
            return PartialView("_UploadLogo", image);
        }

        public ActionResult SelectSaveLogo()
        {
            var logo = new Logo();
            return PartialView("_SelectLogo", logo);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ProfileViewModel profileViewModel)
        {
            if (!ModelState.IsValid)
            {
                CreatModel(profileViewModel);
                return RedirectToAction("Index",profileViewModel);
            }

            using (var db = new BirthDayContext())
            {
                var userProfile = new UserProfile();              
                userProfile.LastName = profileViewModel.LastName;
                userProfile.Title = profileViewModel.Title;
                userProfile.TelephoneNumber = profileViewModel.TelephoneNumber;
                userProfile.FirstName = profileViewModel.FirstName;
                userProfile.BirthDay = profileViewModel.SelectedDay.Value;
                userProfile.BirthMonth = profileViewModel.SelectedMonth.Value;
                var logo = Session["ImageSrc"] as Logo;
                if (logo != null)
                {
                    userProfile.Filename = logo.FileName;
                }
                db.UserProfiles.Add(userProfile);
                db.SaveChanges();
            }

            return RedirectToAction("About");
        }


        [HttpPost]
        public ActionResult SelectSaveLogo(HttpPostedFileBase logo)
        {

            string htmlMessage = string.Empty;
            if (logo != null)
            {
                byte[] bytes = new byte[logo.ContentLength];

                Stream stream = logo.InputStream;
                stream.Read(bytes, 0, int.Parse(logo.ContentLength.ToString()));

                var theLogo = new Logo();
                theLogo.ContentFile = new Binary(bytes);
                theLogo.ContentType = logo.ContentType;

                //request.imgHeight = 0;
                //request.imgWidth = 0;
                theLogo.ContentName = logo.FileName.Replace(" ", "_")
                    .Replace(",", "_")
                    .Replace("'", "")
                    .Replace("/", "_");

                if (IsAnImage(theLogo.ContentType))
                {
                    //System.Drawing.Image img = System.Drawing.Image.FromStream(logo.InputStream);
                    //request.imgHeight = img.Height;
                    //request.imgWidth = img.Width;
                }
                else
                {
                    htmlMessage = "<h2>File format not supported</h2>";
                    // PlayServicesHelper.FormatMessagesInHtmlList(new List<string> { "File format not supported" });
                    return Json(new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden, htmlMessage));
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetFileName(logo.FileName);
                logo.SaveAs(Path.Combine(Server.MapPath(@"~\ProfileImages"), fileName));
                var filePath = string.Format(@"\ProfileImages\{0}", fileName);
                theLogo.FilePath = filePath;
                theLogo.FileName = fileName;
                Session["ImageSrc"] = theLogo;
            }
            else
            {
                htmlMessage = "<h2>No File was uploaded</h2>";
               
                return Json(new HttpStatusCodeResult(System.Net.HttpStatusCode.NoContent, htmlMessage));
            }
            string url = Url.Action("UploadWindow", "HappyBirthDay");

            return Json(new { success = true, url = url });

        }

        public static bool IsAnImage(string contentType)
        {
            bool valid = false;
            switch (contentType)
            {
                case "image/pjpeg":
                    valid = true;
                    break;

                case "image/jpeg":
                    valid = true;
                    break;
                case "image/x-png":
                    valid = true;
                    break;
                case "image/png":
                    valid = true;
                    break;

                case "image/gif":
                    valid = true;
                    break;
            }

            return valid;
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

        private void CreatModel(ProfileViewModel model)
        {

            var months = new List<Month>();
            var days = new List<Day>();

           


            string[] invariantMonths = DateTimeFormatInfo.InvariantInfo.MonthNames;

            for (int i = 1; i <= 12; i++)
            {
                months.Add(new Month { Id = i, Name = invariantMonths[i - 1] });
            }

            model.Months = new SelectList(months, "Id", "Name");


            for (int i = 1; i <= 31; i++)
            {
                days.Add(new Day { Id = i, Name = i.ToString() });
            }

            model.Days = new SelectList(days, "Id", "Name");


        }
    }
}