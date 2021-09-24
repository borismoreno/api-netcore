namespace ApiNetCore.Models
{
    public enum EstadoComprobante
    {
        [EnumValue("PENDIENTE")]
        Pendiente,
        [EnumValue("AUTORIZADO")]
        Autorizado,
        [EnumValue("NO AUTORIZADO")]
        NoAutorizado,
        [EnumValue("DEVUELTA")]
        Devuelto,
        [EnumValue("EN PROCESAMIENTO")]
        EnProcesamiento,
        [EnumValue("RECIBIDA")]
        Recibido,
        [EnumValue("ANULADO")]
        Anulado,
        [EnumValue("PENDIENTE REENVIO")]
        PendienteReenvio,
        [EnumValue("INDETERMINADO")]
        Indeterminado
    }
}