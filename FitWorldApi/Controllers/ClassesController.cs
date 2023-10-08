using DbConnectionTools;

using FitWorld.BLL.Mappers;
using FitWorld.Dal.Mappers;

using FitWorldApi.Mappers;
using FitWorldApi.Models;
using FitWorldApi.Models.Forms;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FitWorldApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly string _connectionString;

        public ClassesController(IOptions<MyOptions> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        // GET: api/<ClassesController>
        [HttpGet]
        public ActionResult<IEnumerable<ClassViewModel>> Get()
        {
            Command command = new(@"
SELECT
  [ClassId]
  ,[MartialArtId]
  ,[InstructorId]
  ,[DateTime]
  ,[PricePerHour]
FROM
  [Class]
ORDER BY
  [ClassId]
;
");
            IEnumerable<ClassViewModel>? classes = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToClass().DalToBllClass().BllToApiClass());

            return classes is null ? BadRequest() : Ok(classes.OrderBy(@class => @class.ClassId));
        }

        // GET api/<ClassesController>/5
        [HttpGet("{id:int:regex(^\\d+$)}")]
        public ActionResult<ClassViewModel> Get(int id)
        {
            Command command = new(@"
SELECT
  [ClassId]
  ,[MartialArtId]
  ,[InstructorId]
  ,[DateTime]
  ,[PricePerHour]
FROM
  [Class]
WHERE
  [ClassId] = @Id
;
");
            command.AddParameters("Id", id);
            ClassViewModel? @class = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToClass().DalToBllClass().BllToApiClass()).SingleOrDefault();

            return @class is null ? NotFound("That is not something we offer (anymore)...") : Ok(@class);
        }

        // TODO: make the 4 search functions work: martialArt, instructor, price & dateTime

        // GET api/<ClassesController>/martialArt/5
        [HttpGet("martialArt/{id:int:regex(^\\d+$)}")]
        public ActionResult<ClassViewModel> GetByMartialArt(int id)
        {
            Command command = new(@"
SELECT
  [ClassId]
  ,[MartialArtId]
  ,[InstructorId]
  ,[DateTime]
  ,[PricePerHour]
FROM
  [Class]
WHERE
  [MartialArtId] = @Id
ORDER BY
  [ClassId]
;
");
            command.AddParameters("Id", id);
            IEnumerable<ClassViewModel>? @classes = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToClass().DalToBllClass().BllToApiClass());

            return @classes is null ? NotFound("That is not something we offer (anymore)...") : Ok(@classes.OrderBy(@class => @class.ClassId));
        }

        // GET api/<ClassesController>/instructor/5
        [HttpGet("instructor/{id:int:regex(^\\d+$)}")]
        public ActionResult<ClassViewModel> GetByInstructor(int id)
        {
            Command command = new(@"
SELECT
  [ClassId]
  ,[MartialArtId]
  ,[InstructorId]
  ,[DateTime]
  ,[PricePerHour]
FROM
  [Class]
WHERE
  [InstructorId] = @Id
ORDER BY
  [ClassId]
;
");
            command.AddParameters("Id", id);
            IEnumerable<ClassViewModel>? @classes = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToClass().DalToBllClass().BllToApiClass());

            return @classes is null ? NotFound("That is not something we offer (anymore)...") : Ok(@classes.OrderBy(@class => @class.ClassId));
        }

        // GET api/<ClassesController>/dateTime/2023-01-18 12:25
        [HttpGet("dateTime/{dateTime:datetime}")]
        public ActionResult<ClassViewModel> GetByDateTime(DateTime dateTime)
        {
            Command command = new(@"
SELECT
  [ClassId]
  ,[MartialArtId]
  ,[InstructorId]
  ,[DateTime]
  ,[PricePerHour]
FROM
  [Class]
WHERE
  [DateTime] = @DateTime
ORDER BY
  [ClassId]
;
");
            command.AddParameters("DateTime", dateTime);
            IEnumerable<ClassViewModel>? @classes = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToClass().DalToBllClass().BllToApiClass());

            return @classes is null ? NotFound("That is not something we offer (anymore)...") : Ok(@classes.OrderBy(@class => @class.ClassId));
        }

        // GET api/<ClassesController>/price/36.7987
        [HttpGet("price/{price:decimal}")]
        public ActionResult<ClassViewModel> GetByPrice(decimal price)
        {
            Command command = new(@"
SELECT
  [ClassId]
  ,[MartialArtId]
  ,[InstructorId]
  ,[DateTime]
  ,[PricePerHour]
FROM
  [Class]
WHERE
  [PricePerHour] = @PricePerHour
ORDER BY
  [ClassId]
;
");
            command.AddParameters("PricePerHour", Math.Round(price, 4));
            IEnumerable<ClassViewModel>? @classes = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToClass().DalToBllClass().BllToApiClass());

            return @classes is null ? NotFound("That is not something we offer (anymore)...") : Ok(@classes.OrderBy(@class => @class.ClassId));
        }

        // POST api/<ClassesController>
        [HttpPost]
        public ActionResult Post([FromBody] ClassCreationForm form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Command command = new(@"
SELECT
  [InstructorId]
  ,[DateTime]
FROM
  [Class]
WHERE
  [InstructorId] = @Who
AND
  [DateTime] = @When
;
");
            command.AddParameters("Who", form.InstructorId);
            command.AddParameters("When", form.DateTime);

            ClassViewModel? existingClass = new Connection(_connectionString).ExecuteReader(command, reader => new ClassViewModel()
            {
                InstructorId = (int)reader["InstructorId"],
                DateTime = (DateTime)reader["DateTime"],
            }).SingleOrDefault();

            if (existingClass is null)
            {
                Command createCommand = new(@"
INSERT INTO [dbo].[Class]
  (  
  [MartialArtId]
  ,[InstructorId]
  ,[DateTime]
  ,[PricePerHour]
  )
	OUTPUT
	inserted.ClassId,
	inserted.MartialArtId,
	inserted.InstructorId,
	inserted.DateTime,
	inserted.PricePerHour
VALUES
  (
  @MartialArtId
  ,@InstructorId
  ,@DateTime
  ,@PricePerHour
  )
;
");
                createCommand.AddParameters("MartialArtId", form.MartialArtId);
                createCommand.AddParameters("InstructorId", form.InstructorId);
                createCommand.AddParameters("DateTime", form.DateTime);
                createCommand.AddParameters("PricePerHour", Math.Round(/*0.0m*/ form.PricePerHour, 4));

                ClassViewModel? newClass = null;

                using (SqlConnection connection = new())
                {
                    connection.ConnectionString = _connectionString;

                    using (SqlCommand manualCommand = connection.CreateCommand())
                    {
                        manualCommand.CommandText = @"
INSERT INTO [dbo].[Class]
  (  
  [MartialArtId]
  ,[InstructorId]
  ,[DateTime]
  ,[PricePerHour]
  )
	OUTPUT
	inserted.ClassId,
	inserted.MartialArtId,
	inserted.InstructorId,
	inserted.DateTime,
	inserted.PricePerHour
VALUES
  (
  @MartialArtId
  ,@InstructorId
  ,@DateTime
  ,@PricePerHour
  )
;
";
                        _ = manualCommand.Parameters.AddWithValue("MartialArtId", form.MartialArtId);
                        _ = manualCommand.Parameters.AddWithValue("InstructorId", form.InstructorId);
                        _ = manualCommand.Parameters.AddWithValue("DateTime", form.DateTime);
                        _ = manualCommand.Parameters.AddWithValue("PricePerHour", Math.Round(form.PricePerHour, 4));

                        connection.Open();
                        using (SqlDataReader reader = manualCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                newClass = new ClassViewModel()
                                {
                                    ClassId = (int)reader["ClassId"],
                                    MartialArtId = (int)reader["MartialArtId"],
                                    InstructorId = (int)reader["InstructorId"],
                                    DateTime = (DateTime)reader["DateTime"],
                                    PricePerHour = (decimal)reader["PricePerHour"],
                                };
                            }
                            reader.Close();
                        }
                        manualCommand.Dispose();
                    }
                    connection.Close();
                    connection.Dispose();
                }

                // ClassViewModel @class = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToClass().DalToBllClass().BllToApiClass()).SingleOrDefault();

                return newClass is null ? BadRequest() : Ok(newClass);
                //return @class is null ? BadRequest() : Ok(@class);
            }
            else
            {
                return Conflict();
            }
        }

        // PUT api/<ClassesController>/5
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] ClassUpdateForm form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Command duplicateCommand = new(@"
SELECT
  [InstructorId]
  ,[DateTime]
FROM
  [Class]
WHERE
  [InstructorId] = @Who
AND
  [DateTime] = @When
;
");
            duplicateCommand.AddParameters("Who", form.InstructorId);
            duplicateCommand.AddParameters("When", form.DateTime);

            ClassViewModel? existingClass = new Connection(_connectionString).ExecuteReader(duplicateCommand, reader => new ClassViewModel()
            {
                InstructorId = (int)reader["InstructorId"],
                DateTime = (DateTime)reader["DateTime"],
            }).SingleOrDefault();

            if (existingClass is null)
            {
                Command command = new(@"
UPDATE [Class]
SET    
  [MartialArtId] = @MartialArtId
  ,[InstructorId] = @InstructorId
  ,[DateTime] = @DateTime
  ,[PricePerHour] = @PricePerHour
WHERE [ClassId] = @id
;
");
                command.AddParameters("id", id);
                command.AddParameters("MartialArtId", form.MartialArtId);
                command.AddParameters("InstructorId", form.InstructorId);
                command.AddParameters("DateTime", form.DateTime);
                command.AddParameters("PricePerHour", Math.Round(/*0.0m*/form.PricePerHour, 4));

                int rowsAffected = new Connection(_connectionString).ExecuteNonQuery(command);

                return rowsAffected == 1 ? Ok() : BadRequest();
            }
            else
            {
                return Conflict();
            }
        }

        // DELETE api/<ClassesController>/5
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            Command command = new(@"
DELETE
FROM
  [Class]
WHERE
  [ClassId] = @Id
;
");
            command.AddParameters("Id", id);
            int rowsAffected = new Connection(_connectionString).ExecuteNonQuery(command);

            return rowsAffected == 1 ? Ok() : BadRequest();
        }
    }
}
