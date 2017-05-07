using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeoplesSource.Models
{
    public class TrackResponse
    {
        public string Tracking { get; set; }
        public string TrackSummary { get; set; }
        public List<TrackerDetailsResponseModel> TrackDetail { get; set; }
    }

}