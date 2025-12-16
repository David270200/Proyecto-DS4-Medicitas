using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;

namespace MediCitasAPI.Controllers
{
    [RoutePrefix("api/doctores")]
    public class DoctorsController : ApiController
    {
        string conexion = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        [HttpGet]
        [Route("")]
        public IHttpActionResult ObtenerDoctores(string especialidad = null)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conexion))
                {
                    string sql = @"
                        SELECT m.id_medico, m.nombre, m.apellido, e.nombre AS especialidad
                        FROM Medicos m
                        INNER JOIN Especialidades e ON m.id_especialidad = e.id_especialidad
                        WHERE m.estado = 1";

                    if (!string.IsNullOrEmpty(especialidad))
                        sql += " AND e.nombre LIKE @especialidad";

                    SqlCommand cmd = new SqlCommand(sql, con);
                    if (!string.IsNullOrEmpty(especialidad))
                        cmd.Parameters.AddWithValue("@especialidad", $"%{especialidad}%");

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    var lista = new List<object>();
                    while (dr.Read())
                    {
                        lista.Add(new
                        {
                            Id = dr["id_medico"],
                            Nombre = $"{dr["nombre"]} {dr["apellido"]}",
                            Especialidad = dr["especialidad"]
                        });
                    }

                    return Ok(lista);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }  
    }
}