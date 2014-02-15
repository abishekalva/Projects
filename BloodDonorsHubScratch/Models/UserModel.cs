using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloodDonorsHubScratch.Models
{
    public class UserModel
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string BloodGroup { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
    }
}