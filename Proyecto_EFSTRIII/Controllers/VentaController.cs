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
    public class VentaController : Controller
    {

        static string cadena = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

        IEnumerable<TipoInmueble> GetTipoInmuebles()
        {
            List<TipoInmueble> tipoInmuebles = new List<TipoInmueble>();

            using (MySqlConnection connection = new MySqlConnection(cadena))
            {
                MySqlCommand command = new MySqlCommand("sp_GetTipoInmueble", connection);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    tipoInmuebles.Add(new TipoInmueble()
                    {
                        idTipoInmueble = dr.GetInt32(0),
                        descripInmueble = dr.GetString(1)
                    });
                }
            }

            return tipoInmuebles;
        }

        IEnumerable<FormaPago> GetFormaPago()
        {
            List<FormaPago> formaPagos = new List<FormaPago>();

            using (MySqlConnection connection = new MySqlConnection(cadena))
            {
                MySqlCommand command = new MySqlCommand("sp_GetFormaPago", connection);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    formaPagos.Add(new FormaPago()
                    {
                        idPago = dr.GetInt32(0),
                        descripPago = dr.GetString(1)
                    });
                }
            }

            return formaPagos;
        }

        IEnumerable<Condicion> GetCondicion()
        {
            List<Condicion> condiciones = new List<Condicion>();

            using (MySqlConnection connection = new MySqlConnection(cadena))
            {
                MySqlCommand command = new MySqlCommand("sp_GetCondicion", connection);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    condiciones.Add(new Condicion()
                    {
                        idCondicion = dr.GetInt32(0),
                        descripCondicion = dr.GetString(1)
                    });
                }
            }

            return condiciones;
        }

        IEnumerable<Cliente> GetClientesVentas()
        {
            List<Cliente> clientes = new List<Cliente>();

            using (MySqlConnection connection = new MySqlConnection(cadena))
            {
                MySqlCommand command = new MySqlCommand("sp_GetClientesVentas", connection);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    clientes.Add(new Cliente()
                    {
                        idCliente = dr.GetInt32(0),
                        nombreCliente = dr.GetString(1)
                    });
                }
            }

            return clientes;
        }

        Venta buscar(int id)
        {
            Venta ventas;
            ventas = GetVentas().Where(item => item.idVenta == id).FirstOrDefault();
            return ventas;
        }


        IEnumerable<Venta> GetVentas()
        {
            List<Venta> ventas = new List<Venta>();
            using (MySqlConnection connection = new MySqlConnection(cadena))
            {
                MySqlCommand cmd = new MySqlCommand("sp_GetListVentas", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                connection.Open();
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ventas.Add(new Venta()
                    {
                        idVenta = dr.GetInt32(0),
                        idInmueble = dr.GetInt32(1),
                        idCliente = dr.GetInt32(2),
                        nombreCliente = dr.GetString(3),
                        idCondicion = dr.GetInt32(4),
                        descrpCondicion = dr.GetString(5),
                        idFormaPago = dr.GetInt32(6),
                        descripFormaPago = dr.GetString(7),
                        totalVenta = dr.GetDecimal(8),
                        idTipoInmueble = dr.GetInt32(9),
                        descripTipoInmueble=dr.GetString(10),
                        totalGeneral = dr.GetDecimal(11),
                        fechaVenta = dr.GetDateTime(12)

                    });
                }
            }
            return ventas;
        }

        // GET: Venta
        public ActionResult Index(int pagina = 0)
        {

            IEnumerable<Venta> temporalVentas = GetVentas();
            int cantidadElementos = temporalVentas.Count();
            int filas = 5;
            int numeroPaginas = cantidadElementos % filas == 0 ? cantidadElementos / filas : (cantidadElementos / filas) + 1;
            ViewBag.pagina = pagina;
            ViewBag.numeroPaginas = numeroPaginas;
            return View(temporalVentas.Skip(pagina * filas).Take(filas));

        }

        [HttpPost]
        public ActionResult create(Venta ventas)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.tipos = new SelectList(GetTipoInmuebles(), "idTipoInmueble", "descripInmueble", "");
                ViewBag.pagos = new SelectList(GetFormaPago(), "idPago", "descripPago", "");
                ViewBag.condiciones = new SelectList(GetCondicion(), "idCondicion", "descripCondicion", "");
                ViewBag.clientes = new SelectList(GetClientesVentas(), "idCliente", "nombreCliente", "");
                return View(ventas);
            }

            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connection"].ConnectionString);

            try
            {
                MySqlCommand cmd = new MySqlCommand("sp_InsertVenta", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("prmintIdInmueble", ventas.idInmueble);
                cmd.Parameters.AddWithValue("prmintIdCliente", ventas.idCliente);
                cmd.Parameters.AddWithValue("prmintIdCondicion", ventas.idCondicion);
                cmd.Parameters.AddWithValue("prmintIdFormaPago", ventas.idFormaPago);
                cmd.Parameters.AddWithValue("prmintTotalVenta", ventas.totalVenta);
                cmd.Parameters.AddWithValue("prmintIdTipoInmueble", ventas.idTipoInmueble);
                cmd.Parameters.AddWithValue("prmintTotalGeneral", ventas.totalGeneral);
                cmd.Parameters.AddWithValue("prmdateFechaVenta", ventas.fechaVenta);
                connection.Open();

                int cantidadActualizada = cmd.ExecuteNonQuery();
                ViewBag.mensaje = $"Se ha insertado { cantidadActualizada} registro.";


            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            finally
            {
                connection.Close();
            }
            ViewBag.tipos = new SelectList(GetTipoInmuebles(), "idTipoInmueble", "descripInmueble", "");
            ViewBag.pagos = new SelectList(GetFormaPago(), "idPago", "descripPago", "");
            ViewBag.condiciones = new SelectList(GetCondicion(), "idCondicion", "descripCondicion", "");
            ViewBag.clientes = new SelectList(GetClientesVentas(), "idCliente", "nombreCliente", "");
            return View("Index", GetVentas());
        }

        public ActionResult Create()
        {
            ViewBag.tipos = new SelectList(GetTipoInmuebles(), "idTipoInmueble", "descripInmueble", "");
            ViewBag.pagos = new SelectList(GetFormaPago(), "idPago", "descripPago", "");
            ViewBag.condiciones = new SelectList(GetCondicion(), "idCondicion", "descripCondicion", "");
            ViewBag.clientes = new SelectList(GetClientesVentas(), "idCliente", "nombreCliente", "");
            return View(new Venta());
        }

        [HttpPost]
        public ActionResult Edit(Venta ventas)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.tipos = new SelectList(GetTipoInmuebles(), "idTipoInmueble", "descripInmueble", "");
                ViewBag.pagos = new SelectList(GetFormaPago(), "idPago", "descripPago", "");
                ViewBag.condiciones = new SelectList(GetCondicion(), "idCondicion", "descripCondicion", "");
                ViewBag.clientes = new SelectList(GetClientesVentas(), "idCliente", "nombreCliente", "");
                return View(ventas);
            }


            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connection"].ConnectionString);

            try
            {
                MySqlCommand cmd = new MySqlCommand("sp_UpdateVenta", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("prmintIdVenta", ventas.idVenta);
                cmd.Parameters.AddWithValue("prmintIdInmueble", ventas.idInmueble);
                cmd.Parameters.AddWithValue("prmintIdCliente", ventas.idCliente);
                cmd.Parameters.AddWithValue("prmintIdCondicion", ventas.idCondicion);
                cmd.Parameters.AddWithValue("prmintIdFormaPago", ventas.idFormaPago);
                cmd.Parameters.AddWithValue("prmintTotalVenta", ventas.totalVenta);
                cmd.Parameters.AddWithValue("prmintIdTipoInmueble", ventas.idTipoInmueble);
                cmd.Parameters.AddWithValue("prmintTotalGeneral", ventas.totalGeneral);
                cmd.Parameters.AddWithValue("prmdateFechaVenta", ventas.fechaVenta.ToString("yyyy-MM-dd"));
                connection.Open();

                int cantidadActualizada = cmd.ExecuteNonQuery();
                ViewBag.mensaje = $"Se ha editado {cantidadActualizada} registro.";
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "ERROR: " + ex.Message;
                ViewBag.tipos = new SelectList(GetTipoInmuebles(), "idTipoInmueble", "descripInmueble", "");
                ViewBag.pagos = new SelectList(GetFormaPago(), "idPago", "descripPago", "");
                ViewBag.condiciones = new SelectList(GetCondicion(), "idCondicion", "descripCondicion", "");
                ViewBag.clientes = new SelectList(GetClientesVentas(), "idCliente", "nombreCliente", "");
                return View(ventas);
            }
            finally
            {

                connection.Close();
            }

            return View("Index", GetVentas());
        }


        public ActionResult Edit(int id)
        {
            Venta ventas = buscar(id);
            ViewBag.tipos = new SelectList(GetTipoInmuebles(), "idTipoInmueble", "descripInmueble", "");
            ViewBag.pagos = new SelectList(GetFormaPago(), "idPago", "descripPago", "");
            ViewBag.condiciones = new SelectList(GetCondicion(), "idCondicion", "descripCondicion", "");
            ViewBag.clientes = new SelectList(GetClientesVentas(), "idCliente", "nombreCliente", "");
            return View(ventas);
        }


    }
}