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
    
    public partial class Manage_OrderMessage
    {
        public decimal OrderMessageId { get; set; }
        public decimal ItemID { get; set; }
        public int Itemtagid { get; set; }
        public decimal Orderid { get; set; }
        public string Status { get; set; }
    
        public virtual ItemTag ItemTag { get; set; }
    }
}
