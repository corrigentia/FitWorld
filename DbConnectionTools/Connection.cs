using System.Data;
using System.Data.SqlClient;

namespace DbConnectionTools
{
    public sealed class Connection
    {
        private readonly string _connectionString;

        public Connection(string connectionString)
        {
            _connectionString = connectionString;
        }
        private SqlConnection CreateConnection()
        {
            SqlConnection connection = new()
            {
                ConnectionString = _connectionString
            };
            return connection;
        }
        private SqlCommand CreateCommand(Command command, SqlConnection connection)
        {
            SqlCommand sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = command.Query;

            if (command.IsStoredProcedure)
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
            }

            foreach (KeyValuePair<string, object> keyValuePair in command.Parameters)
            {
                SqlParameter parameter = new()
                {
                    ParameterName = keyValuePair.Key,
                    Value = keyValuePair.Value
                };

                _ = sqlCommand.Parameters.Add(parameter);
            }
            return sqlCommand;
        }
        public object? ExecuteScalar(Command command)
        {
            using SqlConnection connection = CreateConnection();
            using SqlCommand sqlCommand = CreateCommand(command, connection);
            connection.Open();
            object result = sqlCommand.ExecuteScalar();
            sqlCommand.Dispose();
            connection.Close();
            connection.Dispose();
            return result is DBNull ? null : result;
        }
        public IEnumerable<TResult> ExecuteReader<TResult>(Command command, Func<IDataReader, TResult> selector)
        {
            using SqlConnection connection = CreateConnection();
            using SqlCommand sqlCommand = CreateCommand(command, connection);
            connection.Open();
            using SqlDataReader dataReader = sqlCommand.ExecuteReader();
            while (dataReader.Read())
            {
                yield return selector(dataReader);
            }
            // dataReader.Close();
            // sqlCommand.Dispose();
            // connection.Close();
            // connection.Dispose();
        }
        public int ExecuteNonQuery(Command command)
        {
            using SqlConnection connection = CreateConnection();
            using SqlCommand sqlCommand = CreateCommand(command, connection);
            connection.Open();
            return sqlCommand.ExecuteNonQuery();
        }
    }
}
