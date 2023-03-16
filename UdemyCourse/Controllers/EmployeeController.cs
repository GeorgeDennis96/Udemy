using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using UdemyCourse.Models;

namespace UdemyCourse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly string ConnectionString;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
            ConnectionString = _configuration.GetConnectionString("EmployeeAppCon");
        }


        [HttpGet]
        public JsonResult Get()
        {
            string quary = @"select EmployeeId, EmployeeName, Department, convert(varchar(10), DateOfJoining,120) as DateOfJoining,PhotoFileName from dbo.Employee";

            DataTable table = new DataTable();

            SqlDataReader dataReader;
            using (SqlConnection myCon = new SqlConnection(ConnectionString))
            {
                myCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(quary, myCon))
                {
                    dataReader = sqlCommand.ExecuteReader();
                    table.Load(dataReader);
                    dataReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            string quary = @"insert into dbo.Employee
                           (EmployeeName,Department,DateOfJoining,PhotoFileName)
                    values (@EmployeeName,@Department,@DateOfJoining,@PhotoFileName)
                            ";



            DataTable table = new DataTable();

            SqlDataReader dataReader;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(quary, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    sqlCommand.Parameters.AddWithValue("@Department", employee.Department);
                    sqlCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                    sqlCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
                    dataReader = sqlCommand.ExecuteReader();
                    table.Load(dataReader);
                    dataReader.Close();
                    sqlConnection.Close();
                }
            }

            return new JsonResult("Added Successfully.");
        }

        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            string quary = @"update dbo.Employee
                           set EmployeeName= @EmployeeName, Department= @Department, DateOfJoining= @DateOfJoining, PhotoFileName= @PhotoFileName 
                           where EmployeeId=@EmployeeId";

            DataTable table = new DataTable();

            SqlDataReader dataReader;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(quary, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    sqlCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    sqlCommand.Parameters.AddWithValue("@Department", employee.Department);
                    sqlCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                    sqlCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
                    dataReader = sqlCommand.ExecuteReader();
                    table.Load(dataReader);
                    dataReader.Close();
                    sqlConnection.Close();
                }
            }

            return new JsonResult("Updated Successfully.");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string quary = @"delete from dbo.Employee where EmployeeId=@EmployeeId";

            DataTable table = new DataTable();

            SqlDataReader dataReader;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(quary, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", id);
                    dataReader = sqlCommand.ExecuteReader();
                    table.Load(dataReader);
                    dataReader.Close();
                    sqlConnection.Close();
                }
            }

            return new JsonResult("Deleted Successfully.");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SavFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                var fileName = postedFile.FileName;
                var physicalPath = _environment.ContentRootPath +"/Photos/"+ fileName;

                using (var stream=new FileStream(physicalPath,FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(postedFile.FileName);

            }
            catch (Exception)
            {

                return new JsonResult("anonymous.png");
            }
        }
    }
}
