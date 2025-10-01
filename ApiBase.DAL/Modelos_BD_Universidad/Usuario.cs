using System;
using System.Collections.Generic;

namespace ApiBase.DAL.Modelos_BD_Universidad;

public partial class Usuario
{
    public int idUsuario { get; set; }

    public string userName { get; set; } = null!;

    public string? descripcion { get; set; }

    public string nombre { get; set; } = null!;

    public string primerApellido { get; set; } = null!;

    public string segundoApellido { get; set; } = null!;

    public string email { get; set; } = null!;

    public string passwordHash { get; set; } = null!;

    public string claveIdentidad { get; set; } = null!;

    public int idRol { get; set; }

    public bool activo { get; set; }

    public string? motivoEliminacion { get; set; }

    public string? observaciones { get; set; }

    public DateTime fechaCreacion { get; set; }

    public DateTime? fechaActualizacion { get; set; }

    public DateTime? fechaEliminacion { get; set; }

    public int idUsuarioCreacion { get; set; }

    public int? idUsuarioActualizacion { get; set; }

    public int? idUsuarioEliminacion { get; set; }

    public virtual ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();

    public virtual ICollection<GruposUsuario> GruposUsuarios { get; set; } = new List<GruposUsuario>();

    public virtual ICollection<ObjetosPerdido> ObjetosPerdidos { get; set; } = new List<ObjetosPerdido>();

    public virtual ICollection<Proyecto> Proyectos { get; set; } = new List<Proyecto>();

    public virtual ICollection<ProyectosUsuario> ProyectosUsuarios { get; set; } = new List<ProyectosUsuario>();

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual ICollection<TutoriasOferta> TutoriasOferta { get; set; } = new List<TutoriasOferta>();

    public virtual ICollection<TutoriasSolicitude> TutoriasSolicitudes { get; set; } = new List<TutoriasSolicitude>();

    public virtual Role idRolNavigation { get; set; } = null!;
}
