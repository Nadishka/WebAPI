using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using WebAPI.DataAccess;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class EmployeeController : ApiController
    {

        public IHttpActionResult Get()
        {
            EmployeeDAO employeeDAO = new EmployeeDAO();
            List<EmployeeModel> employees = employeeDAO.GetAllEmployees();

            return Json(employees);
        }

        public IHttpActionResult Get(int id)
        {
            EmployeeDAO employeeDAO = new EmployeeDAO();
            EmployeeModel employee = employeeDAO.GetEmployee(id);

            return Json(employee);
        }

        [Route("api/Employee/Other")]
        [HttpGet]
        public IHttpActionResult Other()
        {
            Dictionary<string, string> map = new Dictionary<string, string>
            {
                { "1", "11" },
                { "2", "22" },
                { "3", "33" },
                { "4", "44" },
                { "5", "55" }
            };
            return Json(map);
        }

        [Route("api/Employee/list")]
        public List<string> GetList()
        {
            return new List<string> { "Data1", "Data2" };
        }

        [Route("api/Employee/E")]
        public IEnumerable<string> GetE()
        {
            return new string[] { "Data1", "Data2" };
        }

        [Route("api/Employee/Msg/{id}")]
        public HttpResponseMessage GetMsg(int id)
        {
            if (id >= 0)
            {
                EmployeeDAO employeeDAO = new EmployeeDAO();
                EmployeeModel employee = employeeDAO.GetEmployee(id);
                return Request.CreateResponse(HttpStatusCode.OK, "Employee name is " + employee.Name);
            }
            else
            {
                //return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Employee Id");
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Employee Id");
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            if (id < 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Employee Id");
            }
            else
            {
                EmployeeDAO employeeDAO = new EmployeeDAO();
                bool result = employeeDAO.DeleteEmployee(id);
                if (result)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "done");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Something went wrong");
                }
            }
        }

        public HttpResponseMessage Post(string name, DateTime DOB, string designation, string homeTown)
        {
            EmployeeModel employeeModel = new EmployeeModel()
            {
                Name = name,
                DateOfBirth = DOB,
                Designation = designation,
                HomeTown = homeTown
            };
            EmployeeDAO employeeDAO = new EmployeeDAO();
            if (employeeDAO.AddEmployee(employeeModel))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Successfully Added");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Something went wrong");
            }
        }

        public HttpResponseMessage Put([FromUri]int id, [FromBody]EmployeeModel employeeModel)
        {
            //EmployeeModel employeeModel = new EmployeeModel()
            //{
            //    ID = id,
            //    Name = name,
            //    DateOfBirth = DOB,
            //    Designation = designation,
            //    HomeTown = homeTown
            //};
            employeeModel.ID = id;
            EmployeeDAO employeeDAO = new EmployeeDAO();
            if (employeeDAO.UpdateEmployee(employeeModel))
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Successfully Updated");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Something went wrong");
            }
        }
    }
}
