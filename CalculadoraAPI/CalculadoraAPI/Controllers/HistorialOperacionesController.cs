using Microsoft.AspNetCore.Mvc;
using CalculadoraAPI.Data;
using CalculadoraAPI.Models;
using System.Data.SqlClient;

namespace CalculadoraAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialOperacionesController : ControllerBase
    {
        private readonly DatabaseConnection _databaseConnection;

        public HistorialOperacionesController(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        // Obtener todos los cálculos
        [HttpGet("todos")]
        public IActionResult GetTodos()
        {
            var operaciones = new List<HistorialOperacion>();

            using (var connection = _databaseConnection.GetConnection())
            {
                connection.Open();
                string query = "SELECT Id, Operacion, Resultado, Fecha FROM HistorialOperaciones";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    operaciones.Add(new HistorialOperacion
                    {
                        Id = (int)reader["Id"],
                        Operacion = reader["Operacion"].ToString(),
                        Resultado = (double)reader["Resultado"],
                        Fecha = (DateTime)reader["Fecha"]
                    });
                }
            }

            return Ok(operaciones);
        }

        // Obtener todas las sumas realizadas
        [HttpGet("sumas")]
        public IActionResult GetSumas()
        {
            var operaciones = new List<HistorialOperacion>();

            using (var connection = _databaseConnection.GetConnection())
            {
                connection.Open();
                string query = "SELECT Id, Operacion, Resultado, Fecha FROM HistorialOperaciones WHERE Operacion LIKE '%+%'";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    operaciones.Add(new HistorialOperacion
                    {
                        Id = (int)reader["Id"],
                        Operacion = reader["Operacion"].ToString(),
                        Resultado = (double)reader["Resultado"],
                        Fecha = (DateTime)reader["Fecha"]
                    });
                }
            }

            return Ok(operaciones);
        }

        // Agregar un nuevo cálculo
        [HttpPost("agregar")]
        public IActionResult AgregarOperacion([FromBody] HistorialOperacion operacion)
        {
            using (var connection = _databaseConnection.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO HistorialOperaciones (Operacion, Resultado, Fecha) VALUES (@Operacion, @Resultado, @Fecha)";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Operacion", operacion.Operacion);
                command.Parameters.AddWithValue("@Resultado", operacion.Resultado);
                command.Parameters.AddWithValue("@Fecha", DateTime.Now);

                command.ExecuteNonQuery();
            }

            return Ok("Operación agregada con éxito.");
        }
    }
}
