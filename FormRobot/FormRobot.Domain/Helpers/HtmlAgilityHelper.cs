using EImece.Domain.Helpers;
using FormRobot.Domain.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormRobot.Domain.Helpers
{
    public class HtmlAgilityHelper
    {
        public static string GenerateFormData(string url, UserFormData userData)
        {
            var doc = new HtmlDocument();
            var htmlResult = DownloadHelper.GetStringFromUrl(url);
            doc.LoadHtml(htmlResult);
     

            var ppp = new Dictionary<String, AirdropTextbox>();

            var fieldNodes = doc.DocumentNode.ChildNodes;
            HtmlAgilityHelper.SearchInputForm(fieldNodes, userData, ppp);
            var docForm = new HtmlDocument();
            String formHtml = "";
            foreach (var key in ppp.Keys)
            {
                if (key.StartsWith("input"))
                {
                    // Console.WriteLine(ppp[key]);
                }
                else
                {

                    docForm.LoadHtml(ppp[key].AirdropTextboxHtml);
                    HtmlNode node = docForm.DocumentNode.ChildNodes[0];
                    //   Console.WriteLine(node.OuterHtml.Replace(node.InnerHtml,""));
                    formHtml = node.OuterHtml.Replace(node.InnerHtml, "");
                }

            }
            formHtml = formHtml.Replace("</form>", String.Join("<br>", ppp.Where(t => t.Key.StartsWith("input")).Select(x => x.Value.AirdropTextboxHtml).ToArray()));
            formHtml = formHtml + "<input type='submit' value='Send Request'></form>";
            return formHtml;
        }

        public static void SearchInputForm(
            HtmlNodeCollection fieldNodes,
            UserFormData myFormData, 
            Dictionary<String, AirdropTextbox> myFormHtml)
        {
            foreach (var currentNode in fieldNodes)
            {
                SearchInputForm(currentNode.ChildNodes, myFormData, myFormHtml);
                if (currentNode.Name == "input" || currentNode.Name == "textarea")
                {
                    if (currentNode.Attributes.Any(t => t.Name.Equals("aria-label")))
                    {
                        //, UserFormData userData
                        string label = currentNode.Attributes["aria-label"].Value.ToStr();
                        if (label.ToLower().Contains("ERC-20 wallet address".ToLower()))
                        {
                            setValue(myFormData.EthWalletAddress, currentNode);
                        }
                        else if (label.ToLower().Contains("Telegram Username".ToLower()))
                        {
                            setValue(myFormData.TelegramUsername, currentNode);
                           // myFormData["Telegram_Username"].ToStr();
                        }
                        else if (label.ToLower().Contains("Bitcointalk profile URL".ToLower()))
                        {
                            setValue(myFormData.BitcointalkProfileURL, currentNode);
                        }
                        else if (label.ToLower().Contains("Bitcointalk username".ToLower()))
                        {
                            setValue(myFormData.BitcointalkUsername, currentNode);
                        }
                        var t = new AirdropTextbox() { AirdropTextboxHtml = "<b>" + label + "</b>" + currentNode.OuterHtml };
                        myFormHtml["input_textbox_" + currentNode.Attributes["name"].Value]                            =t;

                    }
                    else
                    {
                        var t = new AirdropTextbox() { AirdropTextboxHtml = currentNode.OuterHtml };
                        myFormHtml["input_others_" + currentNode.Attributes["name"].Value] =   t ;

                    }

                }
                else if (currentNode.Name == "form")
                {
                    var t = new AirdropTextbox() { AirdropTextboxHtml = currentNode.OuterHtml };
                    myFormHtml["form_" + currentNode.Attributes["action"].Value] = t;
                }

            }
        }

        private static void setValue(String myFormData, HtmlNode currentNode)
        {
            if (currentNode.Name == "input")
            {
                currentNode.Attributes["value"].Value = myFormData;
            }
            else if ( currentNode.Name == "textarea")
            {
                currentNode.InnerHtml = myFormData;
            }
        }

        public static IEnumerable<Tuple<string, HtmlNode>> GetInputNodes(string url, params string[] fields)
        {
            var doc = new HtmlDocument();
            var htmlResult = DownloadHelper.GetStringFromUrl(url);
            doc.LoadHtml(htmlResult);


            var form = doc.DocumentNode.SelectSingleNode("//form");
            foreach (var field in fields)
            {
                var fieldNodes = form.ChildNodes
                    .OfType<HtmlTextNode>();
                var fieldNode = fieldNodes
                    .Where(node => node.Text.Trim().StartsWith(field, StringComparison.OrdinalIgnoreCase))
                    .SingleOrDefault();
                if (fieldNode == null)
                    continue;

                var input = FindCorrespondingInputNode(fieldNode);
                if (input != null)
                    yield return Tuple.Create(field, input);
            }
        }

        public static HtmlNode FindCorrespondingInputNode(HtmlTextNode fieldNode)
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
