using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using MediCitasAPI.Models;

namespace MediCitasAPI.Controllers
{
    [RoutePrefix("api/usuarios")]
    public class UsuariosController : ApiController
    {
        string conexion = ConfigurationManager
            .ConnectionStrings["DefaultConnection"]
            .ConnectionString;

        // Método para probar conexión
        [HttpGet]
        [Route("test")]
        public IHttpActionResult GetDb()
        {
            using (SqlConnection con = new SqlConnection(conexion))
            {
                con.Open();
                return Ok("Conexión a BD OK");
            }
        }

        // Método para login
        [HttpPost]
        [Route("login")]
        public IHttpActionResult PostLogin(Usuario u)
        {
            using (SqlConnection con = new SqlConnection(conexion))
            {
                string sql = @"SELECT id_usuario, nombre, rol
                               FROM Usuarios
                               WHERE email = @email
                                 AND contraseña = @pass
                                 AND estado = 1";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@email", u.email);
                cmd.Parameters.AddWithValue("@pass", u.contraseña);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    return Ok(new
                    {
                        id_usuario = dr["id_usuario"],
                        nombre = dr["nombre"],
                        rol = dr["rol"]
                    });
                }

                return BadRequest("Credenciales incorrectas");
            }
        }

        // Método para registrar usuario
        [HttpPost]
        [Route("registrar")]
        public IHttpActionResult Registrar(Usuario u)
        {
            using (SqlConnection con = new SqlConnection(conexion))
            {
                string sql = @"INSERT INTO Usuarios 
                               (nombre, apellido, email, contraseña, rol)
                               VALUES
                               (@nombre, @apellido, @email, @pass, @rol)";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nombre", u.nombre);
                cmd.Parameters.AddWithValue("@apellido", u.apellido);
                cmd.Parameters.AddWithValue("@email", u.email);
                cmd.Parameters.AddWithValue("@pass", u.contraseña);
                cmd.Parameters.AddWithValue("@rol", u.rol);

                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    return Ok(new { mensaje = "Usuario registrado correctamente" });
                }
                catch (SqlException ex)
                {
                    // Manejo de error por correo duplicado
                    if (ex.Number == 2627) // Unique constraint
                        return BadRequest("El correo ya está registrado");
                    else
                        return InternalServerError(ex);
                }
            }
        }
    }
}