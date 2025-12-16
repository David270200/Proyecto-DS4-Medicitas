using System.Data.SqlClient;
using System.Configuration;
using System.Web.Http;

namespace MediCitasAPI.Controllers
{
    [RoutePrefix("api/reportes")]
    public class ReportesController : ApiController
    {
        string conexion = ConfigurationManager
            .ConnectionStrings["DefaultConnection"]
            .ConnectionString;

        // Citas de hoy
        [HttpGet]
        [Route("citas-hoy")]
        public IHttpActionResult CitasHoy()
        {
            using (SqlConnection con = new SqlConnection(conexion))
            {
                string sql = @"
                    SELECT 
                        c.id_cita,
                        u.nombre + ' ' + u.apellido AS paciente,
                        m.nombre + ' ' + m.apellido AS medico,
                        c.hora,
                        c.estado
                    FROM Citas c
                    INNER JOIN Usuarios u ON c.id_paciente = u.id_usuario
                    INNER JOIN Medicos m ON c.id_medico = m.id_medico
                    WHERE c.fecha = CAST(GETDATE() AS DATE)
                ";

                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                var lista = new System.Collections.Generic.List<object>();

                while (dr.Read())
                {
                    lista.Add(new
                    {
                        id_cita = dr["id_cita"],
                        paciente = dr["paciente"],
                        medico = dr["medico"],
                        hora = dr["hora"],
                        estado = dr["estado"]
                    });
                }

                return Ok(lista);
            }
        }
        // Cantidad de citas por médico
        [HttpGet]
        [Route("citas-por-medico")]
        public IHttpActionResult CitasPorMedico()
        {
            using (SqlConnection con = new SqlConnection(conexion))
            {
                string sql = @"
                    SELECT 
                        m.nombre + ' ' + m.apellido AS medico,
                        COUNT(c.id_cita) AS total_citas
                    FROM Citas c
                    INNER JOIN Medicos m ON c.id_medico = m.id_medico
                    GROUP BY m.nombre, m.apellido
                ";

                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                var lista = new System.Collections.Generic.List<object>();

                while (dr.Read())
                {
                    lista.Add(new
                    {
                        medico = dr["medico"],
                        total_citas = dr["total_citas"]
                    });
                }

                return Ok(lista);
            }
        }
        // Cantidad de citas por estado
        [HttpGet]
        [Route("citas-por-estado")]
        public IHttpActionResult CitasPorEstado()
        {
            using (SqlConnection con = new SqlConnection(conexion))
            {
                string sql = @"
                    SELECT 
                        estado,
                        COUNT(*) AS total
                    FROM Citas
                    GROUP BY estado
                ";

                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                var lista = new System.Collections.Generic.List<object>();

                while (dr.Read())
                {
                    lista.Add(new
                    {
                        estado = dr["estado"],
                        total = dr["total"]
                    });
                }

                return Ok(lista);
            }
        }
    }
}