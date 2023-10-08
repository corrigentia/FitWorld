using DbConnectionTools;

using FitWorld.BLL.Mappers;
using FitWorld.Dal.Mappers;

using FitWorldApi.Mappers;
using FitWorldApi.Models;
using FitWorldApi.Models.Forms;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FitWorldApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly string _connectionString; // = ""; // "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FitWorld;Integrated Security=True;";

        public StudentsController(IOptions<MyOptions> optionsAccessor, IConfiguration configuration)
        {
            // _connectionString = configuration.GetConnectionString("FitWorld");
            _connectionString = optionsAccessor.Value.ConnectionString;
        }

        // DELETE api/<StudentsController>/5
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            Command command = new("DELETE FROM [Student] WHERE StudentId = @id;");
            command.AddParameters("Id", id);

            int rowsAffected = new Connection(_connectionString).ExecuteNonQuery(command);

            return rowsAffected == 1 ? Ok() : BadRequest();
        }

        // GET: api/<StudentsController>
        [HttpGet]
        public ActionResult<IEnumerable<StudentViewModel>?> GetAllStudents()
        {
            Command command = new(@"
SELECT 
[StudentId], 
[Email] 
FROM 
[Student]
ORDER BY
  [StudentId]
;
");

            IEnumerable<StudentViewModel>? students = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToStudent().DalToBllStudent().BllToApiStudent());

            return students is null ? BadRequest() : Ok(students.OrderBy(student => student.StudentId));
        }

        // GET api/<StudentsController>/5
        [HttpGet("{id:int}")]
        public ActionResult<StudentViewModel?> GetStudentById(int id)
        {
            Command command = new("SELECT StudentId ,Email FROM [Student] WHERE StudentId = @id;");
            command.AddParameters("id", id);
            StudentViewModel? student = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToStudent().DalToBllStudent().BllToApiStudent()).SingleOrDefault();

            return student is null ? NotFound("We don't have that student (anymore)...") : Ok(student);
        }

        // GET api/<StudentsController>/email/bruce@lee.com
        [HttpGet("email/{email}")]
        public ActionResult<StudentViewModel?> GetStudentByEmail(string email)
        {
            Command command = new("SELECT [StudentId], [Email] FROM [Student] WHERE [Email] = @email;");
            command.AddParameters("email", email);
            StudentViewModel? student = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToStudent().DalToBllStudent().BllToApiStudent()).SingleOrDefault();

            return student is null ? NotFound("We don't have that student (anymore)...") : Ok(student);
        }

        /**
         * Responses
Curl

curl -X 'GET' \
'https://localhost:7076/api/Students/bruce%40lee.cn' \
  -H 'accept: *\/*' \
-H 'Content-Type: application/json' \
-d '{
"email": "a@b.com"
}'

Request URL

https://localhost:7076/api/Students/bruce%40lee.cn

Server response
Code Details
Undocumented

TypeError: Window.fetch: HEAD or GET Request cannot have a body.
        [FromBody]
        [FromForm]
Responses
Code Description Links
200
        '[[' ']]'
Success
    No links
         */
        // GET api/<StudentsController>/bruce@lee.cn
        //[HttpGet("{email:regex(^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,}$)}")]
        //[HttpGet("{email:regex(\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*)}")]
        //[HttpGet(@"{email:regex(\w+([[-+.']]\w+)*@\w+([[-.]]\w+)*\.\w+([[-.]]\w+)*)}")] // only regex pattern that does NOT throw Errors, BUT is entirely IGNORED
        //[HttpGet("email/{email:regex(\\w+([[-+.']]\\w+)*@\\w+([[-.]]\\w+)*\\.\\w+([[-.]]\\w+)*)}")] // only regex pattern that does NOT throw Errors, BUT is entirely IGNORED
        //[HttpGet("{email}")]
        [HttpGet("emails/{email}")]
        /**
         * HACK: [FromHeader] does NOT throw an Error, BUT is IGNORED
         * HACK: [FromQuery]  does NOT throw an Error, BUT is IGNORED
         * HACK: [FromServices]  does NOT throw an Error, BUT gives: ...
         *
         * 500
Undocumented

Error: Internal Server Error
Response body
Download

System.InvalidOperationException: No service for type 'FitWorldApi.Models.Forms.StudentEmailSearchForm' has been registered.
         *
         * HACK: [FromRoute]  does NOT throw an Error and WORKS with string email
         * UNDONE: [FromRoute]  does NOT throw an Error, BUT gives: ... with StudentEmailSearchForm email
         *
         * curl -X 'GET' \
  'https://localhost:7076/api/Students/{email}' \
  -H 'accept: *\/*'

Request URL

https://localhost:7076/api/Students/{email}

Server response
Code Details
400
Undocumented

Error: Bad Request
Response body
Download

{
"type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
"title": "Bad Request",
"status": 400,
"traceId": "00-0b323a534bfc790cdb3aa902cce4119d-c8328ea1ff0f0b3e-00"
}

    Response headers

content-type: application/problem+json; charset=utf-8  date: Thu,12 Jan 2023 10:43:40 GMT server: Kestrel x-firefox-spdy: h2
         *
         */
        //public IActionResult FindStudentsByEmail([FromRoute] StudentEmailSearchForm email)
        public ActionResult<IEnumerable<StudentViewModel>?> FindStudentsByEmail(string email)
        {
            /*
            //return new HtmlHelper().ViewContext.RequestContext.RouteData.Values["language"].ToString();
            //return Ok(RoutingHttpContextExtensions.GetRouteData(HttpContext).Values);
            //return Ok(RoutingHttpContextExtensions.GetRouteData(HttpContext).Values.GetValueOrDefault("email"));
            //System.Web.Http.Routing.IHttpRouteData routeData = Request.GetRouteData();
            //return HttpContext.Current
            //return Ok(email);
            //Console.WriteLine(JsonSerializer.Serialize(email));
            */
            Command command = new(@"
SELECT 
[StudentId],
[Email] 
FROM 
[Student] 
WHERE 
[Email] LIKE CONCAT('%', TRIM(@email), '%')
ORDER BY
  [StudentId]
;
");

            /**
             *
            // email (:StudentEmailSearchForm)
            // System.ArgumentException: 'No mapping exists from object type FitWorldApi.Models.Forms.StudentEmailSearchForm to a known managed provider native type.'
            // Yes, you need to cast variables to the type corresponding to the database column type before you add them as a parameter, or at the very least to a type with an implicit conversion to the required parameter type
            // Solution: email.Email
             */
            //command.AddParameters("email", email.Email);
            command.AddParameters("email", email);
            IEnumerable<StudentViewModel>? students = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToStudent().DalToBllStudent().BllToApiStudent());

            return students is null ? NotFound("We don't have that student (anymore)... :-)") : Ok(students.OrderBy(student => student.StudentId));
        }

        /*
        // TODO: Remove duplicate Register
        // POST api/<StudentsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
        */

        // PUT api/<StudentsController>/5
        [HttpPut("{id:int}")]
        //public ActionResult Put(int id, [FromForm] StudentUpdateForm data)
        public ActionResult Put(int id, [FromBody] StudentUpdateForm data)
        {
            Console.WriteLine(JsonSerializer.Serialize(data));
            if (!ModelState.IsValid || data.Email is null || data.Password is null)
            {
                return BadRequest();
            }
            Command duplicateCommand = new("SELECT [Email] FROM [Student] WHERE [Email] = @Email;");
            duplicateCommand.AddParameters("Email", data.Email);

            StudentViewModel? existingStudent = new Connection(_connectionString).ExecuteReader(duplicateCommand, reader => new StudentViewModel()
            {
                Email = (string)reader["Email"],
            }).SingleOrDefault();

            if (existingStudent is null)
            {
                Command command = new("spStudentUpdate", true);

                command.AddParameters("StudentId", id);
                command.AddParameters("Email", data.Email);
                command.AddParameters("Password", data.Password);

                int rowsAffected = new Connection(_connectionString).ExecuteNonQuery(command);

                return rowsAffected == 1 ? Ok() : BadRequest();
            }
            else
            {
                return Conflict();
            }
        }
    }
}
