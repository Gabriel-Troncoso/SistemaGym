namespace SistemaGym.Infrastructure.Queries
{
    public static class Primero
    {
        public static string clientesMySql = @"
        select Id, Nombre, Apellido, Ci, Telefono, Correo, FechaRegistro
        from cliente
        order by FechaRegistro desc
        LIMIT @Limit;";

        public static string clientesSql = @"
        select Id, Nombre, Apellido, Ci, Telefono, Correo, FechaRegistro
        from cliente
        order by FechaRegistro desc
        OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;
        ";
        public static string planesMySql = @"
        select idPlan as Id, NombrePlan, Descripcion, DuracionDias, Precio, Estado
        from plan_membresia
        order by idPlan desc
        LIMIT @Limit;";

        public static string planesSql = @"
        select idPlan as Id, NombrePlan, Descripcion, DuracionDias, Precio, Estado
        from plan_membresia
        order by idPlan desc
        OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;
";
        public static string membresiasMySql = @"
        select idMembresia as Id, ClienteId, PlanMembresiaId, FechaInicio, FechaFin, Estado
        from membresia
        order by idMembresia desc
        LIMIT @Limit;";

        public static string membresiasSql = @"
        select idMembresia as Id, ClienteId, PlanMembresiaId, FechaInicio, FechaFin, Estado
        from membresia
        order by idMembresia desc
        OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;
        ";

        public static string pagosMySql = @"
        select idPago as Id, MembresiaId, Monto, FechaPago, MetodoPago, Estado
        from pago
        order by idPago desc
        LIMIT @Limit;";

        public static string pagosSql = @"
        select idPago as Id, MembresiaId, Monto, FechaPago, MetodoPago, Estado
        from pago
        order by idPago desc
        OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;
        ";
    }
}
