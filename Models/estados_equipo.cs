using System.ComponentModel.DataAnnotations;

namespace practica01.Models
{
    public class estados_equipo
    {
        [Key]
        public int id_estados_equipo { get; set; }
        public string? descripcion { get; set; }
        public char? estado{ get; set; }

    }
}
