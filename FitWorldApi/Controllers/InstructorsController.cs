using DbConnectionTools;

using FitWorld.BLL.Mappers;
using FitWorld.Dal.Mappers;

using FitWorldApi.Mappers;
using FitWorldApi.Models;
using FitWorldApi.Models.Forms;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using System.Data.SqlClient;

//using System.Web.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FitWorldApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        private readonly string _connectionString;

        public InstructorsController(IOptions<MyOptions> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        // GET: api/<InstructorsController>
        [HttpGet]
        public ActionResult<IEnumerable<InstructorViewModel>> Get()
        {
            Command command = new(@"
SELECT 
[InstructorId], 
[FirstName], 
[LastName] 
FROM 
[Instructor]
ORDER BY
  [InstructorId]
;
");

            IEnumerable<InstructorViewModel>? instructors = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToInstructor().DalToBllInstructor().BllToApiInstructor());

            return instructors is null ? BadRequest() : Ok(instructors.OrderBy(instructor => instructor.InstructorId));
        }

        // GET api/<InstructorsController>/5
        [HttpGet("{id:int:regex(^\\d+$)}")]
        public ActionResult<InstructorViewModel> Get(int id)
        {
            Command command = new(@"
SELECT
  [InstructorId]
  ,[FirstName]
  ,[LastName]
FROM
  [Instructor]
WHERE
  [InstructorId] = @Id
;
");
            command.AddParameters("Id", id);
            InstructorViewModel? instructor = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToInstructor().DalToBllInstructor().BllToApiInstructor()).SingleOrDefault();

            return instructor is null ? NotFound("Doesn't teach with us (anymore)...") : Ok(instructor);
        }

        // GET api/<InstructorsController>/name/lee
        [HttpGet("name/{name:regex([[\\w-[[\\d]]]]+):length(1,510)}")]
        public ActionResult<IEnumerable<InstructorViewModel>> GetByName([FromRoute] string name)
        {
            Command command = new(@"
SELECT
  [InstructorId]
  ,[FirstName]
  ,[LastName]
FROM
  [Instructor]
WHERE
  [FirstName] 
LIKE CONCAT('%', TRIM(@Name), '%')
  OR
  [LastName] 
LIKE CONCAT('%', TRIM(@Name), '%')
  OR
  CONCAT ([FirstName], ' ', [LastName]) 
LIKE CONCAT('%', TRIM(@Name), '%')
  OR
  CONCAT ([LastName], ' ', [FirstName]) 
LIKE CONCAT('%', TRIM(@Name), '%')
ORDER BY
  [InstructorId]
;
");
            command.AddParameters("Name", name);
            IEnumerable<InstructorViewModel>? instructors = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToInstructor().DalToBllInstructor().BllToApiInstructor());

            return instructors is null ? NotFound("Doesn't teach with us (anymore)... :-(") : Ok(instructors.OrderBy(instructor => instructor.InstructorId));
        }

        // GET api/<InstructorsController>/firstName/bruce
        [HttpGet("firstName/{firstName:regex([[\\w-[[\\d]]]]+):length(1,255)}")]
        public ActionResult<IEnumerable<InstructorViewModel>> GetByFirstName(string firstName)
        {
            Command command = new(@"
SELECT
  [InstructorId]
  ,[FirstName]
  ,[LastName]
FROM
  [Instructor]
WHERE
  [FirstName] = TRIM(@FirstName)
ORDER BY
  [InstructorId]
;
");
            command.AddParameters("FirstName", firstName);
            IEnumerable<InstructorViewModel>? instructors = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToInstructor().DalToBllInstructor().BllToApiInstructor());

            return instructors is null ? NotFound("Doesn't teach with us (anymore)... :-(") : Ok(instructors.OrderBy(instructor => instructor.InstructorId));
        }

        // GET api/<InstructorsController>/firstName/bruce
        //[HttpGet("firstNames/{firstName:regex([[\\w-[[\\d]]]]+):length(1,255)}")]
        [HttpGet("firstNames/{firstName}")]
        public ActionResult<IEnumerable<InstructorViewModel>> FindInstructorsByFirstName(string firstName)
        {
            Command command = new(@"
SELECT
  [InstructorId]
  ,[FirstName]
  ,[LastName]
FROM
  [Instructor]
WHERE
  [FirstName] 
LIKE CONCAT('%', TRIM(@FirstName), '%')
ORDER BY
  [InstructorId]
;
");
            command.AddParameters("FirstName", firstName);
            IEnumerable<InstructorViewModel>? instructors = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToInstructor().DalToBllInstructor().BllToApiInstructor());

            return instructors is null ? NotFound("Doesn't teach with us (anymore)... :-(") : Ok(instructors.OrderBy(instructor => instructor.InstructorId));
        }

        // GET api/<InstructorsController>/lastName/lee
        //[HttpGet("lastName/{lastName:regex([[\\w-[[\\d]]]]+):length(1,255)}")]
        [HttpGet("lastName/{lastName?}")]
        public ActionResult<IEnumerable<InstructorViewModel>> GetByLastName(string? lastName)
        {
            string comparisonChoice = lastName is null ? " IS NULL " : " = TRIM(@LastName) ";
            string sqlQueryString = @"
SELECT
  [InstructorId]
  ,[FirstName]
  ,[LastName]
FROM
  [Instructor]
WHERE
  [LastName] " + comparisonChoice +
              @"
ORDER BY
  [InstructorId]
;
"
            ;

            Command command = new(sqlQueryString);

            command.AddParameters("LastName", lastName is null ? DBNull.Value : lastName);
            IEnumerable<InstructorViewModel>? instructors = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToInstructor().DalToBllInstructor().BllToApiInstructor());

            return instructors is null ? NotFound("Doesn't teach with us (anymore)... :-(") : Ok(instructors.OrderBy(instructor => instructor.InstructorId));
        }

        // GET api/<InstructorsController>/lastName/lee
        //[HttpGet("lastNames/{lastName:regex([[\\w-[[\\d]]]]+):length(1,255)}")]
        [HttpGet("lastNames/{lastName?}")]
        public ActionResult<IEnumerable<InstructorViewModel>> FindInstructorsByLastName(string? lastName)
        {
            string comparisonChoice = lastName is null ? " IS NULL " : " LIKE CONCAT('%', TRIM(@LastName), '%') ";
            string sqlQueryString = @"
SELECT
  [InstructorId]
  ,[FirstName]
  ,[LastName]
FROM
  [Instructor]
WHERE
  [LastName] " + comparisonChoice +
              @"
ORDER BY
  [InstructorId]
;
"
            ;
            Command command = new(sqlQueryString);
            command.AddParameters("LastName", lastName is null ? DBNull.Value : lastName);

            IEnumerable<InstructorViewModel>? instructors = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToInstructor().DalToBllInstructor().BllToApiInstructor());

            return instructors is null ? NotFound("Doesn't teach with us (anymore)... :-(") : Ok(instructors.OrderBy(instructor => instructor.InstructorId));
        }


        // GET api/<InstructorsController>/fullName/bruce lee
        [HttpGet("fullName/{fullName:regex([[\\w-[[\\d]]]]+):length(1,510)}")]
        public ActionResult<IEnumerable<InstructorViewModel>> GetByFullName(string fullName)
        {
            Command command = new(@"
SELECT
  [InstructorId]
  ,[FirstName]
  ,[LastName]
FROM
  [Instructor]
WHERE
  CONCAT ([FirstName], ' ', [LastName]) 
LIKE CONCAT('%', TRIM(@FullName), '%')
  OR
  CONCAT ([LastName], ' ', [FirstName]) 
LIKE CONCAT('%', TRIM(@FullName), '%')
ORDER BY
  [InstructorId]
;
");
            command.AddParameters("FullName", fullName);
            IEnumerable<InstructorViewModel>? instructors = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToInstructor().DalToBllInstructor().BllToApiInstructor());

            return instructors is null ? NotFound("Doesn't teach with us (anymore)... :-(") : Ok(instructors.OrderBy(instructor => instructor.InstructorId));
        }

        /*
*/

        /*
        // GET api/<InstructorsController>/bruce lee
        [HttpGet("{firstname:length(1,255)}{lastname:length(1,255)?}")]
        public ActionResult<InstructorViewModel> Get(string firstname, string lastname)
        //public ActionResult<InstructorViewModel> Get([FromQuery] FullName fullName)
        {
            Command command = new(@"
SELECT
  [InstructorId]
  ,[FirstName]
  ,[LastName]
FROM
  [Instructor]
WHERE
  [FirstName] = TRIM(@FirstName)
  AND
  [LastName] = TRIM(@LastName)
ORDER BY
  [InstructorId]
;
");
            command.AddParameters("FirstName", firstname);
            //command.AddParameters("FirstName", fullName.FirstName);
            command.AddParameters("LastName", lastname);
            //command.AddParameters("LastName", fullName.LastName);
            IEnumerable<InstructorViewModel>? instructors = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToInstructor().DalToBllInstructor().BllToApiInstructor());

            return instructors is null ? NotFound("Doesn't teach with us (anymore)... >:/") : Ok(instructors.OrderBy(instructor=>instructor.InstructorId));
        }
        */

        // POST api/<InstructorsController>
        [HttpPost]
        public ActionResult Post([FromBody] InstructorCreationForm form)
        {
            if (!ModelState.IsValid || form.FirstName is null)
            {
                return BadRequest();
            }
            string query = form.LastName is null
                ? @"
SELECT
  [FirstName]
FROM
  [Instructor]
WHERE
  [FirstName] = TRIM(@FirstName)
AND
  [LastName] IS NULL
;
"
                : @"
SELECT
  [FirstName], [LastName]
FROM
  [Instructor]
WHERE
  [FirstName] = TRIM(@FirstName)
AND
  [LastName] = TRIM(@LastName)
;
";
            Command command = new(query);
            command.AddParameters("FirstName", form.FirstName);

            if (form.LastName is not null)
            {
                command.AddParameters("LastName", form.LastName);
            }

            InstructorViewModel? existingInstructor = new Connection(_connectionString).ExecuteReader(command, reader => new InstructorViewModel()
            {
                FirstName = (string)reader["FirstName"],
                LastName = form.LastName is null ? null : (string)reader["LastName"],
            }).SingleOrDefault();

            if (existingInstructor is null)
            {
                Command createCommand = new(@"
INSERT INTO [dbo].[Instructor]
  (
  [FirstName], [LastName]
  )
	OUTPUT
	inserted.InstructorId,
	inserted.FirstName,
	inserted.LastName
VALUES
  (
    TRIM(@FirstName), TRIM(@LastName)
  );
");
                createCommand.AddParameters("FirstName", form.FirstName);
                // TODO: check for null reference with form.LastName
                createCommand.AddParameters("LastName", form.LastName is null ? DBNull.Value : form.LastName);

                /*
                int rowsAffected = new Connection(_connectionString).ExecuteNonQuery(createCommand);

                return rowsAffected == 1 ? Ok() : BadRequest();
                */

                InstructorViewModel? newInstructor = null;

                using (SqlConnection connection = new())
                {
                    connection.ConnectionString = _connectionString;

                    using (SqlCommand manualCommand = connection.CreateCommand())
                    {
                        manualCommand.CommandText = @"
INSERT INTO [dbo].[Instructor]
  (
  [FirstName], [LastName]
  )
	OUTPUT
	inserted.InstructorId,
	inserted.FirstName,
	inserted.LastName
VALUES
  (
    TRIM(@FirstName), TRIM(@LastName)
  );
";
                        _ = manualCommand.Parameters.AddWithValue("FirstName", form.FirstName);
                        _ = manualCommand.Parameters.AddWithValue("LastName", form.LastName is null ? DBNull.Value : form.LastName);

                        connection.Open();
                        using (SqlDataReader reader = manualCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                newInstructor = new InstructorViewModel()
                                {
                                    InstructorId = (int)reader["InstructorId"],
                                    FirstName = (string)reader["FirstName"],
                                    LastName = reader["LastName"] as string,
                                };
                            }
                            reader.Close();
                        }
                        manualCommand.Dispose();
                    }
                    connection.Close();
                    connection.Dispose();
                }

                // TODO: bug here looking and not finding InstructorId
                // InstructorViewModel instructor = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToInstructor().DalToBllInstructor().BllToApiInstructor()).SingleOrDefault();

                return newInstructor is null ? BadRequest() : Ok(newInstructor);
                //return instructor is null ? BadRequest() : Ok(instructor);
            }
            else
            {
                return Conflict();
            }
        }

        // PUT api/<InstructorsController>/5
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] InstructorUpdateForm form)
        {
            if (!ModelState.IsValid || form.FirstName is null)
            {
                return BadRequest();
            }
            string query = form.LastName is null
                ? @"
SELECT
  [FirstName]
FROM
  [Instructor]
WHERE
  [FirstName] = TRIM(@FirstName)
AND
  [LastName] IS NULL
;
"
                : @"
SELECT
  [FirstName], [LastName]
FROM
  [Instructor]
WHERE
  [FirstName] = TRIM(@FirstName)
AND
  [LastName] = TRIM(@LastName)
;
";
            Command duplicateCommand = new(query);
            duplicateCommand.AddParameters("FirstName", form.FirstName);

            if (form.LastName is not null)
            {
                duplicateCommand.AddParameters("LastName", form.LastName);
            }

            InstructorViewModel? existingInstructor = new Connection(_connectionString).ExecuteReader(duplicateCommand, reader => new InstructorViewModel()
            {
                FirstName = (string)reader["FirstName"],
                LastName = form.LastName is null ? null : (string)reader["LastName"],
            }).SingleOrDefault();

            if (existingInstructor is null)
            {
                Command command = new(@"
	UPDATE [dbo].[Instructor]
	SET
		[FirstName] = TRIM(@FirstName),
		[LastName] = TRIM(@LastName)
	WHERE [InstructorId] = @Id
;
");
                command.AddParameters("Id", id);
                command.AddParameters("FirstName", form.FirstName);
                command.AddParameters("LastName", form.LastName);

                int rowsAffected = new Connection(_connectionString).ExecuteNonQuery(command);

                return rowsAffected == 1 ? Ok() : BadRequest();
            }
            else
            {
                return Conflict();
            }
        }

        // DELETE api/<InstructorsController>/5
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            Command command = new(@"
DELETE 
FROM
  [Instructor]
WHERE
  [InstructorId] = @Id
;
");
            command.AddParameters("Id", id);

            int rowsAffected = new Connection(_connectionString).ExecuteNonQuery(command);

            return rowsAffected == 1 ? Ok() : BadRequest();
        }
    }
}
