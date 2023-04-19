using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Proyecto_EFSTRIII.Models
{
    public class Inmueble
    {
        [Display(Name = "ID Inmueble")]
        public int idInmueble { get; set; }

        [Display(Name = "Tipo Inmueble")]
        public int idTipoInmueble { get; set; }

        [Display(Name = "Inmueble")]
        public String descInmueble { get; set; }

        [Display(Name = "Ubicación")]
        public string ubiInmueble { get; set; }

        [Display(Name = "Costo")]
        public decimal costoInmueble { get; set; }

        [Display(Name = "Id Distrito")]
        public int idDistrito { get; set; }

        [Display(Name ="Imagen")]
        public string urlImagen { get; set; }

        [Display(Name = "Distrito")]
        public string nombreDistrito { get; set; }
    }
}