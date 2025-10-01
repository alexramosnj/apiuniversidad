// DTOs simples para entrada
public class EspacioCrearDto
{
    public string nombre { get; set; } = string.Empty;
    public string? descripcion { get; set; }
    public string? clasificacion { get; set; }
    public string? observaciones { get; set; }
    public bool activo { get; set; } = true;
}