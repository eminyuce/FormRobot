using EImece.Domain.Helpers;
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
        public static void SearchInputForm(
            HtmlNodeCollection fieldNodes,
            Dictionary<String, String> myFormData, Dictionary<String, String> myFormHtml)
        {
            foreach (var currentNode in fieldNodes)
            {
                SearchInputForm(currentNode.ChildNodes, myFormData, myFormHtml);
                if (currentNode.Name == "input")
                {
                    if (currentNode.Attributes.Any(t => t.Name.Equals("aria-label")))
                    {
                        string label = currentNode.Attributes["aria-label"].Value.ToStr();
                        if (label.ToLower().Contains("ERC-20 wallet address".ToLower()))
                        {
                            currentNode.Attributes["value"].Value = myFormData["Eth_Wallet_Address"].ToStr();
                        }
                        else if (label.ToLower().Contains("Telegram Username".ToLower()))
                        {
                            currentNode.Attributes["value"].Value = myFormData["Telegram_Username"].ToStr();
                        }
                        else if (label.ToLower().Contains("Bitcointalk profile URL".ToLower()))
                        {
                            currentNode.Attributes["value"].Value = myFormData["Telegram_Username"].ToStr();
                        }
                        else if (label.ToLower().Contains("Bitcointalk username".ToLower()))
                        {
                            currentNode.Attributes["value"].Value = myFormData["Bitcointalk_username"].ToStr();
                        }
                        myFormHtml["input_" + currentNode.Attributes["name"].Value] = currentNode.OuterHtml;

                    }

                }
                else if (currentNode.Name == "form")
                {
                    myFormHtml["form_" + currentNode.Attributes["action"].Value] = currentNode.OuterHtml;
                }

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
