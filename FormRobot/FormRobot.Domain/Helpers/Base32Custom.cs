using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace EImece.Domain.Helpers
{
    public static class Base32Custom
    {


        private static readonly Dictionary<string, int> CONVERT = new Dictionary<string, int>
        {
            {"0",0},
            {"O",0},
            {"1",1},
            {"L",1},
            {"I",1},
            {"2",2},
            {"3",3},
            {"4",4},
            {"5",5},
            {"6",6},
            {"7",7},
            {"8",8},
            {"9",9},
            {"A",10},
            {"B",11},
            {"C",12},
            {"D",13},
            {"E",14},
            {"F",15},
            {"G",16},
            {"H",17},
            {"J",18},
            {"K",19},
            {"M",20},
            {"N",21},
            {"P",22},
            {"Q",23},
            {"R",24},
            {"S",25},
            {"T",26},
            {"V",27},
            {"W",28},
            {"X",29},
            {"Y",30},
            {"Z",31}

        };


        public static string Encode(long arg)
        {

            StringBuilder Base32 = new StringBuilder();

            long BASE = 32;
            long remainder;

            while (arg != 0)
            {
                remainder = arg % BASE;  // assume K > 1
                arg = arg / BASE;  // integer division
                string key = CONVERT.Where(p => p.Value == remainder).FirstOrDefault().Key;

                Base32.Insert(0, key);
            }

            return Base32.ToString();
        }





        public static long Decode(string arg)
        {
            long Base10 = 0;
            long BASE = 32;


            CharEnumerator chars = arg.ToUpperInvariant().GetEnumerator();

            int power = arg.Length - 1;
            while (chars.MoveNext())
            {

                int val = CONVERT.Where(p => p.Key == chars.Current.ToString()).FirstOrDefault().Value;
                Base10 += val * (long)Math.Pow(BASE, power);

                --power;
            }

            return Base10;
        }



        public static string EncodeEncrypted(long arg)
        {
            return (Encode(EncryptNumber3(arg))).ToLower();
        }


        public static long DecodeEncrypted(string arg)
        {
            return (DecryptNumber3(Decode(arg.ToStr())));
        }

        public static string EncodeRnd(long arg)
        {
            var sArg = arg.ToString();
            if (sArg.Length <= 16)
            {
                Random rnd = new Random();
                string solt = rnd.Next(11, 99).ToString();
                sArg = solt + sArg;
                return (Encode(EncryptNumber3(sArg.ToLong()))).ToLower();
            }
            else
            {
                return "error";
            }



        }


        public static long DecodeRnd(string arg)
        {
            var sVal = (DecryptNumber3(Decode(arg.ToStr()))).ToString();

            if (sVal.Length > 2)
            {
                sVal = sVal.Substring(2, sVal.Length - 2);
                return sVal.ToLong();
            }
            else
            {
                return 0;
            }

        }





        private static long DigitSum(long arg)
        {
            long ret = arg;

            int digit;

            do
            {
                arg = ret;
                ret = 0;
                foreach (char ch in arg.ToString().ToCharArray())
                {
                    int.TryParse(ch.ToString(), out digit);
                    ret += digit;
                }
            }
            while (ret > 9);

            return ret;
        }




        private static long EncryptNumber3(long arg)
        {
            long ret = arg;
            string check = DigitSum(arg << 3).ToString() + DigitSum(arg >> 1) + DigitSum(arg << 2).ToString();

            long.TryParse(arg.ToString() + check, out ret);
            return ret;
        }


        private static long DecryptNumber3(long arg)
        {
            long ret = 0;
            string sarg = arg.ToString();
            int sargLenght = sarg.Length;
            if (sargLenght > 3)
            {
                long number;
                long.TryParse(sarg.Substring(0, sargLenght - 3), out number);
                string checknumber = DigitSum(number << 3).ToString() + DigitSum(number >> 1) + DigitSum(number << 2).ToString();
                string check = sarg.Substring(sargLenght - 3, 3);

                if (check == checknumber) ret = number;

            }


            return ret;
        }

    }
}