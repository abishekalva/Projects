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
                bool ifEmailExists = DataAcessLayer.DataAcess.CheckIfEmailExists(toEmailId);
                if (ifEmailExists)
                {
                    string commandText = "Insert into Users values('" + users.EmployeeId + "','" + users.EmployeeName + "','" + users.Password + "','" + users.City + "','" + users.BloodGroup + "','" + users.Email + "','" + users.PhoneNumber + "');";
                    isSucessful = DataAcessLayer.DataAcess.ExecuteNonQuery(commandText);
                    string emailBody = CreateEmailBody(users.EmployeeName, users.Password, toEmailId);
                    SendEmail(toEmailId, emailBody);
                }
            }
            catch (SqlException)
            {

            }
            return isSucessful;
        }


        //Creates a mail body for registered users
        public static string CreateEmailBody(string employeeName, string password, string emailId)
        {
            StringBuilder builder = new StringBuilder();
            string url = @"<a href = 'http://localhost:47760/Login/Login'>Click here to login now </a>";
            builder.AppendFormat("<h1>Congratulations you have been registered successfully.</h1>");
            builder.AppendFormat("Dear {0},", employeeName);
            builder.AppendFormat("<br />");
            builder.AppendFormat("<p>Below are your login credentials</p>");
            builder.AppendFormat("<p>User name: {0}", emailId);
            builder.AppendFormat("<p>Password: {0}", password);
            builder.AppendFormat("<br />");
            builder.Append(url);
            return builder.ToString();
        }


        //Sends an email for all the users
        public static void SendEmail(string toEmailId, string emailBody)
        {
            List<string> emailIds = new List<string>();
            //string commandText = "select Email from Users";
            //emailIds = DataAcessLayer.DataAcess.GetDataRows(commandText);

            //Get admin email from web.config file
            string fromEmailId = ConfigurationManager.AppSettings["AdminEmail"];

            //var fromName = new MailAddress("blooddonarhub@gmail.com", "From Name");
            string subject = "Registration successful";

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


        public static bool ValidateLoginUser(string useremailid,string userpassword)
        {
           
            bool validateUserExists = false;
            validateUserExists = DataAcessLayer.DataAcess.CheckIfUserExists(useremailid, userpassword);
            return validateUserExists;
        }

        public static void PostRequest(string EmployeeID, string BloodGroupRequested, string Location)
        {
            string commandtext = "Select Email from Users where BloodGroup='"+BloodGroupRequested+"' And City= '"+Location+"';";
            List<string> emailids = new List<string>();
            emailids=DataAcessLayer.DataAcess.GetDataRows(commandtext);
        }
    }

}
