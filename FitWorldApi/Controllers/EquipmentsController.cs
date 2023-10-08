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
    public class EquipmentsController : ControllerBase
    {
        private readonly string _connectionString;

        public EquipmentsController(IOptions<MyOptions> optionsAccessor, IConfiguration configuration)
        {
            _connectionString = optionsAccessor.Value.ConnectionString;
        }

        // TODO: wrap every http method in a try-catch

        // GET: api/<EquipmentsController>
        [HttpGet]
        public ActionResult<IEnumerable<EquipmentViewModel>> Get()
        {
            Command command = new(@"
SELECT 
[EquipmentId], 
[Name], 
[Price] 
FROM 
[Equipment]
ORDER BY
  [EquipmentId]
;
");

            IEnumerable<EquipmentViewModel>? equipments = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToEquipment().DalToBllEquipment().BllToApiEquipment());

            return equipments is null ? BadRequest() : Ok(equipments.OrderBy(equipment => equipment.EquipmentId));
        }

        // GET api/<EquipmentsController>/5
        [HttpGet("{id:int:regex(^\\d+$)}")]
        public ActionResult<EquipmentViewModel> Get(int id)
        {
            Command command = new(@"
SELECT
  [EquipmentId]
  ,[Name]
  ,[Price]
FROM
  [Equipment]
WHERE
  [EquipmentId] = @Id;
");
            command.AddParameters("Id", id);
            EquipmentViewModel? equipment = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToEquipment().DalToBllEquipment().BllToApiEquipment()).SingleOrDefault();

            return equipment is null ? NotFound("We don't sell that (anymore)...") : Ok(equipment);
        }

        // GET api/<EquipmentsController>/price/5
        //[HttpGet("price/{price:decimal:regex(^\\d+\\.\\d+$)}")]
        [HttpGet("name_price/{name},{price:decimal}")]
        public ActionResult<EquipmentViewModel> GetEquimentByNamePrice(string name, decimal price)
        {
            Command command = new(@"
SELECT
  [EquipmentId]
  ,[Name]
  ,[Price]
FROM
  [Equipment]
WHERE
  [Name] = @name
AND
  [Price] = @price
;
");
            command.AddParameters("name", name);
            command.AddParameters("price", Math.Round(price, 4));
            EquipmentViewModel? equipment = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToEquipment().DalToBllEquipment().BllToApiEquipment()).SingleOrDefault();

            return equipment is null ? NotFound("We don't sell that (anymore)... :$") : Ok(equipment);
        }

        // GET api/<EquipmentsController>/price/5
        //[HttpGet("price/{price:decimal:regex(^\\d+\\.\\d+$)}")]
        [HttpGet("price/{price:decimal}")]
        public ActionResult<EquipmentViewModel> GetEquimentsByPrice(decimal price)
        {
            Command command = new(@"
SELECT
  [EquipmentId]
  ,[Name]
  ,[Price]
FROM
  [Equipment]
WHERE
  [Price] = @price
ORDER BY
  [EquipmentId]
;
");
            command.AddParameters("price", Math.Round(price, 4));
            IEnumerable<EquipmentViewModel>? equipments = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToEquipment().DalToBllEquipment().BllToApiEquipment());

            return equipments is null ? NotFound("We don't sell that (anymore)... :$") : Ok(equipments.OrderBy(equipment => equipment.EquipmentId));
        }

        // GET api/<EquipmentsController>/price/5
        //[HttpGet("price/{price:decimal:regex(^\\d+\\.\\d+$)}")]
        [HttpGet("prices/{price:decimal}")]
        public ActionResult<EquipmentViewModel> FindEquimentsByPrice(decimal price)
        {
            Command command = new(@"
SELECT
  [EquipmentId]
  ,[Name]
  ,[Price]
FROM
  [Equipment]
WHERE
  [Price] 
LIKE CONCAT('%', CONVERT(VARCHAR(23), @price), '%')
ORDER BY
  [EquipmentId]
;
");
            command.AddParameters("price", Math.Round(price, 4));
            IEnumerable<EquipmentViewModel>? equipments = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToEquipment().DalToBllEquipment().BllToApiEquipment());

            return equipments is null ? NotFound("We don't sell that (anymore)... :$") : Ok(equipments.OrderBy(equipment => equipment.EquipmentId));
        }

        // GET api/<EquipmentsController>/name/uniform
        //[HttpGet("name/{name:regex([[\\w-[[\\d]]]]+):length(1,42)}")]
        [HttpGet("name/{name}")]
        public ActionResult<EquipmentViewModel> GetEquipmentsByName(string name)
        {
            Command command = new(@"
SELECT
  [EquipmentId]
  ,[Name]
  ,[Price]
FROM
  [Equipment]
WHERE
  [Name] = @name
ORDER BY
  [EquipmentId]
;
");
            command.AddParameters("name", name);
            IEnumerable<EquipmentViewModel>? equipments = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToEquipment().DalToBllEquipment().BllToApiEquipment());

            return equipments is null ? NotFound("We don't sell that (anymore)... :-(") : Ok(equipments.OrderBy(equipment => equipment.EquipmentId));
        }

        // GET api/<EquipmentsController>/name/uniform
        //[HttpGet("names/{name:regex([[\\w-[[\\d]]]]+):length(1,42)}")]
        [HttpGet("names/{name}")]
        public ActionResult<EquipmentViewModel> FindEquipmentsByName(string name)
        {
            Command command = new(@"
SELECT
  [EquipmentId]
  ,[Name]
  ,[Price]
FROM
  [Equipment]
WHERE
  [Name] 
LIKE CONCAT('%', TRIM(@name), '%')
ORDER BY
  [EquipmentId]
;
");
            command.AddParameters("name", name);
            IEnumerable<EquipmentViewModel>? equipments = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToEquipment().DalToBllEquipment().BllToApiEquipment());

            return equipments is null ? NotFound("We don't sell that (anymore)... :-(") : Ok(equipments.OrderBy(equipment => equipment.EquipmentId));
        }

        // POST api/<EquipmentsController>
        [HttpPost]
        public ActionResult Post([FromBody] EquipmentCreationForm form)
        {
            if (!ModelState.IsValid || form.Name is null)
            {
                return BadRequest();
            }

            Command command = new(@"
SELECT
  [Name]
  ,[Price]
FROM
  [Equipment]
WHERE
  [Name] = TRIM(@Name)
  AND
  [Price] = @Price
ORDER BY
  [EquipmentId]
;
");

            command.AddParameters("Name", form.Name);
            command.AddParameters("Price", Math.Round(form.Price, 4));

            EquipmentViewModel? existingEquipment = new Connection(_connectionString).ExecuteReader(command, reader => new EquipmentViewModel()
            {
                Name = (string)reader["Name"],
                Price = (decimal)reader["Price"],
            }).SingleOrDefault();

            if (existingEquipment is null)
            {
                Command createCommand = new(@"
INSERT INTO [dbo].[Equipment]
  (
  [Name], [Price]
  )
	OUTPUT
	inserted.EquipmentId,
	inserted.Name,
	inserted.Price
VALUES
  (
    TRIM(@Name) ,@Price
  );
");
                createCommand.AddParameters("Name", form.Name);
                createCommand.AddParameters("Price", Math.Round(form.Price, 4));

                /*
                int rowsAffected = new Connection(_connectionString).ExecuteNonQuery(createCommand);

                return rowsAffected == 1 ? Ok() : BadRequest();
                */

                EquipmentViewModel? newEquipment = null;

                using (SqlConnection connection = new())
                {
                    connection.ConnectionString = _connectionString;

                    using (SqlCommand manualCommand = connection.CreateCommand())
                    {
                        manualCommand.CommandText = @"
INSERT INTO [dbo].[Equipment]
  (
  [Name], [Price]
  )
	OUTPUT
	inserted.EquipmentId,
	inserted.Name,
	inserted.Price
VALUES
  (
    TRIM(@Name) ,@Price
  );
";
                        _ = manualCommand.Parameters.AddWithValue("Name", form.Name);
                        _ = manualCommand.Parameters.AddWithValue("Price", Math.Round(form.Price, 4));

                        connection.Open();
                        using (SqlDataReader reader = manualCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                newEquipment = new EquipmentViewModel()
                                {
                                    EquipmentId = (int)reader["EquipmentId"],
                                    Name = (string)reader["Name"],
                                    Price = (decimal)reader["Price"],
                                };
                            }
                            reader.Close();
                        }
                        manualCommand.Dispose();
                    }
                    connection.Close();
                    connection.Dispose();
                }
                /*
                EquipmentViewModel? equipment = new Connection(_connectionString).ExecuteReader(command, reader =>
                {
                    object[] values = new object[20];
                    _ = reader.GetValues(values);

                    Console.WriteLine(JsonSerializer.Serialize(values));

                    EquipmentViewModel newEquipment = new()
                    {
                        // EquipmentId = (int)reader["EquipmentId"],
                        Name = (string)reader["Name"],
                        Price = (decimal)reader["Price"],
                    };

                    return newEquipment;
                }).SingleOrDefault();
                */
                /*
                EquipmentViewModel equipment = new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToEquipment().DalToBllEquipment().BllToApiEquipment()).SingleOrDefault();
                */
                //Console.WriteLine(JsonSerializer.Serialize(new Connection(_connectionString).ExecuteReader(command, reader => reader.DbToEquipment())));
                // Console.WriteLine(JsonSerializer.Serialize(equipment));

                return newEquipment is null ? BadRequest() : Ok(newEquipment);
                // return equipment is null ? BadRequest() : Ok(equipment);
            }
            else
            {
                return Conflict();
            }
        }

        // PUT api/<EquipmentsController>/5
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] EquipmentUpdateForm form)
        {
            if (!ModelState.IsValid || form.Name is null)
            {
                return BadRequest();
            }

            Command duplicateCommand = new(@"
SELECT
  [Name]
  ,[Price]
FROM
  [Equipment]
WHERE
  [Name] = TRIM(@Name)
  AND
  [Price] = @Price;
");

            duplicateCommand.AddParameters("Name", form.Name);
            duplicateCommand.AddParameters("Price", Math.Round(form.Price, 4));

            EquipmentViewModel? existingEquipment = new Connection(_connectionString).ExecuteReader(duplicateCommand, reader => new EquipmentViewModel()
            {
                Name = (string)reader["Name"],
                Price = (decimal)reader["Price"],
            }).SingleOrDefault();

            if (existingEquipment is null)
            {
                Command command = new(@"
	UPDATE [dbo].[Equipment]
	SET
		[Name] = TRIM(@Name),
		[Price] = @Price
	WHERE [EquipmentId] = @Id
;
");
                command.AddParameters("Id", id);
                command.AddParameters("Name", form.Name);
                command.AddParameters("Price", Math.Round(form.Price, 4));

                int rowsAffected = new Connection(_connectionString).ExecuteNonQuery(command);

                return rowsAffected == 1 ? Ok() : BadRequest();
            }
            else
            {
                return Conflict();
            }
        }

        // DELETE api/<EquipmentsController>/5
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            Command command = new(@"
DELETE 
FROM
  [Equipment]
WHERE
  [EquipmentId] = @Id;
");
            command.AddParameters("Id", id);

            int rowsAffected = new Connection(_connectionString).ExecuteNonQuery(command);

            return rowsAffected == 1 ? Ok() : BadRequest();
        }
    }
}
