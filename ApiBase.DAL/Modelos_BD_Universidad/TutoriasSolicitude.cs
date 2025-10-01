using System;
using System.Collections.Generic;

namespace ApiBase.DAL.Modelos_BD_Universidad;

public partial class TutoriasSolicitude
{
    public int idTutoriaSolicitud { get; set; }

    public string? nombre { get; set; }

    public string? descripcion { get; set; }

    public int idUsuario { get; set; }

    public string materia { get; set; } = null!;

    public bool activo { get; set; }

    public string? motivoEliminacion { get; set; }

    public string? observaciones { get; set; }

    public DateTime fechaCreacion { get; set; }

    public DateTime? fechaActualizacion { get; set; }

    public DateTime? fechaEliminacion { get; set; }

    public int idUsuarioCreacion { get; set; }

    public int? idUsuarioActualizacion { get; set; }

    public int? idUsuarioEliminacion { get; set; }

    public virtual ICollection<TutoriasOfertasTutoriasSolicitude> TutoriasOfertasTutoriasSolicitudes { get; set; } = new List<TutoriasOfertasTutoriasSolicitude>();

    public virtual Usuario idUsuarioNavigation { get; set; } = null!;
}
