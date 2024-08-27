using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Anime
{
    public int IdAnime { get; set; }

    public string Nombre { get; set; } = null!;

    public byte[] Imagen { get; set; } = null!;

    public string Texto { get; set; } = null!;

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
}
