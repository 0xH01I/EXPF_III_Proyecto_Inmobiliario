using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Proyecto_EFSTRIII.Models;

namespace Proyecto_EFSTRIII.Controllers
{
    public class DistritoController : Controller
    {
        static string cadena = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;
        IEnumerable<Distrito> GetDistritos()
        {
            List<Distrito> distritos = new List<Distrito>();
            using (MySqlConnection connection = new MySqlConnection(cadena))
            {
                MySqlCommand cmd = new MySqlCommand("GetDistritos", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                connection.Open();
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    distritos.Add(new Distrito()
                    {
                        idDistrito = dr.GetInt32(0),
                        nombreDistrito = dr.GetString(1)
                    
                    });
                }
            }
            return distritos;
        }

        // GET: Distrito
        public ActionResult Index(int pagina = 0)
        {
            IEnumerable<Distrito> tmpProveedores = GetDistritos();
            int cantidadElementos = tmpProveedores.Count();
            int filasPorPagina = 5;
            int numeroPaginas = (cantidadElementos % filasPorPagina == 0)
                ? (cantidadElementos / filasPorPagina)
                : (cantidadElementos / filasPorPagina) + 1;

            ViewBag.proveedores = new SelectList(GetDistritos(), "idDistrito", "nombreDistrito", null);
            ViewBag.pagina = pagina;
            ViewBag.numeroPaginas = numeroPaginas;

            return View(tmpProveedores.Skip(pagina * filasPorPagina).Take(filasPorPagina));
        }
        /*
        public ActionResult CreateProveedor()
        {
            ViewBag.paises = new SelectList(GetPaises(), "idPais", "nombrePais", null);
            return View(new Proveedor());
        }
        [HttpPost]
        public ActionResult CreateProveedor(Proveedor proveedor)
        {
            ViewBag.paises = new SelectList(GetPaises(), "idPais", "nombrePais", null);
            if (!ModelState.IsValid)
            {
                return View(proveedor);
            }

            SqlConnection connection = new SqlConnection(cadena);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertProveedor", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prmstrRazonSocial", proveedor.razonSocial);
                cmd.Parameters.AddWithValue("@prmstrDireccion", proveedor.direccion);
                cmd.Parameters.AddWithValue("@prmstrTelefono", proveedor.telefono);
                cmd.Parameters.AddWithValue("@prmintIdPais", proveedor.idPais);

                connection.Open();
                cmd.ExecuteNonQuery();
                ViewBag.mensaje = $"Proveedor Registrado";
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            finally
            {
                connection.Close();
            }
            return View(new Proveedor());
        }
        public ActionResult EditProveedor(int idProveedor)
        {
            ViewBag.paises = new SelectList(GetPaises(), "idPais", "nombrePais", null);
            Proveedor proveedor = GetProveedores().Where(p => p.idProveedor == idProveedor).FirstOrDefault();
            return View(proveedor);
        }
        [HttpPost]
        public ActionResult EditProveedor(Proveedor proveedor)
        {
            ViewBag.paises = new SelectList(GetPaises(), "idPais", "nombrePais", null);
            if (!ModelState.IsValid)
            {
                return View(proveedor);
            }

            SqlConnection connection = new SqlConnection(cadena);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateProveedor", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prmintIdProveedor", proveedor.idProveedor);
                cmd.Parameters.AddWithValue("@prmstrRazonSocial", proveedor.razonSocial);
                cmd.Parameters.AddWithValue("@prmstrDireccion", proveedor.direccion);
                cmd.Parameters.AddWithValue("@prmstrTelefono", proveedor.telefono);
                cmd.Parameters.AddWithValue("@prmintIdPais", proveedor.idPais);

                connection.Open();
                cmd.ExecuteNonQuery();
                ViewBag.mensaje = $"Proveedor Actualizado";
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            finally
            {
                connection.Close();
            }
            return View(new Proveedor());
        }*/
    }
}