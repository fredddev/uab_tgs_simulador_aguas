using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace GestionAguaPeriurbanos.Data
{
    public class MySqlConnectionFactory
    {
        private readonly string _connectionString;

        public MySqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        // Returns a closed connection; caller is responsible for opening/disposing.
        public IDbConnection CreateConnection() => new MySqlConnection(_connectionString);

        // Convenience: returns an open connection.
        public IDbConnection CreateOpenConnection()
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}