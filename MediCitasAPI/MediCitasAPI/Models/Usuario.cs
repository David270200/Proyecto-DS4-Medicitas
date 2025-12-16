using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediCitasAPI.Models
{
    public class Usuario
    {
        public int id_usuario { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
        public string contraseña { get; set; }
        public string rol { get; set; }
        public DateTime fecha_registro { get; set; }
        public bool estado { get; set; }
    }
}

