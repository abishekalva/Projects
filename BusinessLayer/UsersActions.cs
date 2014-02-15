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
using DataAccessModels;

namespace BusinessLayer
{
    public class UserActions
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
                bool ifEmailExists = DataAcessLayer.DataAcess.CheckIfEmailExists(toEmailId);
                if (ifEmailExists)
                {
                    string commandText = "Insert into Users values('" + users.EmployeeId + "','" + users.EmployeeName + "','" + users.Password + "','" + users.City + "','" + users.BloodGroup + "','" + users.Email + "','" + users.PhoneNumber + "');";
                    isSucessful = DataAcessLayer.DataAcess.ExecuteNonQuery(commandText);
                    
                    //Email subject
                    string subject = "Registration successful";
                    
                    //Email Body
                    StringBuilder emailBody = new StringBuilder();
                    string url = @"<a href = 'http://localhost:47760/'>Click here to login now </a>";
                    emailBody.AppendFormat("<h1>Congratulations you have been registered successfully.</h1>");
                    emailBody.AppendFormat("Dear {0},", users.EmployeeName);
                    emailBody.AppendFormat("<br/><br/>");
                    emailBody.AppendFormat("<p>Below are your login credentials:-</p>");
                    emailBody.AppendFormat("<br/>");
                    emailBody.AppendFormat("<p>User name: {0}", toEmailId);
                    emailBody.AppendFormat("<p>Password: {0}", users.Password);
                    emailBody.AppendFormat("<br/><br/>");
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
            validateUserExists = DataAcessLayer.DataAcess.CheckIfUserExists(useremailid, userpassword);
            return validateUserExists;
        }


        //Post a request to the users based on the criteria
        //<return> Returns void</return>
        public static void PostRequest(string EmployeeID, string BloodGroupRequested, string City)
        {
            //Email subject
            string subject = BloodGroupRequested+" Blood needed Urgently..!!!";

            string commandforEmployee = "Select * from Users where EmployeeID='" + EmployeeID + "';";
            DataTable user = DataAcessLayer.DataAcess.ExecuteQuery(commandforEmployee);
            string ename=user.Rows[0]["EmployeeName"].ToString();
            string email = user.Rows[0]["Email"].ToString();


            ////Email Body
            //StringBuilder emailBody = new StringBuilder();
            //emailBody.AppendFormat("<h1>Congratulations you have been registered successfully.</h1>");
            ////emailBody.AppendFormat("Dear {0},", employeeName[0]);
            //emailBody.AppendFormat("<br/><br/>");
            //emailBody.AppendFormat("<p>Below are your login credentials:-</p>");
            //emailBody.AppendFormat("<br/>");
            //emailBody.AppendFormat("<p>User name: {0}", user.Email);
            //emailBody.AppendFormat("<p>Password: {0}", user.Password);
            //emailBody.AppendFormat("<br/><br/>");

            ////Gets the email ids of users with specified criteria
            //string commandtext = "Select Email from Users where BloodGroup='"+BloodGroupRequested+"' And City= '"+City+"';";
            //List<string> emailids = new List<string>();
            //emailids = DataAcessLayer.DataAcess.GetEmailIdAsStrings(commandtext);          

            ////Blast the blood request email to all users with required criteria
            //foreach(var emailid in emailids)
            //{
            //    SendEmail(subject, emailBody.ToString(), emailid);
            //}
            
        }
    }

}
