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
    
    public partial class vw_ReorderProducts
    {
        public string SKU { get; set; }
        public Nullable<System.DateTime> Date_Last_Sale { get; set; }
        public Nullable<System.DateTime> Date_product_was_last_stocked { get; set; }
        public string Quantity_on_Hand { get; set; }
        public int Total_Number_of_Units_Sold_between_Last_Stock_and_Last_sale_date { get; set; }
        public Nullable<int> Total_Number_of_Units_Sold_in_Past_30_days { get; set; }
        public Nullable<decimal> Daily_Units_Sold_Rate_for_Past_30_Days { get; set; }
        public string Product_Title { get; set; }
        public int Total_Number_of_Days_from_last_stock_to_last_sale_date { get; set; }
        public Nullable<decimal> Daily_Units_Sold_Rate_from_Last_Restock_to_Last_Sale_Date { get; set; }
    }
}
