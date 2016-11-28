using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Caracal.Common
{
    public static class StringExtensions
    {
        public static bool IsNumber(this string text)
        {
            return text.ToCharArray().ToList().TrueForAll(x => char.IsNumber(x));
        }
        public static bool IsDigit(this string text)
        {
            return text.ToCharArray().ToList().TrueForAll(x => char.IsDigit(x));
        }
        public static bool IsLetter(this string text)
        {
            return text.ToCharArray().ToList().TrueForAll(x => char.IsLetter(x));
        }
        public static bool IsUpper(this string text)
        {
            return text.ToCharArray().ToList().TrueForAll(x => char.IsUpper(x));
        }
        public static bool IsLower(this string text)
        {
            return text.ToCharArray().ToList().TrueForAll(x => char.IsLower(x));
        }
        public static bool IsHexadecimal(this string text)
        {
            return Regex.IsMatch(text, @"\A\b[0-9a-fA-F]+\b\Z");
        }
        public static T To<T>(this string text)
        {
            try
            {
                Type typeT = typeof(T);
                T x = (T)Convert.ChangeType(text, typeT);
                return x;
            }
            catch (InvalidCastException) { }
            return Activator.CreateInstance<T>();
        }
        public static string Limit(this string text, int value)
        {
            if (text != null)
                return text.Length > value ? text.Substring(0, value) : text;
            return null;
        }
    }
}
