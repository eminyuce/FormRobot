using FormRobot.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FormRobot.Domain.Entities;
using EImece.Domain.Helpers;

namespace FormRobot.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Arrange
            //Airdrop table storing URLs
            // User  
           
            return View();
        }

        public ActionResult GetAirDropHtmlPage(string airDropLink)
        {
            var newAirDropHtml = new AirDropForm();
            newAirDropHtml.AirDropUrl = airDropLink;
            var myFormData = new UserFormData();
            myFormData.EthWalletAddress = "";
            myFormData.BitcointalkProfileURL = "";
            myFormData.TelegramUsername = "";
            myFormData.PersonalEmailAddress = "";
            myFormData.BitcointalkUsername = "";
            newAirDropHtml.myFormData = myFormData;

            var tempData = new TempDataDictionary();
            var html = this.RenderPartialToString(
                        @"~/Views/Shared/pAirDropForm.cshtml",
                        new ViewDataDictionary(newAirDropHtml), tempData);
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}