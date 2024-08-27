using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Prestamo
{
    public int IdPrestamo { get; set; }

    public double? Monto { get; set; }

    public DateOnly? PrimerVencimiento { get; set; }

    public int? Cuotas { get; set; }

    public int? Punitorios { get; set; }

    public int? IdCategoria { get; set; }

    public virtual CategoriasPrestamo? IdCategoriaNavigation { get; set; }
}
