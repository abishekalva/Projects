using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer
{
    public class DataAcess
    {
        private static SqlConnection connection;
        private static SqlCommand command;
        private static SqlDataReader reader;

        //Creates only the single instance of a SqlConnection
        //<return> connection object </return>
        public static SqlConnection Connection()
        {
            if (connection == null)
            {
                connection = new SqlConnection();
            }
            return connection;
        }

        //Creates only the single instance of a SqlCommand
        //<return> Command object </return>
        public static SqlCommand Command()
        {
            if (command == null)
            {
                command = new SqlCommand();
            }
            return command;
        }

        //Returns the sql connection object to the caller function.
        //<return> Connection object </return>
        public static SqlConnection GetSqlConnection()
        {
            connection = Connection();
            connection.ConnectionString = GetConnectionString();
            return connection;
        }

        //Gets the connection string
        //<Return> connection string </Return>
        public static string GetConnectionString()
        {
            string connectionString = "Data Source = localhost; Initial Catalog = BloodDonorsHub; Integrated Security = true";
            return connectionString;
        }


        //Executes the command for the command text provided.
        //Should handle INSERT,UPDATE,DELETE 
        //<Return> True if success else failure </Return>
        public static bool ExecuteNonQuery(string commandText)
        {
            try
            {
                command.CommandText = commandText;
                using (connection = GetSqlConnection())
                { 
                    connection.Open();
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException)
            {
                return false;
            }
        }

        //This method checks if the Email already exists in the DB.
        //<return> Returns false if email exists else true </return>
        public static bool CheckIfEmailExists(string emailId)
        {
            string commandText = "Select * from Users where Email = '" + emailId + "'";
            using (connection = GetSqlConnection())
            {
                command = Command();
                connection.Open();
                command.Connection = connection;
                command.CommandText = commandText;
                reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        //Verify if Email Id & Password exists in the the DB
        //<return> Returns true if email id & password matches else false</return>
        public static bool CheckIfUserExists(string emailId,string password)
        {
            string commandText = "Select * from Users where Email ='" + emailId + "' And Password ='"+password+"';";
            using (connection = GetSqlConnection())
            {
                command = Command();
                connection.Open();
                command.Connection = connection;
                command.CommandText = commandText;
                reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //Method to get all the email ids from user table
        //<return> List of emails </return>
        public static List<string> GetDataRows(string commandText)
        {
            List<string> emailIds = new List<string>();
            using (connection = GetSqlConnection())
            {
                command = Command();
                connection.Open();
                command.Connection = connection;
                command.CommandText = commandText;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    emailIds.Add(reader[0].ToString());
                }
                return emailIds;
            }
        }



    }
}
