using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiArtistas.Entidades
{
    public class Artista
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "el campo de nombre {0} es requerido")]
        [StringLength(maximumLength: 10, ErrorMessage = "El campo {0} solo puede tener 10 caracteres")]
        public string Nombre { get; set; }

        [Range(1, 100, ErrorMessage = "El campo Albums no se encuentra dentro del rango")]
        [NotMapped]
        public int Albums { get; set; }

        [CreditCard]
        [NotMapped]
        public string Tarjeta { get; set; }

        [Url]
        [NotMapped]
        public string Url { get; set; }

        public List<Dato> datos { get; set; }
    }
}
