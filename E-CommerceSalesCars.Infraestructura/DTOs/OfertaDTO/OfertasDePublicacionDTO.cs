using E_CommerceSalesCars.Dominio.Entidades;
using E_CommerceSalesCars.Infraestructura.DTOs.PublicacionDTO.E_CommerceSalesCars.Infraestructura.DTOs.PublicacionDTO;
using E_CommerceSalesCars.Infraestructura.DTOs.UsuarioDTO;
using System;

namespace E_CommerceSalesCars.Infraestructura.DTOs.OfertaDTO
{
    public class OfertasDePublicacionDTO
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public EstadoOferta Estado { get; set; }

        public UsuarioRespuestaDto Comprador { get; set; }
    }
}
