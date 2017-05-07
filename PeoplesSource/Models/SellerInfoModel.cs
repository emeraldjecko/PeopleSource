using System;
using System.Collections.Generic;

namespace PeoplesSource.Models
{
    public class SellerInfoModel
    {
        public SellerInfoModel()
        {
            SellerErrorItems = new List<ErrorModel>();
        }

        public List<ErrorModel> SellerErrorItems { get; set; }
        public string Ack { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Version { get; set; }
    }
}