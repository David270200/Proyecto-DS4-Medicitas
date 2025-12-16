using System;

namespace MediCitasAPI.Models
{
    public class Cita
    {
        public int id_cita { get; set; }
        public int id_paciente { get; set; }
        public int id_medico { get; set; }
        public DateTime fecha { get; set; }
        public TimeSpan hora { get; set; }
        public string estado { get; set; }
    }
}
