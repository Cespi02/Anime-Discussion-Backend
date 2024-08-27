using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string? NombreUsuario { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? NroDoc { get; set; }

    public string? Contrasenia { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
}
