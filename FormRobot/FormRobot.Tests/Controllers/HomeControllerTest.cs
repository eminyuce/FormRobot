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
using FormRobot.Domain.Entities;

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
        public void Contact()
        {
            // Arrange
            string url = "https://docs.google.com/forms/d/e/1FAIpQLSf3LM5k6PI98HZgzjhvYadF_fbxQRdM6cZzATVQWaLtiuoXnw/viewform";
            var myFormData = new UserFormData();
            myFormData.EthWalletAddress = "0x75E352B05d54313358204877496F39b00016c62e";
            myFormData.BitcointalkProfileURL = "guvenulu";
            myFormData.TelegramUsername= "guvenulu";
            myFormData.PersonalEmailAddress = "prisoner.ever@gmail.com";
            myFormData.BitcointalkUsername = "guvenulu";
            var htmlOutput = HtmlAgilityHelper.GenerateFormData(url, myFormData);

            Console.WriteLine(htmlOutput);
            //


        }

        
    }
}
