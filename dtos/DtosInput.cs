using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ApiNetCore.Entities;

namespace ApiNetCore.Dtos
{
    public record LoginDto(
        [Required(ErrorMessage = "El mail es requerido")]
        string Email,
        [Required(ErrorMessage = "El password es requerido")]
        string Password
    );

    #region Empresa
    public record EmpresaCreateDto(
        string ObligadoContabilidad,
        string SecuencialNotaCredito,
        string SecuencialRetencion,
        bool RegimenMicroempresas,
        int Ambiente,
        int TipoEmision,
        string RazonSocial,
        string NombreComercial,
        string Ruc,
        string Establecimiento,
        string PuntoEmision,
        string DireccionMatriz,
        string DireccionEstablecimiento,
        string ContribuyenteEspecial,
        string SecuencialFactura,
        string ClaveFirma,
        string PathCertificado,
        string MailEnvioComprobantes,
        string NombreNotificacion
    );

    #endregion

    #region Usuario

    public record UsuarioCrearDto(
        string Rol,
        string Nombre,
        string Email,
        string Password,
        string IdEmpresa
    );

    #endregion

    #region Tipo Forma Pago

    public record TipoFormaPagoInsertDto(
        string FormaPago,
        string Codigo,
        string[] TipoPago
    );

    public record TipoProductoInsertarDto(
        string Descripcion,
        string Codigo
    );

    public record TarifaIvaInsertarDto(
        string Porcentaje,
        string Codigo
    );

    #endregion

    public record ProductoInsertarDto(
        string CodigoPrincipal,
        string CodigoAuxiliar,
        string TipoProducto,
        string TarifaIva,
        string Descripcion,
        decimal ValorUnitario
    );

    public record ClienteInsertarDto(
        string RazonSocial,
        string TipoIdentificacion,
        string NumeroIdentificacion,
        string Telefono,
        string Mail,
        string Direccion
    );

    public record TipoIdentificacionInsertarDto(
        string Descripcion,
        string Codigo
    );

    public record GuardarFacturaDto(
        string IdCliente,
        string FechaEmision,
        List<ImpuestosDetalleDto> ImpuestosDetalle,
        List<DetallesDto> Detalles,
        FormasPagoDto FormaPago,
        List<DatoAdicionalDto> DatosAdicionales,
        bool Autorizar,
        DetalleValoresDto DetalleValores
    );

    public record ImpuestosDetalleDto(
        decimal SubtotalImpuesto,
        decimal ValorImpuesto,
        decimal BaseImponible,
        string CodigoPorcentaje,
        decimal TarifaImpuesto
    );

    public record DetallesDto(
        string Id,
        decimal ValorUnitario,
        int Cantidad,
        string TarifaIva,
        decimal Subtotal
    );

    public record FormasPagoDto(
        string TipoFormaPago,
        decimal ValorPago,
        string Plazo,
        string TipoPlazo
    );

    public record DatoAdicionalDto(
        string NombreAdicional,
        string ValorAdicional
    );

    public record DetalleValoresDto(
        decimal TotalSinImpuestos,
        decimal TotalDescuento,
        decimal TotalIva,
        decimal ImporteTotal,
        decimal Propina
    );
}