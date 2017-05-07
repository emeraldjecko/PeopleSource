using System;
using System.Web;

namespace PeoplesSource.Helpers
{
    public static class SessionHelper
    {
        //public static string UserName
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session["UserName"] == null)
        //        {
        //            return string.Empty;
        //        }
        //        return Convert.ToString(HttpContext.Current.Session["UserName"]);
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["UserName"] = value;
        //    }
        //}

        //public static int ClientID
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session["ClientID"] == null)
        //        {
        //            return 0;
        //        }
        //        return Convert.ToInt32(HttpContext.Current.Session["ClientID"]);
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["ClientID"] = value;
        //    }
        //}

        public static int PhysicianID
        {
            get
            {
                if (HttpContext.Current.Session["PhysicianID"] == null)
                {
                    return 0;
                }
                return Convert.ToInt32(HttpContext.Current.Session["PhysicianID"]);
            }
            set
            {
                HttpContext.Current.Session["PhysicianID"] = value;
            }
        }

        //public static Guid UserID
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session["UserID"] == null)
        //        {
        //            return Guid.Empty;
        //        }
        //        return Guid.Parse(Convert.ToString(HttpContext.Current.Session["UserID"]));
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["UserID"] = value; ;
        //    }
        //}

        public static int ClientType
        {
            get
            {
                if (HttpContext.Current.Session["ClientType"] == null)
                {
                    return 0;
                }
                return Convert.ToInt32(HttpContext.Current.Session["ClientType"]);
            }
            set
            {
                HttpContext.Current.Session["ClientType"] = value;
            }
        }

        public static int IndividualPhysicianId
        {
            get
            {
                if (HttpContext.Current.Session["IndividualPhysicianId"] == null)
                {
                    return 0;
                }
                return Convert.ToInt32(HttpContext.Current.Session["IndividualPhysicianId"]);
            }
            set
            {
                HttpContext.Current.Session["IndividualPhysicianId"] = value;
            }
        }

    }
}