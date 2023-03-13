using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebAPI.Models;

namespace WebAPI.DataAccess
{
    public class EmployeeDAO
    {
        private readonly string _connString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public List<EmployeeModel> GetAllEmployees()
        {
            List<EmployeeModel> employees = new List<EmployeeModel>();
            using (var conn = new SqlConnection(_connString))
            {
                string sqlString = @"SELECT * FROM Employee";
                using (var command = new SqlCommand(sqlString, conn))
                {
                    conn.Open();
                    SqlDataReader sqlDataReader = command.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            EmployeeModel employeeModel = new EmployeeModel
                            {
                                ID = sqlDataReader.GetInt32(0),
                                Name = sqlDataReader.GetString(1),
                                DateOfBirth = sqlDataReader.GetDateTime(2),
                                Designation = sqlDataReader.GetString(3),
                                HomeTown = sqlDataReader.GetString(4)
                            };
                            employees.Add(employeeModel);
                        }
                    }
                }
            }
            return employees;
        }

        public EmployeeModel GetEmployee(int id)
        {
            EmployeeModel employeeModel = new EmployeeModel();

            using (var conn = new SqlConnection(_connString))
            {
                string sqlString = @"SELECT * FROM Employee where Id = @id";
                using (var command = new SqlCommand(sqlString, conn))
                {
                    command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                    conn.Open();
                    SqlDataReader sqlDataReader = command.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            employeeModel.ID = id;
                            employeeModel.Name = sqlDataReader.GetString(1);
                            employeeModel.DateOfBirth = sqlDataReader.GetDateTime(2);
                            employeeModel.Designation = sqlDataReader.GetString(3);
                            employeeModel.HomeTown = sqlDataReader.GetString(4);
                        }
                    }
                }
            }
            return employeeModel;
        }

        public bool DeleteEmployee(int id)
        {
            EmployeeModel employeeModel = new EmployeeModel();
            try
            {
                using (var conn = new SqlConnection(_connString))
                {
                    string sqlString = @"Delete FROM Employee where Id = @id";
                    using (var command = new SqlCommand(sqlString, conn))
                    {
                        command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
                        conn.Open();
                        SqlDataReader sqlDataReader = command.ExecuteReader();
                        if (sqlDataReader.HasRows)
                        {
                            while (sqlDataReader.Read())
                            {
                                employeeModel.ID = id;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddEmployee(EmployeeModel employeeModel)
        {
            try
            {
                using (var conn = new SqlConnection(_connString))
                {
                    string sqlString = @"INSERT INTO Employee(name, dateOfBirth, designation, homeTown) VALUES (@name, @dateOfBirth, @designation, @homeTown);";
                    using (var command = new SqlCommand(sqlString, conn))
                    {
                        command.Parameters.Add("@name", System.Data.SqlDbType.NVarChar).Value = employeeModel.Name;
                        command.Parameters.Add("@dateOfBirth", System.Data.SqlDbType.SmallDateTime).Value = employeeModel.DateOfBirth;
                        command.Parameters.Add("@designation", System.Data.SqlDbType.NVarChar).Value = employeeModel.Designation;
                        command.Parameters.Add("@homeTown", System.Data.SqlDbType.NChar).Value = employeeModel.HomeTown;
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}