using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_EFSTRIII.Models
{
    public class Venta
    {

        [Display(Name = "ID Venta")]
        public int idVenta { get; set; }

        [Display(Name = "ID Inmueble")]
        public int idInmueble { get; set; }

        [Display(Name = "Cliente")]
        public int idCliente { get; set; }

        [Display(Name = "Cliente")]
        public string nombreCliente { get; set; }

        [Display(Name = "Estado")]
        public int idCondicion { get; set; }

        [Display(Name = "Estado")]
        public string descrpCondicion { get; set; }

        [Display(Name = "Forma de Pago")]
        public int idFormaPago { get; set; }

        [Display(Name = "Forma de Pago")]
        public string descripFormaPago { get; set; }

        [Display(Name = "Total Venta")]
        public decimal totalVenta { get; set; }

        [Display(Name = "Tipo Inmueble")]
        public int idTipoInmueble { get; set; }

        [Display(Name = "Tipo Inmueble")]
        public string descripTipoInmueble { get; set; }

        [Display(Name = "Total General")]
        public decimal totalGeneral { get; set; }

        [Display(Name = "Fecha de Venta")]
        public DateTime fechaVenta { get; set; }

    }
}