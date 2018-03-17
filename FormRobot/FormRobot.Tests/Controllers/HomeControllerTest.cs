using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FormRobot;
using FormRobot.Controllers;
using HtmlAgilityPack;
using FormRobot.Domain.Helpers;
using EImece.Domain.Helpers;

namespace FormRobot.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            string url = "https://docs.google.com/forms/d/e/1FAIpQLSfaTavUqVi5LG7jsxaYwi-AGv0lY4ZRcVB6Vs1A1q0U6ykpGw/viewform";

            HtmlAgilityHelper.GenerateFormData(url);
            //


        }

        
    }
}
