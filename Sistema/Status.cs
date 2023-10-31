namespace Sistema
{
    /// <summary>
    /// Enum de los estados de una solicitud,
    /// 1 => APROBADO,
    /// 2 => RECHAZADO,
    /// 3 => PENDIENTE
    /// </summary>
    public enum Status
    {
        APROBADO = 1,
        RECHAZADA = 2,
        PENDIENTE_APROBACION = 3,
    }
}
