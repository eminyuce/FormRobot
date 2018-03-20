using EImece.Domain.Helpers;
using FormRobot.Domain.DB;
using FormRobot.Domain.Entities;
using FormRobot.Domain.Entities.AirDropType;
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
                SearchInputForm(currentNode.ChildNodes, myFormData, myFormHtml);
                if (currentNode.Name == "input" || currentNode.Name == "textarea")
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
                    

                        myFormHtml["input_textbox_" + currentNode.Attributes["name"].Value]  =t;

                    }
                    else
                    {
                        var t = new AirdropTextbox() { AirdropTextboxHtml = currentNode.OuterHtml };
                        myFormHtml["input_others_" + currentNode.Attributes["name"].Value] =   t ;
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
