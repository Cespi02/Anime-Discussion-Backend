using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class CategoriasPrestamo
{
    public int IdCategoria { get; set; }

    public double? Limite { get; set; }

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
