namespace CalculadoraAPI.Models
{
    public class HistorialOperacion
    {
        public int Id { get; set; }                 // Representa el campo Id de la tabla
        public string? Operacion { get; set; }       // Representa el campo Operacion (ejemplo: "5 + 3")
        public double Resultado { get; set; }       // Representa el resultado del cálculo
        public DateTime Fecha { get; set; }         // Representa la fecha de la operación
    }
}