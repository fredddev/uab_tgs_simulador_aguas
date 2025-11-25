using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Threading.Tasks;

namespace GestionAguaPeriurbanos.Data
{
    // Simple DTO representing a saved scenario
    public class EscenarioDto
    {
        public int? Id { get; set; }
        public string Name { get; set; } = "";
        public double TankCapacity { get; set; }
        public double InitialVolume { get; set; }
        public double[] EntradaDaily { get; set; } = Array.Empty<double>();
        public double[] ConsumoDaily { get; set; } = Array.Empty<double>();
        public string ReglasJson { get; set; } = "{}"; // serialized ReglasFeedbackModel
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class EscenariosRepository
    {
        private readonly MySqlConnectionFactory _factory;

        // Recommended schema (example):
        // CREATE TABLE escenarios (id INT AUTO_INCREMENT PRIMARY KEY, name VARCHAR(200), payload JSON, created_at DATETIME);
        public EscenariosRepository(MySqlConnectionFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task<int> SaveAsync(EscenarioDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var payload = JsonSerializer.Serialize(dto);
            using var conn = _factory.CreateOpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO escenarios (name, payload, created_at) VALUES (@name, @payload, @created_at); SELECT LAST_INSERT_ID();";
            var p1 = cmd.CreateParameter(); p1.ParameterName = "@name"; p1.Value = dto.Name; cmd.Parameters.Add(p1);
            var p2 = cmd.CreateParameter(); p2.ParameterName = "@payload"; p2.Value = payload; cmd.Parameters.Add(p2);
            var p3 = cmd.CreateParameter(); p3.ParameterName = "@created_at"; p3.Value = dto.CreatedAt; cmd.Parameters.Add(p3);

            var idObj = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(idObj);
        }

        public async Task<EscenarioDto?> GetByIdAsync(int id)
        {
            using var conn = _factory.CreateOpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT payload FROM escenarios WHERE id = @id LIMIT 1";
            var p = cmd.CreateParameter(); p.ParameterName = "@id"; p.Value = id; cmd.Parameters.Add(p);
            var obj = await cmd.ExecuteScalarAsync();
            if (obj == null) return null;
            var payload = Convert.ToString(obj)!;
            return JsonSerializer.Deserialize<EscenarioDto>(payload);
        }

        public async Task<IEnumerable<EscenarioDto>> GetAllAsync()
        {
            var list = new List<EscenarioDto>();
            using var conn = _factory.CreateOpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT payload FROM escenarios ORDER BY created_at DESC";
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var payload = reader.GetString(0);
                var dto = JsonSerializer.Deserialize<EscenarioDto>(payload);
                if (dto != null) list.Add(dto);
            }
            return list;
        }
    }
}