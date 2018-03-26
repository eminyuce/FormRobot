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
using System.Threading.Tasks;

namespace FormRobot.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index(int id = 0)
        {
            //Arrange
            //Airdrop table storing URLs
            // User  

            var item = AirDropLinkRepository.GetAirDropLink(id);
            ViewBag.AirDropUrl = item.AirDropLinkUrl.ToStr().Trim();
            return View();
        }
        public ActionResult DeleteAirDropLink(int id = 0)
        {
            var item = AirDropLinkRepository.GetAirDropLink(id);
            item.IsDeleted = true;
            AirDropLinkRepository.SaveOrUpdateAirDropLink(item);
            return RedirectToAction("AirDropLinkPage");
        }
        public ActionResult AirDropLinkPage(bool isAll = false)
        {
            var items = AirDropLinkRepository.GetAirDropLinksFromCache();
            var goodOnes = items.Where(t => !t.IsDeleted).ToList();
            if (isAll)
            {
                goodOnes = items;
            }
            return View(goodOnes);
        }
        public ActionResult FormMatchs(string search = "")
        {
            var formMatches = FormMatchRepository.GetFormMatchsFromCache();
            if (!String.IsNullOrEmpty(search))
            {
                formMatches = formMatches.Where(t => t.FormItemText.ToLower().Contains(search.ToLower())).ToList();
            }
            return View(formMatches);
        }
        public ActionResult FormMatchItem(int id = 0)
        {
            var pe = new FormMatch();


            if (id == 0)
            {

            }
            else
            {
                pe = FormMatchRepository.GetFormMatch(id);
            }

            // Get one instance and then iterate all the properties
            List<SelectListItem> selectListItems = FormMatchRepository.GenerateUserDataDropDownItems();

            ViewBag.SearchFields = selectListItems;

            return View(pe);

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
            var emptyObjXml = XmlParserHelper.ToXml<UserFormData>(emptyObj);
            using (var context = new UsersContext())
            {
                var user = context.UserProfiles.FirstOrDefault(t => t.UserName.Equals(User.Identity.Name));
                user.UserData = String.IsNullOrEmpty(user.UserData.ToStr()) ? emptyObjXml : user.UserData;
                var result = XmlParserHelper.ToObject<UserFormData>(user.UserData);
                result.UserId = user.UserId;
                return View(result);
            }

        }
        [HttpPost]
        public ActionResult UserData(UserFormData profile, int id = 0)
        {
            if (String.IsNullOrEmpty(profile.YourFullName.ToStr().Trim()))
            {
                ModelState.AddModelError("YourFullName", "FullName is required.");
                return View(profile);
            }
            if (String.IsNullOrEmpty(profile.EthWalletAddress.ToStr().Trim()))
            {
                ModelState.AddModelError("EthWalletAddress", "Eth Wallet Address is required.");
                return View(profile);
            }
            using (var context = new UsersContext())
            {
                var user = context.UserProfiles.FirstOrDefault(t => t.UserId == profile.UserId);
                user.UserData = XmlParserHelper.ToXml<UserFormData>(profile);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

        }
        public ActionResult GetGoogleSearch()
        {
            Task.Factory.StartNew(() =>
            {

                var searchResult = HtmlAgilityHelper.GetGoogleDriveLinks("site:https://docs.google.com/forms/d/ airdrop");
                foreach (var s in searchResult)
                {
                    Console.WriteLine(s);
                    AirDropLinkRepository.SaveOrUpdateAirDropLink
                        (new AirDropLink()
                        {
                            AirDropLinkUrl = s.ToStr().Trim()
                        });

                }

            });


            return RedirectToAction("Index");
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
                if (!String.IsNullOrEmpty(myFormData.EthWalletAddress))
                {
                    myFormData = XmlParserHelper.ToObject<UserFormData>(user.UserData);
                }
            }

            int airdropId = AirDropLinkRepository.SaveOrUpdateAirDropLink(new AirDropLink() { AirDropLinkUrl = airDropLink.ToStr().Trim(), IsDeleted=false });
            newAirDropHtml.myFormData = myFormData;
            newAirDropHtml.AirDropId = airdropId;
            var tempData = new TempDataDictionary();
            tempData["airdropId"] = airdropId;
            var html = this.RenderPartialToString(
                        @"~/Views/Shared/pAirDropForm.cshtml",
                        new ViewDataDictionary(newAirDropHtml), tempData);
            return Json(html, JsonRequestBehavior.AllowGet);
        }


    }
}