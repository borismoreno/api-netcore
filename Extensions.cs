using ApiNetCore.Dtos;
using ApiNetCore.Entities;

namespace ApiNetCore
{
    public static class Extensions
    {
        public static EmpresaDto AsDto(this Empresa empresa)
        {
            return new EmpresaDto(empresa.RazonSocial, empresa.Ruc, empresa.NombreComercial);
        }
    }
}