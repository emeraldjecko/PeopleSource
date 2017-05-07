using System;
using System.Collections.Generic;

namespace PeoplesSource.Models
{
    public class TrackingInfoModel
    {
        public TrackingInfoModel()
        {
            TrackingErrorItems = new List<ErrorModel>();
        }

        public List<ErrorModel> TrackingErrorItems { get; set; }
        public string DeliveryStatus { get; set; }
        public string Ack { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Version { get; set; }
    }
}