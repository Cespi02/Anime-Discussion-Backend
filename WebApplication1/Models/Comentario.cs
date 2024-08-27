using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Comentario
{
    public int IdComentario { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdAnime { get; set; }

    public byte[]? Imagen { get; set; }

    public string Contenido { get; set; } = null!;

    public DateTime Fechayhora { get; set; }

    public virtual Anime? IdAnimeNavigation { get; set; }

    public virtual Cliente? IdUsuarioNavigation { get; set; }
}
