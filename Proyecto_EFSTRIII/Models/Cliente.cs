using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_EFSTRIII.Models
{
    public class Cliente
    {
        [Display(Name = "ID Cliente")]
        public int idCliente { get; set; }

        [Display(Name = "Nombre")]
        public string nombreCliente { get; set; }

        [Display(Name = "Dirección")]
        public string direccionCliente { get; set; }

        [Display(Name = " ID Distrito")]
        public int  idDistrito { get; set; }

        [Display(Name = "Distrito")]
        public string nombreDistrito { get; set; }

        [Display(Name = "Correo ")]
        public string correoCliente { get; set; }

        [Display(Name = "Teléfono")]
        public string telefonoCliente { get; set; }
    }
}