using System;
using System.Collections.Generic;
using ApiNetCore.Entities;

namespace ApiNetCore.Dtos
{
    public record UsuarioAutenticadoDto(
        bool Ok,
        string Msg,
        string Uid = "",
        string Nombre = "",
        string NombreComercial = "",
        string EmpresaId = "",
        string Token = ""
    );

    public record UsuarioCreadoDto(
        bool Ok,
        string Msg,
        Guid Id
    );

    public record EmpresaCreadaDto(
        bool Ok,
        string Msg,
        Empresa Empresa = null
    );

    public record EmpresaDto(
        string RazonSocial,
        string Ruc,
        string NombreComercial
    );

    public record EmpresaRespuestaDto(
        bool Ok,
        string Msg,
        List<EmpresaDto> Empresas = null
    );

    public record EmpresaConfiguracionDto(
        bool Ok,
        string Msg,
        string SecuencialFactura,
        string SecuencialRetencion,
        string SecuencialNotaCredito,
        string PuntoEmision,
        string Establecimiento,
        string RazonSocial
    );

    public record TiposFormaPagoDto(
        bool Ok,
        string Msg,
        List<TipoFormaPago> TiposFormaPago
    );

    public record TiposProductoDto(
        bool Ok,
        string Msg,
        List<TipoProducto> TiposProducto
    );

    public record TarifasIvaConsultaDto(
        bool Ok,
        string Msg,
        List<TarifaIva> TarifasIva
    );

    public record ProductoInsertadoDto(
        bool Ok,
        string Msg,
        Producto Producto
    );

    public record ProductosConsultaDto(
        bool Ok,
        string Msg,
        List<Producto> Productos
    );
}