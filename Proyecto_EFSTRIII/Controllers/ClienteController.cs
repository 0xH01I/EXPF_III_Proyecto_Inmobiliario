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
    public class ClienteController : Controller
    {

        static string cadena = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

        IEnumerable<Distrito> GetDistritos()
        {
            List<Distrito> distritos = new List<Distrito>();
            using (MySqlConnection connection = new MySqlConnection(cadena))
            {
                MySqlCommand cmd = new MySqlCommand("sp_GetListDistritos", connection);
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

        IEnumerable<Cliente> GetClientes()
        {
            List<Cliente> clientes = new List<Cliente>();
            using (MySqlConnection connection = new MySqlConnection(cadena))
            {
                MySqlCommand cmd = new MySqlCommand("sp_GetClientes", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                connection.Open();

                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    clientes.Add(new Cliente()
                    {
                        idCliente = dr.GetInt32(0),
                        nombreCliente = dr.GetString(1),
                        direccionCliente = dr.GetString(2),
                        idDistrito= dr.GetInt32(3),
                        nombreDistrito = dr.GetString(4),
                        correoCliente = dr.GetString(5),
                        telefonoCliente = dr.GetString(6)
                    });
                }
            }
            return clientes;
        }

        Cliente buscar(int id)
        {
            Cliente cliente;
            cliente = GetClientes().Where(item => item.idCliente == id).FirstOrDefault();
            return cliente;
        }


        // GET: Cliente
        public ActionResult Index(int pagina=0)
        {
            ViewBag.distritos = new SelectList(GetDistritos(), "idDistrito", "nombreDistrito", null);

            IEnumerable<Cliente> temporalCliente = GetClientes();
            int cantidadElementos = temporalCliente.Count();
            int filas = 5;
            int numeroPaginas = cantidadElementos % filas == 0 ? cantidadElementos / filas : (cantidadElementos / filas) + 1;
            ViewBag.pagina = pagina;
            ViewBag.numeroPaginas = numeroPaginas;
            return View(temporalCliente.Skip(pagina * filas).Take(filas));

        }

        [HttpPost]
        public ActionResult create(Cliente cliente)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.distritos = new SelectList(GetDistritos(), "idDistrito", "nombreDistrito", "");
                return View(cliente);
            }

            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connection"].ConnectionString);

            try
            {
                MySqlCommand cmd = new MySqlCommand("sp_InsertCliente", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prmstrNombreCliente", cliente.nombreCliente);
                cmd.Parameters.AddWithValue("@prmstrDirCliente", cliente.direccionCliente);
                cmd.Parameters.AddWithValue("@prmintIdDistrito", cliente.idDistrito );
                cmd.Parameters.AddWithValue("@prmstrCorreoCliente", cliente.correoCliente);
                cmd.Parameters.AddWithValue("@prmstrTlfCliente", cliente.telefonoCliente);
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

            ViewBag.distritos = new SelectList(GetDistritos(), "idDistrito", "nombreDistrito", "");
            return View("Index", GetClientes());
        }

        public ActionResult Create()
        {
            ViewBag.distritos = new SelectList(GetDistritos(), "idDistrito", "nombreDistrito", "");
            return View(new Cliente());
        }

        [HttpPost]
        public ActionResult Edit(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.distritos = new SelectList(GetDistritos(), "idDistrito", "nombreDistrito", "");
                return View(cliente);
            }


            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connection"].ConnectionString);

            try
            {
                MySqlCommand cmd = new MySqlCommand("sp_UpdateCliente", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prmintIdCliente", cliente.idCliente);
                cmd.Parameters.AddWithValue("@prmstrNombreCliente", cliente.nombreCliente);
                cmd.Parameters.AddWithValue("@prmstrDirCliente", cliente.direccionCliente);
                cmd.Parameters.AddWithValue("@prmintIdDistrito", cliente.idDistrito);
                cmd.Parameters.AddWithValue("@prmstrCorreoCliente", cliente.correoCliente);
                cmd.Parameters.AddWithValue("@prmstrTlfCliente", cliente.telefonoCliente);
                connection.Open();

                int cantidadActualizada = cmd.ExecuteNonQuery();
                ViewBag.mensaje = $"Se ha editado {cantidadActualizada} registro.";
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "ERROR: " + ex.Message;
                ViewBag.distritos = new SelectList(GetDistritos(), "idDistrito", "nombreDistrito", "");
                return View(cliente);
            }
            finally
            {
                connection.Close();
            }

            return View("Index", GetClientes());
        }

        public ActionResult Edit(int id)
        {
            Cliente cliente = buscar(id);
            ViewBag.distritos = new SelectList(GetDistritos(), "idDistrito", "nombreDistrito", "");
            return View(cliente);
        }
    }
}