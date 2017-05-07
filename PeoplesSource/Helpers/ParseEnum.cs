using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeoplesSource.Helpers
{
    public class ParseEnum
    {
        public static T Parse<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}