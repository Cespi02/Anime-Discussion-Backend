using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public string? Nombre { get; set; }

    public string? NroDoc { get; set; }

    public string? Contrasenia { get; set; }

    public string? Email { get; set; }

    public double? Sueldo { get; set; }
}
