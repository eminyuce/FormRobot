using FormRobot.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FormRobot.Domain.Entities;

namespace FormRobot.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Arrange
            string url = "https://docs.google.com/forms/d/e/1FAIpQLSfaTavUqVi5LG7jsxaYwi-AGv0lY4ZRcVB6Vs1A1q0U6ykpGw/viewform";
            var newAirDropHtml = new AirDropForm();
            newAirDropHtml.AirDropUrl = url;
            return View(newAirDropHtml);
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
    }
}