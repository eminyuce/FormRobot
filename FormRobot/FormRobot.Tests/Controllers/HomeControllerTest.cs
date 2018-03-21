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
using FormRobot.Domain.DB;

namespace FormRobot.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            var searchResult = HtmlAgilityHelper.GetGoogleSearch();
            foreach(var s in searchResult)
            {
                Console.WriteLine(s);
                AirDropLinkRepository.SaveOrUpdateAirDropLink
                    (new AirDropLink() {
                        AirDropLinkUrl = s.ToStr().Trim() });

            }
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
