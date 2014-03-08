using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Collections;

namespace BusinessLayer
{
    public class Users
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string BloodGroup { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }

        //Registers employees from the web
        //<return> Returns sucess on adding of record else failure </return>
        public static bool Register(Users users)
        {
            bool isSucessful = false;
            try
            {
                string toEmailId = users.Email;
                bool ifEmailExists = DataAccessLayer.DataAccess.CheckIfEmailExists(toEmailId);
                if (ifEmailExists)
                {
                    string commandText = "Insert into Users values('" + users.EmployeeId + "','" + users.EmployeeName + "','" + users.Password + "','" + users.City + "','" + users.BloodGroup + "','" + users.Email + "','" + users.PhoneNumber + "');";
                    isSucessful = DataAccessLayer.DataAccess.ExecuteNonQuery(commandText);
                    
                    //Email subject
                    string subject = "Registration successful";
                    
                    //Email Body
                    StringBuilder emailBody = new StringBuilder();
                    string url = @"<a href = 'http://localhost:47760/'>Click here to login now </a>";
                    emailBody.AppendFormat("<h1>Congratulations you have been registered successfully.</h1>");
                    emailBody.AppendFormat("Dear {0},", users.EmployeeName);
                    emailBody.AppendFormat("<br/>");
                    emailBody.AppendFormat("<p>Below are your login credentials:-</p>");
                    emailBody.AppendFormat("<br/>");
                    emailBody.AppendFormat("<p>User name: {0}", toEmailId);
                    emailBody.AppendFormat("<p>Password: {0}", users.Password);
                    emailBody.AppendFormat("<br/>");
                    emailBody.Append(url);

                    SendEmail(toEmailId, emailBody.ToString(),subject);
                }
            }
            catch (SqlException)
            {

            }
            return isSucessful;
        }

        //Sends an email from administrator emailid
        //<return> Returns void  </return>
        public static void SendEmail(string toEmailId,string emailBody,string subject)
        {

            //Get admin email from web.config file
            string fromEmailId = ConfigurationManager.AppSettings["AdminEmail"];
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["AdminEmail"], ConfigurationManager.AppSettings["AdminPassword"]);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            MailMessage message = new MailMessage(fromEmailId, toEmailId);
            message.Subject = subject;
            message.IsBodyHtml = true;
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(emailBody, null, "text/html");
            message.AlternateViews.Add(htmlView);
            smtp.Send(message);
        }

        //Validate if User exists in the database
        //<return> Returns true if user exists else failure </return>
        public static bool ValidateLoginUser(string useremailid,string userpassword)
        {
           
            bool validateUserExists = false;
            validateUserExists = DataAccessLayer.DataAccess.CheckIfUserExists(useremailid, userpassword);
            return validateUserExists;
        }
        
        //Gets Name of Employee based on Email ID, User in Welcome screen
        public static string getNameByEmail(string email)
        {
            string commandforEmployeeName = "Select * from Users where Email='" + email + "';";
            DataTable user = DataAccessLayer.DataAccess.ExecuteQuery(commandforEmployeeName);
            string EmployeeName = user.Rows[0]["EmployeeName"].ToString();
            return EmployeeName;
        }

        //Post a request to the users based on the criteria
        //<return> Returns void</return>
        public static void PostRequest(string EmployeeID, string BloodGroupRequested, string City)
        {
            //Email subject
            string subject = BloodGroupRequested+" Blood needed Urgently..!!!";

            string commandforEmployee = "Select * from Users where EmployeeID='" + EmployeeID + "';";
            DataTable user = DataAccessLayer.DataAccess.ExecuteQuery(commandforEmployee);
            string requester_Name=user.Rows[0]["EmployeeName"].ToString();
            string requester_Email = user.Rows[0]["Email"].ToString();
            string requester_PhoneNumber = user.Rows[0]["PhoneNumber"].ToString();


            //Email Body
            StringBuilder emailBody = new StringBuilder();
            emailBody.AppendFormat("<h2>A patient needs {0} blood immediately.If you're willing to donate please contact below:</h2>",BloodGroupRequested);
            emailBody.AppendFormat("Contact Details:-<br/>");
            emailBody.AppendFormat("Name: {0} <br/>", requester_Name);
            emailBody.AppendFormat("Email: {0} <br/>", requester_Email);
            emailBody.AppendFormat("Mobile: {0} <br/>", requester_PhoneNumber);
            emailBody.AppendFormat("<br/><br/>");
            emailBody.AppendFormat("Thank you,<br/> Blood Donors Hub");

            //Gets the email ids of users with specified criteria
            string commandtext = "Select Email from Users where BloodGroup='" + BloodGroupRequested + "' And City= '" + City + "';";
            List<string> emailids = new List<string>();
            emailids = DataAccessLayer.DataAccess.GetEmailIdAsStrings(commandtext);

            //Blast the blood request email to all users with required criteria
            foreach (var emailid in emailids)
            {
                SendEmail(emailid,emailBody.ToString(),subject ); 
            }
            
        }
    }

}
