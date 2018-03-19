using FormRobot.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FormRobot.Domain.Entities;
using EImece.Domain.Helpers;
using FormRobot.Models;
using HelpersProject;
using FormRobot.Domain.DB;

namespace FormRobot.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index(int id=0)
        {
            //Arrange
            //Airdrop table storing URLs
            // User  

            var item = AirDropLinkRepository.GetAirDropLink(id);
            ViewBag.AirDropUrl = item.AirDropLinkUrl.ToStr().Trim();
            return View();
        }
        public ActionResult AirDropLinkPage()
        {
            var items = AirDropLinkRepository.GetAirDropLinks();
            return View(items);
        }
        public ActionResult FormMatchs(string search="")
        {
            var formMatches = FormMatchRepository.GetFormMatchs();
            if (!String.IsNullOrEmpty(search))
            {
                formMatches = formMatches.Where(t => t.FormItemText.ToLower().Contains(search.ToLower())).ToList();
            }
            return View(formMatches);
        }
        public ActionResult FormMatchItem(int id = 0)
        {
            var pe = new FormMatch();

            {
                if (id == 0)
                {

                }
                else
                {
                    pe = FormMatchRepository.GetFormMatch(id);
                }
                return View(pe);
            }
        }
        [HttpPost]
        public ActionResult FormMatchItem(FormMatch model)
        {
            FormMatchRepository.SaveOrUpdateFormMatch(model);
            return RedirectToAction("FormMatchs");
        }
        public ActionResult UserData()
        {
            var emptyObj = new UserFormData();
            emptyObj.EthWalletAddress = "EthWalletAddress";
            emptyObj.BitcointalkProfileURL = "BitcointalkProfileURL";
            emptyObj.TelegramUsername = "TelegramUsername";
            emptyObj.BitcointalkUsername = "BitcointalkUsername";
            emptyObj.PersonalEmailAddress = "PersonalEmailAddress";
            emptyObj.FacebookName = "FacebookName";
            emptyObj.MediumProfileURL = "MediumProfileURL";
            emptyObj.FacebookProfileURL = "FacebookProfileURL";
            emptyObj.TwitterUsername = "TwitterUsername";
            emptyObj.TwitterProfileURL = "TwitterProfileURL";
            emptyObj.YourSkills = "YourSkills";
            emptyObj.YourLanguage = "YourLanguage";
            emptyObj.YourHelps = "YourHelps";


            var emptyObjXml = XmlParserHelper.ToXml<UserFormData>(emptyObj);
            using (var context = new UsersContext())
            {
                var user = context.UserProfiles.FirstOrDefault(t => t.UserName.Equals(User.Identity.Name));
                user.UserData = String.IsNullOrEmpty(user.UserData.ToStr()) ? emptyObjXml : user.UserData;
                return View(user);
            }

        }
        [HttpPost]
        public ActionResult UserData(UserProfile profile)
        {
            using (var context = new UsersContext())
            {
                var user = context.UserProfiles.FirstOrDefault(t => t.UserName.Equals(User.Identity.Name));
                user.UserData = profile.UserData;
                context.SaveChanges();
                return RedirectToAction("Index");
            }

        }
        public ActionResult GetAirDropHtmlPage(string airDropLink)
        {
            if (String.IsNullOrEmpty(airDropLink))
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            var newAirDropHtml = new AirDropForm();
            newAirDropHtml.AirDropUrl = airDropLink;
            var myFormData = new UserFormData();
            using (var context = new UsersContext())
            {
                var user = context.UserProfiles.FirstOrDefault(t => t.UserName.Equals(User.Identity.Name));
                myFormData = XmlParserHelper.ToObject<UserFormData>(user.UserData);
            }

            AirDropLinkRepository.SaveOrUpdateAirDropLink(new AirDropLink() { AirDropLinkUrl = airDropLink.ToStr().Trim() });
            newAirDropHtml.myFormData = myFormData;

            var tempData = new TempDataDictionary();
            var html = this.RenderPartialToString(
                        @"~/Views/Shared/pAirDropForm.cshtml",
                        new ViewDataDictionary(newAirDropHtml), tempData);
            return Json(html, JsonRequestBehavior.AllowGet);
        }


    }
}