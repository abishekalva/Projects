using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloodDonorsHubScratch.Models
{
    public class RequestModel
    {
        public string EmployeeID { get; set; }
        public string BloodGroupRequested { get; set; }
        public string Location{ get; set; }
      
    }
}