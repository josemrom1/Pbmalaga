namespace ApiClientApp.Models
{
    /// <summary>
    /// Modelo reutilizable para representar una fila de la hoja de Google Sheets
    /// que se está usando en esta aplicación.
    /// </summary>
    public class Penista
    {
        public string? Nombre { get; set; }

        public string? Telefono { get; set; }

        public string? DNI { get; set; }

        public string? GoogleURL { get; set; }

        public string? AppleURL { get; set; }

        public string? ShareURL { get; set; }
    }
}
