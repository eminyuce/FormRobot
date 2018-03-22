using EImece.Domain.Helpers;
using FormRobot.Domain.DB;
using FormRobot.Domain.Entities;
using FormRobot.Domain.Entities.AirDropType;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormRobot.Domain.Helpers
{


    public class HtmlAgilityHelper
    {
        public static List<String> GetGoogleSearch()
        {
            String SearchResults = "https://www.google.com/search?q=site:https://docs.google.com/forms/d/+airdrop&source=lnt&tbs=qdr:w&sa=X&ved=0ahUKEwji4YjAt_7ZAhWSxFkKHfuDDecQpwUIIA&biw=1280&bih=669";
            var resultList = new List<String>();
            resultList.AddRange(GetGoogleDriveLinks(SearchResults));
            SearchResults = "https://www.google.com/search?q=site:https://docs.google.com/forms/d/+bounty&source=lnt&tbs=qdr:m&sa=X&ved=0ahUKEwiPoaeLvf7ZAhWPm1kKHSOrAuYQpwUIIA&biw=1280&bih=669";
            resultList.AddRange(GetGoogleDriveLinks(SearchResults));
            return resultList;
        }
        public static List<String> GetGoogleDriveLinks(string SearchResults)
        {
            var resultList = new List<String>();
            try
            {


                StringBuilder sb = new StringBuilder();
                byte[] ResultsBuffer = new byte[8192];
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SearchResults);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream resStream = response.GetResponseStream();
                string tempString = null;
                int count = 0;
                do
                {
                    count = resStream.Read(ResultsBuffer, 0, ResultsBuffer.Length);
                    if (count != 0)
                    {
                        tempString = Encoding.ASCII.GetString(ResultsBuffer, 0, count);
                        sb.Append(tempString);
                    }
                }

                while (count > 0);
                string sbb = sb.ToString();


                var inputString = sbb;
                var regex = new Regex("<a [^>]*href=(?:'(?<href>.*?)')|(?:\"(?<href>.*?)\")", RegexOptions.IgnoreCase);
                var urls = regex.Matches(inputString).OfType<Match>().Select(m => m.Groups["href"].Value);
                return urls.ToList().Where(t => t.StartsWith("https://docs.google.com/forms/d/")).ToList();
            }
            catch (Exception)
            {

            }

            return resultList;
        }
        public static string GenerateFormData(string url, UserFormData userData)
        {

            WebClient webClient = new WebClient();
            var htmlResult = webClient.DownloadString(url); //DownloadHelper.GetStringFromUrl(url);
            var resultList = new List<string>();
            var ppp = new Dictionary<String, AirdropTextbox>();
            String formHtml = "";
            Match match = Regex.Match(htmlResult, @"<form[^>]*>(.*?)</form>",RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (match.Success)
            {
                // fetch result
                formHtml = match.Groups[0].Value;
               
            }
            String inputRegexResult = "";
            Match match2 = Regex.Match(htmlResult, @"<input[^>]*>(.*?)/>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (match2.Success)
            {
                // fetch result
                inputRegexResult = match2.Groups[0].Value;
            }
            var doc = new HtmlDocument();
            formHtml = Regex.Replace(formHtml, @"\t|\n|\r", "");
            doc.LoadHtml(formHtml);
            //  <form[^>]*>(.*?)</form>
            //  var inputNodes = doc.DocumentNode.SelectNodes("//input");
            //    var textareaNodes = doc.DocumentNode.SelectNodes("//textarea");
            //    var formNodes = doc.DocumentNode.SelectNodes("//form");
            formHtml = "";
            HtmlAgilityHelper.SearchInputForm(doc.DocumentNode.ChildNodes, userData, ppp);
            var docForm = new HtmlDocument();

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
            var htmlFormInput = String.Join("<br>", ppp.Where(t => t.Value.AirdropType != AirdropComponentType.Hidden && t.Key.StartsWith("input")).Select(x => x.Value.AirdropTextboxHtml).ToArray());
            htmlFormInput = htmlFormInput + String.Join("", ppp.Where(t => t.Value.AirdropType == AirdropComponentType.Hidden && t.Key.StartsWith("input")).Select(x => x.Value.AirdropTextboxHtml).ToArray());

            formHtml = formHtml.Replace("</form>", htmlFormInput);
            formHtml = formHtml + "<br><input type='submit' class='btn btn-success btn-lg btn-block' value='Submit AIR DROP Form'></form>";
            return formHtml;
        }

        public static void SearchInputForm(
            HtmlNodeCollection fieldNodes,
            UserFormData myFormData,
            Dictionary<String, AirdropTextbox> myFormHtml)
        {
            var allFormKeyItems = FormMatchRepository.GetFormMatchsFromCache();
            foreach (var currentNode in fieldNodes)
            {

                if (currentNode.NextSibling != null)
                {
                    SearchInputForm(currentNode.NextSibling.ChildNodes, myFormData, myFormHtml);
                }
                if (currentNode.PreviousSibling != null)
                {
                    SearchInputForm(currentNode.PreviousSibling.ChildNodes, myFormData, myFormHtml);
                }
                SearchInputForm(currentNode.ChildNodes, myFormData, myFormHtml);
                if (currentNode.Name.Equals("input",StringComparison.InvariantCultureIgnoreCase)
                    || currentNode.Name.Equals("textarea", StringComparison.InvariantCultureIgnoreCase))
                {

                    if (currentNode.Attributes.Any(t => t.Name.Equals("aria-label")))
                    {
                        //, UserFormData userData
                        string label = currentNode.Attributes["aria-label"].Value.ToStr().Trim();


                        var item = allFormKeyItems.FirstOrDefault(r => label.ToLower().Contains(r.FormItemText.ToLower()));
                        if (item == null)
                        {
                            item = allFormKeyItems.FirstOrDefault(r => r.FormItemText.ToLower().Contains(label.ToLower()));
                        }
                        if (item != null)
                        {
                            var property = myFormData.GetType().GetProperty(item.FormItemKey.ToStr().Trim());
                            string s = property.GetValue(myFormData, null) as string;
                            // property.SetValue(myFormData, System.Convert.ChangeType(s.ToStr().Trim(), property.PropertyType), null);
                            setFormValue(s.ToStr().Trim(), currentNode);
                        }

                        var t = new AirdropTextbox() { AirdropTextboxHtml = "<b>" + label + "</b>" + currentNode.OuterHtml };
                        if (currentNode.Name == "input")
                        {
                            t.AirdropType = AirdropComponentType.Textbox;
                        }
                        else
                        {
                            t.AirdropType = AirdropComponentType.Textarea;
                        }


                        myFormHtml["input_textbox_" + currentNode.Attributes["name"].Value] = t;

                    }
                    else
                    {
                        var t = new AirdropTextbox() { AirdropTextboxHtml = currentNode.OuterHtml };
                        myFormHtml["input_others_" + currentNode.Attributes["name"].Value] = t;
                        t.AirdropType = Entities.AirDropType.AirdropComponentType.Hidden;
                    }

                }
                else if (currentNode.Name == "form")
                {
                    var t = new AirdropTextbox() { AirdropTextboxHtml = currentNode.OuterHtml };
                    myFormHtml["form_" + currentNode.Attributes["action"].Value] = t;
                    t.AirdropType = Entities.AirDropType.AirdropComponentType.Form;
                }

            }
        }

        private static void setFormValue(String myFormData, HtmlNode currentNode)
        {
            if (currentNode.Name == "input")
            {
                currentNode.Attributes["value"].Value = myFormData;
            }
            else if (currentNode.Name == "textarea")
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
