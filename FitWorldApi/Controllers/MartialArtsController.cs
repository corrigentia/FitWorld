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
    public class MartialArtsController : ControllerBase
    {
        private readonly string _connectionString;

        public MartialArtsController(IOptions<MyOptions> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        // GET: api/<MartialArtsController>
        [HttpGet]
        public ActionResult<IEnumerable<MartialArtViewModel>> Get()
        {
            Command command = new(@"
SELECT 
[MartialArtId], 
[Name] 
FROM 
[MartialArt]
ORDER BY
  [MartialArtId]
;
");

            IEnumerable<MartialArtViewModel>? martialArts = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToMartialArt().DalToBllMartialArt().BllToApiMartialArt());

            return martialArts is null ? BadRequest() : Ok(martialArts.OrderBy(martialArt => martialArt.MartialArtId));
        }

        // GET api/<MartialArtsController>/5
        [HttpGet("{id:int:regex(^\\d+$)}")]
        public ActionResult<MartialArtViewModel> Get(int id)
        {
            Command command = new(@"
SELECT
  [MartialArtId]
  ,[Name]
FROM
  [MartialArt]
WHERE
  [MartialArtId] = @Id
;
");
            command.AddParameters("Id", id);
            MartialArtViewModel? martialArt = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToMartialArt().DalToBllMartialArt().BllToApiMartialArt()).SingleOrDefault();

            return martialArt is null ? NotFound("We don't teach that (anymore)...") : Ok(martialArt);
        }

        // GET api/<MartialArtsController>/name/karate
        [HttpGet("name/{name:regex([[\\w-[[\\d]]]]+):length(1,42)}")]
        public ActionResult<MartialArtViewModel> GetByName(string name)
        {
            Command command = new(@"
SELECT
  [MartialArtId]
  ,[Name]
FROM
  [MartialArt]
WHERE
  [Name] LIKE CONCAT('%', TRIM(@name), '%')
ORDER BY
  [MartialArtId]
;
"
);
            command.AddParameters("name", name);
            IEnumerable<MartialArtViewModel>? martialArts = new Connection(_connectionString).ExecuteReader(command, reader => reader
            .DbToMartialArt()
            .DalToBllMartialArt()
            .BllToApiMartialArt()
            );

            return martialArts is null ? NotFound("We don't teach that (anymore)... :-(") : Ok(martialArts.OrderBy(martialArt => martialArt.MartialArtId));
        }
        // TODO: OUTPUT all inserted.*Id etc.
        // POST api/<MartialArtsController>
        [HttpPost]
        public ActionResult Post([FromBody] MartialArtCreationForm form)
        {
            if (!ModelState.IsValid || form.Name is null)
            {
                return BadRequest();
            }

            Command command = new(@"
SELECT
  [Name]
FROM
  [MartialArt]
WHERE
  [Name] = TRIM(@Name)
");
            command.AddParameters("Name", form.Name);

            MartialArtViewModel? existingMartialArt = new Connection(_connectionString).ExecuteReader(command, reader => new MartialArtViewModel()
            {
                Name = (string)reader["Name"],
            }).SingleOrDefault();

            if (existingMartialArt is null)
            {
                Command createCommand = new(@"
INSERT INTO [dbo].[MartialArt]
  (
  [Name]
  )
	OUTPUT
	inserted.MartialArtId,
	inserted.Name
VALUES
  (
    TRIM(@Name)
  )
;
");
                createCommand.AddParameters("Name", form.Name);

                /*
                int rowsAffected = new Connection(_connectionString).ExecuteNonQuery(createCommand);

                return rowsAffected == 1 ? Ok() : BadRequest();
                */

                MartialArtViewModel? newMartialArt = null;

                using (SqlConnection connection = new())
                {
                    connection.ConnectionString = _connectionString;

                    using (SqlCommand manualCommand = connection.CreateCommand())
                    {
                        manualCommand.CommandText = @"
INSERT INTO [dbo].[MartialArt]
  (
  [Name]
  )
	OUTPUT
	inserted.MartialArtId,
	inserted.Name
VALUES
  (
    TRIM(@Name)
  )
;
";
                        _ = manualCommand.Parameters.AddWithValue("Name", form.Name);

                        connection.Open();
                        using (SqlDataReader reader = manualCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                newMartialArt = new MartialArtViewModel()
                                {
                                    MartialArtId = (int)reader["MartialArtId"],
                                    Name = (string)reader["Name"],
                                };
                            }
                            reader.Close();
                        }
                        manualCommand.Dispose();
                    }
                    connection.Close();
                    connection.Dispose();
                }

                // MartialArtViewModel martialArt = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToMartialArt().DalToBllMartialArt().BllToApiMartialArt()).SingleOrDefault();

                return newMartialArt is null ? BadRequest() : Ok(newMartialArt);
                //return martialArt is null ? BadRequest() : Ok(martialArt);
            }
            else
            {
                return Conflict();
            }
        }

        // PUT api/<MartialArtsController>/5
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] MartialArtUpdateForm form)
        {
            if (!ModelState.IsValid || form.Name is null)
            {
                return BadRequest();
            }

            Command duplicateCommand = new(@"
SELECT
  [Name]
FROM
  [MartialArt]
WHERE
  [Name] = TRIM(@Name)
");
            duplicateCommand.AddParameters("Name", form.Name);

            MartialArtViewModel? existingMartialArt = new Connection(_connectionString).ExecuteReader(duplicateCommand, reader => new MartialArtViewModel()
            {
                Name = (string)reader["Name"],
            }).SingleOrDefault();

            if (existingMartialArt is null)
            {
                Command command = new(@"
	UPDATE [dbo].[MartialArt]
	SET
		[Name] = TRIM(@Name)
	WHERE [MartialArtId] = @Id
;
");
                command.AddParameters("Id", id);
                command.AddParameters("Name", form.Name);

                int rowsAffected = new Connection(_connectionString).ExecuteNonQuery(command);

                return rowsAffected == 1 ? Ok() : BadRequest();
            }
            else
            {
                return Conflict();
            }
        }

        // DELETE api/<MartialArtsController>/5
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            Command command = new(@"
DELETE 
FROM
  [MartialArt]
WHERE
  [MartialArtId] = @Id
;
");
            command.AddParameters("Id", id);

            int rowsAffected = new Connection(_connectionString).ExecuteNonQuery(command);

            return rowsAffected == 1 ? Ok() : BadRequest();
        }
    }
}
