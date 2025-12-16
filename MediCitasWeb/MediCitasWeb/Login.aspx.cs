using System;
using System.Net.Http;
using System.Text;
using System.Web.UI;
using Newtonsoft.Json;

namespace MediCitasWeb
{
    public partial class Login : Page
    {
        protected async void btnLogin_Click(object sender, EventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44345/");

                var loginData = new
                {
                    email = txtEmail.Text,
                    contraseña = txtPass.Text
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(loginData),
                    Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage response =
                    await client.PostAsync("api/usuarios/login", content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(result);

                    // GUARDAR SESIÓN
                    Session["id_usuario"] = data.id_usuario.ToString();
                    Session["nombre"] = data.nombre.ToString();
                    Session["rol"] = data.rol.ToString();

                    // REDIRECCIÓN POR ROL
                    if (Session["rol"].ToString().ToUpper() == "ADMIN")
                    {
                        Response.Redirect("Reportes.aspx");
                    }
                    else
                    {
                        Response.Redirect("AgendarCita.aspx");
                    }
                }
                else
                {
                    lblMsg.Text = "Correo o contraseña incorrectos";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        //  REGISTRO 

        protected async void btnSubmitRegister_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            lblMsg.ForeColor = System.Drawing.Color.Red;

            try
            {
                var nuevoUsuario = new
                {
                    nombre = txtNombre.Text,
                    apellido = txtApellido.Text,
                    email = txtEmailReg.Text,
                    contraseña = txtPassReg.Text,
                    rol = "PACIENTE"
                };

                string json = JsonConvert.SerializeObject(nuevoUsuario);

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44345/");
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response =
                        await client.PostAsync("api/usuarios/registrar", content);

                    if (response.IsSuccessStatusCode)
                    {
                        lblMsg.ForeColor = System.Drawing.Color.Green;
                        lblMsg.Text = "Usuario registrado correctamente. Ahora puede iniciar sesión.";

                        txtNombre.Text = "";
                        txtApellido.Text = "";
                        txtEmailReg.Text = "";
                        txtPassReg.Text = "";

                        ScriptManager.RegisterStartupScript(
                            this,
                            this.GetType(),
                            "showLogin",
                            "mostrarLogin();",
                            true
                        );
                    }
                    else
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        lblMsg.Text = "Error al registrar: " + result;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Ocurrió un error: " + ex.Message;
            }
        }
    }
}