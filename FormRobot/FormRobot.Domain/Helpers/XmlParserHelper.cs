using EImece.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HelpersProject
{
    public class XmlParserHelper
    {
        public static string ToXml<T>(T objectToParse) where T : class, new()
        {
            if (objectToParse == null)
                throw new Exception("Unable to parse a object which is null.", new ArgumentNullException("objectToParse"));

            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(T));
            try
            {
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);
                serializer.Serialize(stringwriter, objectToParse);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Unable to serialize the object {0}.", objectToParse.GetType()), e);
            }

            return GeneralHelper.FormatXml(stringwriter.ToString());
        }

        public static T ToObject<T>(string xmlTextToParse) where T : class, new()
        {
             
            if (string.IsNullOrEmpty(xmlTextToParse))
                throw new Exception("Invalid string input. Cannot parse an empty or null string.", new ArgumentException("xmlTestToParse"));

            var stringReader = new System.IO.StringReader(xmlTextToParse);
            var serializer = new XmlSerializer(typeof(T));
            try
            {
                return serializer.Deserialize(stringReader) as T;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Unable to convert to given string into the type {0}. See inner exception for details.", typeof(T)), e);
            }
        }

    }
}
