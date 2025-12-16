using MediCitasAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MediCitasAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/citas")]
    public class CitasController : ApiController
    {
        string conexion = ConfigurationManager
            .ConnectionStrings["DefaultConnection"]
            .ConnectionString;

        // POST api/citas
  
        [HttpPost]
        [Route("")]
        public IHttpActionResult GuardarCita(Cita c)
        {
            using (SqlConnection con = new SqlConnection(conexion))
            {
                string sql = @"INSERT INTO Citas
                               (id_paciente, id_medico, fecha, hora)
                               VALUES
                               (@id_paciente, @id_medico, @fecha, @hora)";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id_paciente", c.id_paciente);
                cmd.Parameters.AddWithValue("@id_medico", c.id_medico);
                cmd.Parameters.AddWithValue("@fecha", c.fecha.Date);
                cmd.Parameters.AddWithValue("@hora", TimeSpan.Parse(c.hora.ToString()));

                con.Open();
                cmd.ExecuteNonQuery();

                return Ok("Cita guardada correctamente");
            }
        }

        // GET api/citas/paciente/{id}
  
        [HttpGet]
        [Route("paciente/{id}")]
        public IHttpActionResult VerCitasPorPaciente(int id)
        {
            using (SqlConnection con = new SqlConnection(conexion))
            {
                string sql = @"
                    SELECT 
                        c.id_cita,
                        c.fecha,
                        FORMAT(c.hora,'HH:mm') AS hora,
                        c.estado,
                        m.nombre + ' ' + m.apellido AS medico
                    FROM Citas c
                    INNER JOIN Medicos m ON c.id_medico = m.id_medico
                    WHERE c.id_paciente = @id";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                var lista = new List<object>();
                while (dr.Read())
                {
                    lista.Add(new
                    {
                        id_cita = dr["id_cita"],
                        fecha = dr["fecha"],
                        hora = dr["hora"],
                        estado = dr["estado"],
                        medico = dr["medico"]
                    });
                }

                return Ok(lista);
            }
        }

        // GET api/citas  (REPORTE ADMIN)
    
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetCitas()
        {
            using (SqlConnection con = new SqlConnection(conexion))
            {
                string sql = @"
            SELECT
                c.id_cita,
                u.nombre + ' ' + u.apellido AS paciente,
                m.nombre + ' ' + m.apellido AS medico,
                c.fecha,
                CONVERT(VARCHAR(5), c.hora, 108) AS hora,
                c.estado
            FROM Citas c
            INNER JOIN Usuarios u ON c.id_paciente = u.id_usuario
            INNER JOIN Medicos m ON c.id_medico = m.id_medico";

                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                var lista = new List<object>();

                while (dr.Read())
                {
                    lista.Add(new
                    {
                        id_cita = dr["id_cita"],
                        paciente = dr["paciente"],
                        medico = dr["medico"],
                        fecha = dr["fecha"],
                        hora = dr["hora"],
                        estado = dr["estado"]
                    });
                }

                return Ok(lista);
            }
        }


        // GET api/citas/disponibles/{id}?fecha=yyyy-MM-dd

        [HttpGet]
        [Route("disponibles/{id}")]
        public IHttpActionResult ObtenerHorariosDisponibles(int id, [FromUri] string fecha)
        {
            if (!DateTime.TryParseExact(fecha, "yyyy-MM-dd", null,
                System.Globalization.DateTimeStyles.None, out DateTime fechaCita))
                return BadRequest("Fecha inválida");

            using (SqlConnection con = new SqlConnection(conexion))
            {
                string sql = @"
                    DECLARE @Horarios TABLE (Hora TIME(0));
                    INSERT INTO @Horarios VALUES
                    ('09:00'),('10:00'),('11:00'),('12:00'),
                    ('13:00'),('14:00'),('15:00'),('16:00');

                    SELECT CONVERT(VARCHAR(5), h.Hora, 108) AS Hora
                    FROM @Horarios h
                    WHERE NOT EXISTS (
                        SELECT 1 FROM Citas c
                       WHERE c.id_medico = @id
                         AND c.fecha = @fecha
                          AND CAST(c.hora AS TIME(0)) = h.Hora
                     );";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@fecha", fechaCita.Date);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                var horarios = new List<string>();
                while (dr.Read())
                    horarios.Add(dr["Hora"].ToString());

                return Ok(horarios);
            }
        }

    }
}