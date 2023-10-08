using DbConnectionTools;

using FitWorld.BLL.Interfaces;
using FitWorld.BLL.Models;

using FitWorldApi.Infrastructure;
using FitWorldApi.Models;
using FitWorldApi.Models.Forms;

using Microsoft.AspNetCore.Mvc;

using System.Data.SqlClient;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FitWorldApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly TokenManager _tokenManager;
        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FitWorld;Integrated Security=True;";

        public AuthController(IStudentService studentService, TokenManager tokenManager)
        {
            _studentService = studentService;
            _tokenManager = tokenManager;
        }

        // POST api/<AuthController>/Register
        [HttpPost(nameof(Register))]
        //public ActionResult Register([FromForm] StudentRegisterForm form)
        public ActionResult Register([FromBody] StudentRegisterForm form)
        {

            try
            {

                if (!ModelState.IsValid || form.Email is null || form.Password is null)
                {
                    return BadRequest();
                }

                Command command = new(@"SELECT [Email] FROM [Student] WHERE Email = @Email;", false);

                command.AddParameters("Email", form.Email);
                //command.AddParameters("Password", form.Password);

                StudentViewModel? existingStudent = new Connection(_connectionString).ExecuteReader(command, reader =>
                new StudentViewModel()
                {
                    // StudentId = (int)reader["StudentId"],
                    Email = (string)reader["Email"],
                }).SingleOrDefault();

                if (existingStudent is null)
                {
                    //_ = _studentService.RegisterStudent(form.ApiToBllStudent());
                    //return Ok(_studentService.RegisterStudent(form.ApiToBllStudent()).BllToApiStudent());

                    //return Redirect($"students/email/{form.Email}");

                    StudentViewModel theNewStudent = new();

                    using (SqlConnection connection = new(_connectionString))
                    {
                        using (SqlCommand manualCreate = connection.CreateCommand())
                        {
                            //manualCreate.CommandText = "spStudentRegister";
                            manualCreate.CommandText = @"
	DECLARE @PasswordHash BINARY(64), @SecurityStamp UNIQUEIDENTIFIER;

	SET @SecurityStamp = NEWID()
	SET @PasswordHash = dbo.fHasher(TRIM(@Password), @SecurityStamp)

	INSERT INTO [Student]
		(Email, [PasswordHash], [SecurityStamp])
	OUTPUT
	inserted.StudentId,
	inserted.Email
	VALUES
		(TRIM(@Email) ,@PasswordHash ,@SecurityStamp)
;
";
                            //manualCreate.CommandType = System.Data.CommandType.StoredProcedure;
                            _ = manualCreate.Parameters.AddWithValue("Email", form.Email);
                            _ = manualCreate.Parameters.AddWithValue("Password", form.Password);
                            connection.Open();
                            using (SqlDataReader reader = manualCreate.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    theNewStudent = new()
                                    {
                                        StudentId = (int)reader["StudentId"],
                                        Email = (string)reader["Email"],
                                    };
                                }
                                _ = reader.CloseAsync();
                                _ = reader.DisposeAsync();
                            }
                            _ = manualCreate.DisposeAsync();
                        }
                        _ = connection.CloseAsync();
                        _ = connection.DisposeAsync();
                    }

                    Command retrieveNew = new(@"
SELECT
  [StudentId]
  ,[Email]
FROM
  [Student]
WHERE [Email] = @Email;");
                    retrieveNew.AddParameters("Email", form.Email);

                    // TODO: look into why the stored procedure does NOT return the StudentId
                    StudentViewModel? newStudent = new Connection(_connectionString).ExecuteReader(command, reader =>
                    // reader.DbToStudent().DalToBllStudent().BllToApiStudent()

                    {
                        /*
                        Console.WriteLine(string.Format("{0}", reader[0]));
                        while (reader.Read())
                        {
                            Console.WriteLine(string.Format("{0}", reader[0]));
                        }
                        */

                        object[] values = new object[20];
                        _ = reader.GetValues(values);

                        Console.WriteLine(JsonSerializer.Serialize(values));

                        StudentViewModel newStudent = new()
                        {
                            //StudentId = (int)reader["StudentId"],
                            Email = (string)reader["Email"],
                        };

                        return newStudent;
                    }

                    ).SingleOrDefault();

                    //return newStudent is null ? BadRequest() : Ok(newStudent);
                    return theNewStudent is null ? BadRequest() : Ok(theNewStudent);
                }
                else
                {
                    return Conflict();
                }

            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
                //throw;
            }

        }

        // POST api/<AuthController>/LogIn
        [HttpPost(nameof(LogIn))]
        public ActionResult<StudentWithToken> LogIn([FromBody] StudentLogInForm form)
        {

            try
            {

                if (!ModelState.IsValid || form.Email is null || form.Password is null)
                {
                    return BadRequest(ModelState);
                }

                Command command = new("SELECT [Email] FROM [Student] WHERE Email = @Email;", false);

                command.AddParameters("Email", form.Email);
                //command.AddParameters("Password", form.Password);

                StudentViewModel? existingStudent = new Connection(_connectionString).ExecuteReader(command, reader =>
                new StudentViewModel()
                {
                    // StudentId = (int)reader["StudentId"],
                    Email = (string)reader["Email"],
                }).SingleOrDefault();

                if (existingStudent is null)
                {
                    // _studentService.RegisterStudent(form.ApiToBll());
                    // return Ok();
                    return NotFound("We don't have that student (anymore)...");
                }

                StudentModel currentStudent = _studentService.LogInStudent(form.Email, form.Password);

                if (currentStudent is null)
                {
                    return Ok("Wrong password");
                }

                _ = currentStudent.Email is null ? string.Empty : currentStudent.Email;

                StudentWithToken studentWithToken = new()
                {
                    StudentId = currentStudent.StudentId,
                    Email = currentStudent.Email,
                    Token = _tokenManager.GenerateJWT(currentStudent),
                };

                return Ok(studentWithToken);

            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
                // throw;
            }

        }
    }
}
