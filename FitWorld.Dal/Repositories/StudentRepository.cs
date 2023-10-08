using DbConnectionTools;

using FitWorld.Dal.Data;
using FitWorld.Dal.Interfaces;
using FitWorld.Dal.Mappers;

namespace FitWorld.Dal.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly Connection _connection;

        public StudentRepository(Connection connection)
        {
            _connection = connection;
        }

        //public StudentData LogInStudent(string email, string password)
        public StudentData? LogInStudent(string email, string password)
        {
            Command command = new("spStudentLogIn", true);

            command.AddParameters("Email", email);
            command.AddParameters("Password", password);

            return _connection.ExecuteReader(command, reader => reader.DbToStudent()).SingleOrDefault();
        }

        //public void RegisterStudent(StudentData data)
        public StudentData? RegisterStudent(StudentData data)
        {
            Command command = new("spStudentRegister", true);

            command.AddParameters("Email", data.Email);
            command.AddParameters("Password", data.Password);

            //_ = _connection.ExecuteNonQuery(command);
            return _connection.ExecuteReader(command, reader => reader.DbToStudent()).SingleOrDefault();
        }
    }
}
