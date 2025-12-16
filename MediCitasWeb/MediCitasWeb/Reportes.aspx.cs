using System;
using System.Net.Http;
using System.Web.UI;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MediCitasWeb
{
    public partial class Reportes : Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await CargarCitasAsync();
            }
        }

        protected async Task CargarCitasAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44345/");

                    
                    HttpResponseMessage response = await client.GetAsync("api/citas");

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();


                        if (string.IsNullOrWhiteSpace(result))
                        {
                            lblMsg.Text = "No hay citas registradas.";
                            return;
                        }

                        var citas = JsonConvert.DeserializeObject<List<Cita>>(result);

                        gvCitas.DataSource = citas;
                        gvCitas.DataBind();
                    }
                    else
                    {
                        lblMsg.Text = "Error al cargar las citas desde la API.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Ocurrió un error: " + ex.Message;
            }
        }


        public class Cita
        {
            public int id_cita { get; set; }
            public string paciente { get; set; }
            public string medico { get; set; }
            public string fecha { get; set; }
            public string hora { get; set; }
            public string estado { get; set; }
        }

    }
}