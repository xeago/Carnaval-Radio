using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace ExtensionMethods
{
    /// <summary>
    /// Summary description for ExtensionMethods
    /// </summary>
    public static class MyExtensions
    {
        public static string ToUrlString(this string s)
        {
            return s.ToLower().StartsWith(@"http://") ? s : ("http://" + s);
        }
    }
}