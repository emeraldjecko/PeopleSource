using System;
using System.Collections.Generic;

namespace PeoplesSource.Models
{
    public class MemberMessage
    {
        public MemberMessage()
        {
            ErrorMessageItems = new List<ErrorMessageModel>();
        }

        public string Ack { get; set; }
        public string Build { get; set; }
        public string CorrelationId { get; set; }
        public string HardExpirationWarning { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Version { get; set; }
        public List<ErrorMessageModel> ErrorMessageItems { get; set; }

    }
}