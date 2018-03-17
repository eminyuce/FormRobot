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
            string[] fields = new[] { "username" };
            var doc = new HtmlDocument();
            var htmlResult = DownloadHelper.GetStringFromUrl(url);
            doc.LoadHtml(htmlResult);
            Dictionary<String, String> myFormData = new Dictionary<string, string>();
            myFormData["Eth_Wallet_Address"] = "0x75E352B05d54313358204877496F39b00016c62e";
            myFormData["Bitcointalk_profile_URL"] = "guvenulu";
            myFormData["Telegram_Username"] = "guvenulu";
            myFormData["Personal_Email_Address"] = "prisoner.ever@gmail.com";
            myFormData["Bitcointalk_username"] = "guvenulu";
   
            var ppp = new Dictionary<String, String>();

           var fieldNodes = doc.DocumentNode.ChildNodes;
            foreach (var field in fields)
            {
                HtmlAgilityHelper.SearchInputForm(fieldNodes, myFormData, ppp);
            }
            foreach (var key in ppp.Keys)
            {
                if (key.StartsWith("input"))
                {
                    Console.WriteLine(ppp[key]);
                }else
                {
                    Console.WriteLine(ppp[key]);
                }
              
            }
            //


        }

        

        public  HtmlNode FindCorrespondingInputNode(HtmlTextNode fieldNode)
        {
            for (var currentNode = fieldNode.NextSibling;
                 currentNode != null && currentNode.NodeType != HtmlNodeType.Text;
                 currentNode = currentNode.NextSibling)
            {
                if (currentNode.Name == "input"
                 && !currentNode.Attributes["type"].Value.Contains("hidden"))
                {
                    return currentNode;
                }
            }
            return null;
        }


    }
}
