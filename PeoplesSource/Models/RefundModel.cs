using System;
using System.Collections.Generic;

namespace PeoplesSource.Models
{
    public class RefundModel
    {
        public RefundModel()
        {
            RefundErrorItems = new List<ErrorModel>();
        }

        public List<ErrorModel> RefundErrorItems { get; set; }
        public string RefundStatus { get; set; }
        public string Ack { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Version { get; set; }
    }
}