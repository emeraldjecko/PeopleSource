//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PeoplesSource.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Filteration
    {
        public decimal FilterID { get; set; }
        public string Action_Type { get; set; }
        public string Action_Note { get; set; }
        public Nullable<bool> Action_Delete { get; set; }
        public Nullable<decimal> Tagid { get; set; }
        public string Filter_From { get; set; }
        public string Filter_To { get; set; }
        public string Filter_Subject { get; set; }
        public string Filter_HasWord { get; set; }
        public Nullable<bool> Filter_HasNote { get; set; }
        public string Filter_TagName { get; set; }
        public string Filter_FromDate { get; set; }
        public string Filter_ToDate { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
    
        public virtual Tag Tag { get; set; }
    }
}
