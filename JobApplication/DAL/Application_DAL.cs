using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobApplication.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.ComponentModel;

namespace JobApplication.DAL
{
    public class Application_DAL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ApplicationDB"].ToString();

        /// <summary>
        /// Get all student applicatons
        /// </summary>
        /// <returns></returns>
        public List<Applications> GetAllApplications()
        {
            List<Applications> applicationList = new List<Applications>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SPR_JobApplication", connection);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        applicationList.Add(new Applications
                        {
                            ApplicationID = Convert.ToInt32(reader["ApplicationID"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Place = reader["Place"].ToString(),
                            DOB = Convert.ToDateTime(reader["DOB"]),
                            ImageBase64 = reader["PhotoFile"] != DBNull.Value ?Convert.ToBase64String((byte[])reader["PhotoFile"]) : null,
                            ResumeBase64 = reader["ResumeFile"] != DBNull.Value ? Convert.ToBase64String((byte[])reader["ResumeFile"]) : null
                        });
                    }
                }
                connection.Close();
            }
            return applicationList;
        }
        /// <summary>
        /// Insert Applications to Database
        /// </summary>
        /// <param name="appplications"></param>
        /// <returns></returns>
        public bool InsertApplication(Applications applications)
        {
            int id = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SPI_JobApplication",connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FirstName", applications.FirstName);
                command.Parameters.AddWithValue("@LastName",applications.LastName);
                command.Parameters.AddWithValue("@Email", applications.Email);
                command.Parameters.AddWithValue("@Phone", applications.Phone);
                command.Parameters.AddWithValue("@Place", applications.Place);
                command.Parameters.AddWithValue("@DOB", applications.DOB);
                command.Parameters.AddWithValue("@PhotoFile", applications.Photo);
                command.Parameters.AddWithValue("@ResumeFile", applications.Resume);
                connection.Open();
                id = command.ExecuteNonQuery();
                connection.Close();
            }
            if (id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
      /// <summary>
      /// Get Application by id
      /// </summary>
      /// <param name="applicationid"></param>
      /// <returns></returns>
        public List<Applications> GetApplicationByID(int applicationID)
        {
            List<Applications> applicationList = new List<Applications>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetById";
                command.Parameters.AddWithValue("@ApplicationID", applicationID);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                connection.Open();
                adapter.Fill(dt);
                connection.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    applicationList.Add(new Applications
                    {
                        ApplicationID = Convert.ToInt32(dr["ApplicationID"]),
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        Email = dr["Email"].ToString(),
                        Phone = dr["Phone"].ToString(),
                        Place = dr["Place"].ToString(),
                        DOB = Convert.ToDateTime(dr["DOB"]),
                        ImageBase64 = dr["PhotoFile"] != DBNull.Value ? Convert.ToBase64String((byte[])dr["PhotoFile"]) : null,
                        ResumeBase64 = dr["ResumeFile"] != DBNull.Value ? Convert.ToBase64String((byte[])dr["ResumeFile"]) : null
                    });
                }
            }
            return applicationList;
        }
        /// <summary>
        /// Update application
        /// </summary>
        /// <param name="applications"></param>
        /// <returns></returns>
        public bool Update(Applications applications)
        {
            int i = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SPU_JobApplication", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ApplicationID", applications.ApplicationID);
                command.Parameters.AddWithValue("@FirstName", applications.FirstName);
                command.Parameters.AddWithValue("@LastName", applications.LastName);
                command.Parameters.AddWithValue("@Email", applications.Email);
                command.Parameters.AddWithValue("@Phone", applications.Phone);
                command.Parameters.AddWithValue("@Place", applications.Place);
                command.Parameters.AddWithValue("@DOB", applications.DOB);
                command.Parameters.AddWithValue("@PhotoFile", applications.Photo);
                command.Parameters.AddWithValue("@ResumeFile", applications.Resume);
                connection.Open();
                i = command.ExecuteNonQuery();
                connection.Close();
            }
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }   
        /// <summary>
        /// delete application
        /// </summary>
        /// <param name="applicationid"></param>
        /// <returns></returns>
        public string DeleteApplication(int applicationid)
        {
            string result = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SPD_JobApplication",connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ApplicationID", applicationid);
                command.Parameters.Add("@ReturnMessage", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                connection.Open();
                command.ExecuteNonQuery();
                result = command.Parameters["@ReturnMessage"].Value.ToString();
                connection.Close();
            }
            return result;
        }
    }
}