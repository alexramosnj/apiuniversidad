using System;
using System.Collections.Generic;

namespace ApiBase.DAL.Modelos_BD_Universidad;

public partial class TutoriasOfertasTutoriasSolicitude
{
    public int idTutoriasOfertasTutoriasSolicitudes { get; set; }

    public string? nombre { get; set; }

    public string? descripcion { get; set; }

    public int idTutoriaOferta { get; set; }

    public int idTutoriaSolicitud { get; set; }

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

    public virtual TutoriasOferta idTutoriaOfertaNavigation { get; set; } = null!;

    public virtual TutoriasSolicitude idTutoriaSolicitudNavigation { get; set; } = null!;
}
