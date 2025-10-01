using System;
using System.Collections.Generic;

namespace ApiBase.DAL.Modelos_BD_Universidad;

public partial class Role
{
    public int idRol { get; set; }

    public string nombre { get; set; } = null!;

    public string? descripcion { get; set; }

    public bool activo { get; set; }

    public string? motivoEliminacion { get; set; }

    public string? observaciones { get; set; }

    public DateTime fechaCreacion { get; set; }

    public DateTime? fechaActualizacion { get; set; }

    public DateTime? fechaEliminacion { get; set; }

    public int idUsuarioCreacion { get; set; }

    public int? idUsuarioActualizacion { get; set; }

    public int? idUsuarioEliminacion { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
