using System;
using System.Net.Http;
using System.Web.UI;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MediCitasWeb
{
    public partial class AgendarCita : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["nombre"] != null && Session["apellido"] != null)
                    lblBienvenida.Text = $"Bienvenido {Session["nombre"]} {Session["apellido"]} a MediCitas";
                else
                    lblBienvenida.Text = "Bienvenido a MediCitas";

                CargarDoctores();
            }
        }

        private async void CargarDoctores()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44345/");
                    HttpResponseMessage response = await client.GetAsync("api/doctores");

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        var doctores = JsonConvert.DeserializeObject<List<Doctor>>(result);

                        doctoresContainer.Controls.Clear();

                        foreach (var doc in doctores)
                        {
                            var div = new System.Web.UI.WebControls.Panel();
                            div.CssClass = "doctor-card";
                            div.Attributes["onclick"] = $"seleccionarDoctor({doc.Id}, '{doc.Nombre.Replace("'", "\\'")}')";

                            div.Controls.Add(new System.Web.UI.WebControls.Label { Text = $"<h3>{doc.Nombre}</h3>", EnableViewState = false });
                            div.Controls.Add(new System.Web.UI.WebControls.Label { Text = $"<p>ID: {doc.Id}</p>", EnableViewState = false });
                            div.Controls.Add(new System.Web.UI.WebControls.Label { Text = $"<p>Especialidad: {doc.Especialidad}</p>", EnableViewState = false });

                            doctoresContainer.Controls.Add(div);
                        }
                    }
                    else
                    {
                        doctoresContainer.Controls.Add(new System.Web.UI.WebControls.Label
                        {
                            Text = "No se pudieron cargar los doctores.",
                            ForeColor = System.Drawing.Color.Red
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                doctoresContainer.Controls.Add(new System.Web.UI.WebControls.Label
                {
                    Text = "Error al cargar doctores: " + ex.Message,
                    ForeColor = System.Drawing.Color.Red
                });
            }
        }

        public class Doctor
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Especialidad { get; set; }
        }
    }
}

