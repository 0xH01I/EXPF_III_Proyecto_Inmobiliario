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
    public class InmuebleController : Controller
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

        public IEnumerable<Inmueble> GetInmueble()
        {
            List<Inmueble> inmuebles = new List<Inmueble>();
            using (MySqlConnection connection = new MySqlConnection(cadena))
            {
                MySqlCommand command = new MySqlCommand("sp_GetInmueble", connection);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    string urlImagen2 = "";
                    try
                    {
                        urlImagen2 = dr.GetString(6);
                    }
                    catch (Exception ex)
                    {
                        urlImagen2 = "";
                    }

                    inmuebles.Add(new Inmueble()
                    {
                        idInmueble = dr.GetInt32(0),
                        idTipoInmueble = dr.GetInt32(1),
                        descInmueble = dr.GetString(2),
                        ubiInmueble = dr.GetString(3),
                        costoInmueble = dr.GetDecimal(4),
                        idDistrito = dr.GetInt32(5),
                        urlImagen = urlImagen2,
                        nombreDistrito = dr.GetString(7)
                        
                                           
                        
                    });

                }
            }
            return inmuebles;
        }
        Inmueble buscar(int id)
        {
            Inmueble inmueble;
            inmueble = GetInmueble().Where(item => item.idInmueble == id).FirstOrDefault();
            return inmueble;
        }

        // GET: Inmueble
        public ActionResult Index(int pagina =0)
        {
            ViewBag.tipos = new SelectList(GetTipoInmuebles(), "idTipoInmueble", "descripInmueble", null);
            ViewBag.distritos = new SelectList(GetDistritos(), "idDistrito", "nombreDistrito", null);

            IEnumerable<Inmueble> temporalInmueble = GetInmueble();
            int cantidadElementos = temporalInmueble.Count();
            int filas = 5;
            int numeroPaginas = cantidadElementos % filas == 0 ? cantidadElementos / filas : (cantidadElementos / filas) + 1;
            ViewBag.pagina = pagina;
            ViewBag.numeroPaginas = numeroPaginas;
            return View(temporalInmueble.Skip(pagina * filas).Take(filas));
        }

        [HttpPost]
        public ActionResult create(Inmueble inmueble)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.tipos = new SelectList(GetTipoInmuebles(), "idTipoInmueble", "descripInmueble", "");
                ViewBag.distritos = new SelectList(GetDistritos(), "idDistrito", "nombreDistrito", "");
                return View(inmueble);
            }

            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connection"].ConnectionString);

            try
            {
                MySqlCommand cmd = new MySqlCommand("sp_InsertInmueble", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("prmintIdTipoInmueble", inmueble.idTipoInmueble);
                cmd.Parameters.AddWithValue("prmstrUbicInmueble", inmueble.ubiInmueble);
                cmd.Parameters.AddWithValue("prmstrCostoInmueble", inmueble.costoInmueble);
                cmd.Parameters.AddWithValue("prmintIdDistrito", inmueble.idDistrito);
                cmd.Parameters.AddWithValue("prmstrUrlImagen", inmueble.urlImagen);
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
            ViewBag.distritos = new SelectList(GetDistritos(), "idDistrito", "nombreDistrito", "");
            return View("Index", GetInmueble());
        }

        public ActionResult Create()
        {
            ViewBag.tipos = new SelectList(GetTipoInmuebles(), "idTipoInmueble", "descripInmueble", "");
            ViewBag.distritos = new SelectList(GetDistritos(), "idDistrito", "nombreDistrito", "");
            return View(new Inmueble());
        }

        [HttpPost]
        public ActionResult Edit(Inmueble inmueble)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.tipos = new SelectList(GetTipoInmuebles(), "idTipoInmueble", "descripInmueble", "");
                ViewBag.distritos = new SelectList(GetDistritos(), "idDistrito", "nombreDistrito", "");
                return View(inmueble);
            }


            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connection"].ConnectionString);

            try
            {
                MySqlCommand cmd = new MySqlCommand("sp_UpdateInmueble", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prmintIdInmueble", inmueble.idInmueble);
                cmd.Parameters.AddWithValue("@prmintIdTipoInmueble", inmueble.idTipoInmueble);
                cmd.Parameters.AddWithValue("@prmstrUbicInmueble", inmueble.ubiInmueble);
                cmd.Parameters.AddWithValue("@prmstrCostoInmueble", inmueble.costoInmueble);
                cmd.Parameters.AddWithValue("@prmintIdDistrito", inmueble.idDistrito);
                cmd.Parameters.AddWithValue("prmstrUrlImagen", inmueble.urlImagen);
                connection.Open();

                int cantidadActualizada = cmd.ExecuteNonQuery();
                ViewBag.mensaje = $"Se ha editado {cantidadActualizada} registro.";
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = "ERROR: " + ex.Message;
                ViewBag.tipos = new SelectList(GetTipoInmuebles(), "idTipoInmueble", "descripInmueble", "");
                ViewBag.distritos = new SelectList(GetDistritos(), "idDistrito", "nombreDistrito", "");
                return View(inmueble);
            }
            finally
            {

                connection.Close();
            }

            return View("Index", GetInmueble());
        }

        public ActionResult Edit(int id)
        {
            Inmueble inmueble = buscar(id);
            ViewBag.tipos = new SelectList(GetTipoInmuebles(), "idTipoInmueble", "descripInmueble", "");
            ViewBag.distritos = new SelectList(GetDistritos(), "idDistrito", "nombreDistrito", "");
            return View(inmueble);
        }

    }
}