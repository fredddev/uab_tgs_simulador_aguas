using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Threading.Tasks;
using GestionAguaPeriurbanos.Model;

namespace GestionAguaPeriurbanos.Data
{
    // Simple record used to persist a set of ResultadoDia
    public class SimulacionRecord
    {
        public int? Id { get; set; }
        public string Name { get; set; } = "";
        public ResultadoDia[] Results { get; set; } = Array.Empty<ResultadoDia>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class SimulacionesRepository
    {
        private readonly MySqlConnectionFactory _factory;

        // Recommended schema (example):
        // CREATE TABLE simulaciones (id INT AUTO_INCREMENT PRIMARY KEY, name VARCHAR(200), payload JSON, created_at DATETIME);
        public SimulacionesRepository(MySqlConnectionFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task<int> SaveAsync(SimulacionRecord record)
        {
            if (record == null) throw new ArgumentNullException(nameof(record));
            var payload = JsonSerializer.Serialize(record);
            using var conn = _factory.CreateOpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO simulaciones (name, payload, created_at) VALUES (@name, @payload, @created_at); SELECT LAST_INSERT_ID();";
            var p1 = cmd.CreateParameter(); p1.ParameterName = "@name"; p1.Value = record.Name; cmd.Parameters.Add(p1);
            var p2 = cmd.CreateParameter(); p2.ParameterName = "@payload"; p2.Value = payload; cmd.Parameters.Add(p2);
            var p3 = cmd.CreateParameter(); p3.ParameterName = "@created_at"; p3.Value = record.CreatedAt; cmd.Parameters.Add(p3);

            var idObj = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(idObj);
        }

        public async Task<SimulacionRecord?> GetByIdAsync(int id)
        {
            using var conn = _factory.CreateOpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT payload FROM simulaciones WHERE id = @id LIMIT 1";
            var p = cmd.CreateParameter(); p.ParameterName = "@id"; p.Value = id; cmd.Parameters.Add(p);
            var obj = await cmd.ExecuteScalarAsync();
            if (obj == null) return null;
            var payload = Convert.ToString(obj)!;
            return JsonSerializer.Deserialize<SimulacionRecord>(payload);
        }

        public async Task<IEnumerable<SimulacionRecord>> GetAllAsync()
        {
            var list = new List<SimulacionRecord>();
            using var conn = _factory.CreateOpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT payload FROM simulaciones ORDER BY created_at DESC";
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var payload = reader.GetString(0);
                var dto = JsonSerializer.Deserialize<SimulacionRecord>(payload);
                if (dto != null) list.Add(dto);
            }
            return list;
        }
    }
}