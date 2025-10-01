using System;
using System.Collections.Generic;

namespace ApiBase.DAL.Modelos_BD_Universidad;

public partial class ObjetosPerdido
{
    public int idObjetoPerdido { get; set; }

    public string nombre { get; set; } = null!;

    public string? descripcion { get; set; }

    public int idUsuario { get; set; }

    public string? lugar { get; set; }

    public DateOnly fecha { get; set; }

    public string? fotoUrl { get; set; }

    public string estado { get; set; } = null!;

    public bool activo { get; set; }

    public string? motivoEliminacion { get; set; }

    public string? observaciones { get; set; }

    public DateTime fechaCreacion { get; set; }

    public DateTime? fechaActualizacion { get; set; }

    public DateTime? fechaEliminacion { get; set; }

    public int idUsuarioCreacion { get; set; }

    public int? idUsuarioActualizacion { get; set; }

    public int? idUsuarioEliminacion { get; set; }

    public virtual Usuario idUsuarioNavigation { get; set; } = null!;
}
