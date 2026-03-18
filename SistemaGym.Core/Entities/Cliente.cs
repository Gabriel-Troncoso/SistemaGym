using System;
using System.Collections.Generic;
namespace SistemaGym.Core.Entities;


public partial class Cliente
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Ci { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Estado { get; set; }
}
