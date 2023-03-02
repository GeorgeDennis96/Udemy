using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using UdemyCourse.Models;

namespace UdemyCourse.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly string ConnectionString;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;

            ConnectionString = _configuration.GetConnectionString("EmployeeAppCon");
        }

        [HttpGet]
        public JsonResult Get()
        {
            string quary = @"select DepartmentId, DepartmentName from dbo.Department";

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
        public JsonResult Post(Department department)
        {
            string quary = @"insert into dbo.Department values(@DepartmentName)";

            DataTable table = new DataTable();

            SqlDataReader dataReader;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(quary, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    dataReader = sqlCommand.ExecuteReader();
                    table.Load(dataReader);
                    dataReader.Close();
                    sqlConnection.Close();
                }
            }

            return new JsonResult("Added Successfully.");
        }

        [HttpPut]
        public JsonResult Put(Department department)
        {
            string quary = @"update dbo.Department set DepartmentName= @DepartmentName where DepartmentId=@DepartmentId";

            DataTable table = new DataTable();

            SqlDataReader dataReader;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(quary, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentId", department.DepartmentId);
                    sqlCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
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
            string quary = @"delete from dbo.Department where DepartmentId=@DepartmentId";

            DataTable table = new DataTable();

            SqlDataReader dataReader;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(quary, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@DepartmentId", id);
                    dataReader = sqlCommand.ExecuteReader();
                    table.Load(dataReader);
                    dataReader.Close();
                    sqlConnection.Close();
                }
            }

            return new JsonResult("Deleted Successfully.");
        }
    }
}

