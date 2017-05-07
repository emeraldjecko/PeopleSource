using System;
using System.Collections.Generic;

namespace PeoplesSource.Models
{
    public class ActivityOptionModel
    {
        public ActivityOptionModel()
        {
            ErrorItems = new List<ErrorModel>();
        }

        public List<ErrorModel> ErrorItems { get; set; }
        public List<string> ActivityOptions { get; set; }
        public string Ack { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Version { get; set; }
    }
}