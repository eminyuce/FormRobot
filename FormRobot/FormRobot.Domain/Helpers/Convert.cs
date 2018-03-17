using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EImece.Domain.Helpers
{
    public static class Convert
    {

        public static int GetId(this string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return 0;
            }
  
            var m = id.Split("-".ToCharArray()).Last();
            return Base32Custom.DecodeRnd(m).ToInt();
        }
        public static string ToAlphaNumericOnly(this string input)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            return rgx.Replace(input, "");


        }

        public static string ToAlphaOnly(this string input)
        {
            Regex rgx = new Regex("[^a-zA-Z]");
            return rgx.Replace(input, "");
        }

        public static string ToNumericOnly(this string input)
        {
            Regex rgx = new Regex("[^0-9]");
            return rgx.Replace(input, "");
        }
        public static string Substring(this string str, string StartString, string EndString)
        {
            if (str.Contains(StartString))
            {
                int iStart = str.IndexOf(StartString) + StartString.Length;
                int iEnd = str.IndexOf(EndString, iStart);
                return str.Substring(iStart, (iEnd - iStart));
            }
            return null;
        }

        private static readonly Regex CarriageRegex = new Regex(@"(\r\n|\r|\n)+");
        //remove carriage returns from the header name
        public static string RemoveCarriage(this string text, string replace = "")
        {
            if (String.IsNullOrEmpty(text))
            {
                return "";
            }
            return CarriageRegex.Replace(text, replace).Trim();
        }

        public static string StripHtml(string html)
        {
            html = html.ToStr();
            char[] array = new char[html.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < html.Length; i++)
            {
                char let = html[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
        /// <summary>
        /// Parse the input string by placing a space between character case changes in the string
        /// </summary>
        /// <param name="strInput">The string to parse</param>
        /// <returns>The altered string</returns>
        public static string ParseByCase(this string strInput)
        {
            // The altered string (with spaces between the case changes)
            string strOutput = "";

            // The index of the current character in the input string
            int intCurrentCharPos = 0;

            // The index of the last character in the input string
            int intLastCharPos = strInput.Length - 1;

            // for every character in the input string
            for (intCurrentCharPos = 0; intCurrentCharPos <= intLastCharPos; intCurrentCharPos++)
            {
                // Get the current character from the input string
                char chrCurrentInputChar = strInput[intCurrentCharPos];

                // At first, set previous character to the current character in the input string
                char chrPreviousInputChar = chrCurrentInputChar;

                // If this is not the first character in the input string
                if (intCurrentCharPos > 0)
                {
                    // Get the previous character from the input string
                    chrPreviousInputChar = strInput[intCurrentCharPos - 1];

                } // end if

                // Put a space before each upper case character if the previous character is lower case
                if (char.IsUpper(chrCurrentInputChar) == true && char.IsLower(chrPreviousInputChar) == true)
                {
                    // Add a space to the output string
                    strOutput += " ";

                } // end if

                // Add the character from the input string to the output string
                strOutput += chrCurrentInputChar;

            } // next

            // Return the altered string
            return strOutput;

        } // end method

        public static string TruncateAtSentence(this string text, int length, int lengthMin = -1,
                                        bool addEllipsis = true)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= length) return text;


            const string ellChar = "…";
            const int defLengthMin = 20;
            char[] goodChars = { ',', ';' };
            char[] badChars = { ':', '-', '—', '–', ' ' };

            var r = text;
            if (lengthMin < 0 || lengthMin > length)
            {

                lengthMin = length * .8 > defLengthMin ? (length * 0.8).ToInt() : defLengthMin;

            }

            Regex rx = new Regex(@"(\S.+?[.!?])(?=\s+|$)");
            var ret = new StringBuilder();
            foreach (Match match in rx.Matches(text))
            {
                if (ret.ToString().Length + match.Value.Trim().Length + 1 <= length)
                {
                    if (ret.Length > 0)
                        ret.Append(" ");

                    ret.Append(match.Value.Trim());

                }
                else
                {
                    if (ret.Length >= lengthMin)
                        return ret.ToString();
                    else
                    {
                        if (ret.Length > 0)
                            ret.Append(" ");

                        ret.Append(match.Value.Trim());
                        break;
                    }
                }
            }

            r = ret.ToString().Trim();
            if (r.Length <= length) return r;

            int index;
            index = r.IndexOfAny(goodChars, lengthMin, length - lengthMin);
            if (index > 0)
                return addEllipsis ? r.Substring(0, index).Trim() + ellChar : r.Substring(0, index).Trim();

            index = r.IndexOfAny(badChars, lengthMin, length - lengthMin);
            if (index > 0)
                return addEllipsis ? r.Substring(0, index).Trim() + ellChar : r.Substring(0, index).Trim();

            return addEllipsis ? r.Substring(0, length).Trim() + ellChar : r.Substring(0, length).Trim();
        }


        public static string ToUrlFriendly(this string value)
        {


            return Regex.Replace(value, @"[^A-Za-z0-9_]+", "-");


        }



        public static int ToInt(this object arg, int defaultValue = 0)
        {
            int ret = defaultValue;

            if (int.TryParse(arg.ToStr(), out ret))
            {
                return ret;

            }
            else
            {
                return defaultValue;
            }


        }



        public static long ToLong(this object arg)
        {
            long ret = 0;

            long.TryParse(arg.ToStr(), out ret);

            return ret;
        }
        public static float ToFloat(this object arg)
        {
            float ret = 0;


            float.TryParse(arg.ToStr(), out ret);

            return ret;
        }
        public static decimal ToDecimal(this object arg)
        {
            decimal ret = 0;


            decimal.TryParse(arg.ToStr(), out ret);

            return ret;
        }
        public static double ToDouble(this object arg)
        {
            double ret = 0;


            double.TryParse(arg.ToStr(), out ret);

            return ret;
        }

        public static string ToTitleCase(this object arg)
        {
            string ret = string.Empty;
            if (arg != null)
            {
                ret = arg.ToString();
            }
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

            return myTI.ToTitleCase(ret);
        }




        public static string ReplaceHtmlNewLineWithNewLine(this object arg)
        {
            return ReplaceStr(arg, "<br>", "\n");
        }
        public static string ReplaceNewLineWithHtmlNewLine(this object arg)
        {
            return ReplaceStr(arg, "\n", "<br>");
        }
        public static string ReplaceStr(object arg, String first, String second)
        {
            string ret = string.Empty;
            if (arg != null)
            {
                ret = arg.ToString().Trim();
            }
            return ret.Replace(first, second);
        }


        public static string ToStr(this object arg, String defaultValue = "")
        {
            string ret = string.Empty;
            if (arg != null)
            {
                ret = arg.ToString().Trim();
            }
            if (String.IsNullOrEmpty(ret))
            {
                if (!String.IsNullOrEmpty(defaultValue))
                {
                    return defaultValue;
                }
            }
            return ret;
        }


        public static string ToStr(this object arg, int length)
        {
            string ret = string.Empty;
            if (arg != null)
            {
                ret = arg.ToString();
            }
            if (ret.Length > length)
            {
                return ret.Substring(0, length);
            }
            else
            {
                return ret;
            }
        }

        public static string ToStr(this string text, int minLen, int maxLen)
        {
            string s = text != null ? text : "";
            if (s.Length > maxLen) s = s.Substring(0, maxLen).Trim();

            int ix = 0;
            ix = s.LastIndexOf(".");
            if (ix > minLen)
            {
                s = s.Substring(0, ix + 1).Trim();
            }
            else if ((ix = s.LastIndexOf(",")) > minLen)
            {
                s = s.Substring(0, ix).Trim();

            }
            else if ((ix = s.LastIndexOf(" ")) > minLen)
            {
                s = s.Substring(0, ix).Trim();
            }

            return s;
        }

        public static bool HasValue(this object arg)
        {
            string ret = string.Empty;
            if (arg != null)
            {
                ret = arg.ToString();
            }
            return !string.IsNullOrEmpty(ret);
        }


        public static string ToTitleCase(this string text)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            return textInfo.ToTitleCase(text);
        }


        public static string ToSmartCase(this string text)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            if (string.Equals(text, text.ToLower()))
            {

                return textInfo.ToTitleCase(text);
            }


            return text;

        }

        public static string HtmlDecode(this string arg)
        {
            return WebUtility.HtmlDecode(arg);
        }
        public static string HtmlEncode(this string arg)
        {
            return WebUtility.HtmlEncode(arg);
        }
        public static string NoRepeatedSpaces(string arg)
        {

            return Regex.Replace(arg, " {2,}", " ");
        }


        public static string NoBreakLine(string arg)
        {

            return arg.Replace("\r", " ").Replace("\n", " ");
        }


        public static string ToNormal(this string arg)
        {
            return NoRepeatedSpaces(NoBreakLine(arg.HtmlDecode()));
        }


        public static bool ToBool(this object arg, bool defaultValue = false)
        {
            bool ret = defaultValue;
            var m = arg.ToStr();
            if (!String.IsNullOrEmpty(m) && !bool.TryParse(m, out ret))
            {
                if (arg.ToStr().ToLower().Contains((!defaultValue).ToString().ToLower()))
                {
                    ret = !defaultValue;
                }
            }


            return ret;
        }


        static Regex _dateRegex = new Regex(@"^(19|20)(\d\d)[- /.]?(0[1-9]|1[012])[- /.]?(0[1-9]|[12][0-9]|3[01])$", RegexOptions.Compiled);




        public static DateTime? ToNullableDateTime(this object arg)
        {
            DateTime ret = DateTime.MinValue;

            if (!DateTime.TryParse(arg.ToStr(), out ret))
            {
                Match md = _dateRegex.Match(arg.ToStr());

                if (md != null && md.Groups.Count == 5)
                {
                    int year = (md.Groups[1].Value + md.Groups[2].Value).ToInt();
                    int month = (md.Groups[3].Value).ToInt();
                    int day = (md.Groups[4].Value).ToInt();
                    try
                    {
                        ret = new DateTime(year, month, day);
                    }
                    catch { }
                }


            }

            if (ret != DateTime.MinValue)
            {
                return ret;
            }
            else
            {
                return null;
            }
        }



        public static DateTime ToDateTime(this object arg)
        {
            DateTime ret = DateTime.MinValue;

            if (!DateTime.TryParse(arg.ToStr(), out ret))
            {
                Match md = _dateRegex.Match(arg.ToStr());

                if (md != null && md.Groups.Count == 5)
                {
                    int year = (md.Groups[1].Value + md.Groups[2].Value).ToInt();
                    int month = (md.Groups[3].Value).ToInt();
                    int day = (md.Groups[4].Value).ToInt();
                    try
                    {
                        ret = new DateTime(year, month, day);
                    }
                    catch { }
                }


            }



            return ret;
        }



        public static string ToFlexDateTime(this DateTime dt)
        {

            string rt = "";
            if (dt > DateTime.Now.Date)
            {
                rt = dt.ToString("h:mmtt").ToLower();
            }
            //else if (dt > DateTime.Now.AddDays(-DateTime.Now.DayOfYear))
            //{

            //    rt = dt.ToString("MMM dd");
            //}
            else if (dt > DateTime.MinValue)
            {
                rt = dt.ToString("MMM dd, yyyy ");
            }
            return rt;
        }


        public static object DeepClone(object obj)
        {
            object objResult = null;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);

                ms.Position = 0;
                objResult = bf.Deserialize(ms);
            }
            return objResult;
        }



        public static string UrlEncode(this string text)
        {

            return UrlEncodeCustom(text);
            //return HttpUtility.UrlPathEncode(text.Replace(" ", "_")).ToLower();

            //char c;
            //((int) c).ToString("X");

        }

        public static string UrlDecode(this string text)
        {
            //return HttpUtility.UrlDecode(text);
            return UrlDecodeCustom(text);
            // return HttpUtility.UrlPathEncode(text).Replace("_", " ");
        }


        private static string UrlEncodeCustom(string text)
        {
            StringBuilder ret = new StringBuilder();

            foreach (var c in text)
            {
                if (c >= '0' && c <= '9' ||
                    c >= 'a' && c <= 'z' ||
                    c >= 'A' && c <= 'Z' ||
                    c == ' '
                    //|| c == '-'
                    )
                {
                    ret.Append(c);
                }
                else
                {
                    ret.Append("~" + ((int)c).ToString("X"));
                }
            }

            return ret.ToString().Replace(" ", "_");
        }


        private static string UrlDecodeCustom(string text)
        {

            var chars = text.Replace("_", " ").ToCharArray();

            StringBuilder ret = new StringBuilder();

            int i = 0;
            while (i < chars.Length)
            {
                char c = chars[i];
                if (c >= '0' && c <= '9' ||
                    c >= 'a' && c <= 'z' ||
                    c >= 'A' && c <= 'Z' ||
                    c == ' ' || c == '-')
                {
                    ret.Append(c);
                    i++;
                }
                else if (c == '~' && i + 2 < chars.Length)
                {

                    try
                    {
                        string hexValue = chars[i + 1].ToString() + chars[i + 2].ToString();
                        int intChar = int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
                        ret.Append((char)intChar);
                    }
                    catch (Exception)
                    {
                    }

                    i = i + 3;

                }
                else
                {
                    break;
                }
            }

            return ret.ToString();
        }


        public static string CalculateMD5Hash(string input)

        {

            // step 1, calculate MD5 hash from input

            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)

            {

                sb.Append(hash[i].ToString("X2"));

            }

            return sb.ToString();

        }


    }
}
