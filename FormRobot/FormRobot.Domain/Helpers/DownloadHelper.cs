using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EImece.Domain.Helpers
{
    public class DownloadHelper
    {
        public static byte[] GetImageFromUrl(string url, Dictionary<String, String> dictionary)
        {
            System.Net.HttpWebRequest request = null;
            System.Net.HttpWebResponse response = null;
            byte[] b = null;

            if (dictionary == null)
            {
                dictionary = new Dictionary<String, String>();
            }

            request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            request.Timeout = 99999;
            response = (System.Net.HttpWebResponse)request.GetResponse();

            if (request.HaveResponse)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    using (BinaryReader br = new BinaryReader(receiveStream))
                    {
                        b = br.ReadBytes(500000);
                        br.Close();
                    }

                    foreach (var h in response.Headers.AllKeys)
                    {
                        dictionary.Add(h, response.Headers[h]);
                    }
                    dictionary.Add("ContentType", response.ContentType);
                }
            }

            return b;
        }
        public static String GetStringFromUrl(string url)
        {
            System.Net.HttpWebRequest request = null;
            System.Net.HttpWebResponse response = null;
            byte[] b = null;
 
            request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            request.Timeout = 99999;
            response = (System.Net.HttpWebResponse)request.GetResponse();

            if (request.HaveResponse)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var encoding = ASCIIEncoding.ASCII;
                    using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                    {
                        string responseText = reader.ReadToEnd();

                        return responseText;
                    }
                }
            }

            return String.Empty;
        }
    }
}
