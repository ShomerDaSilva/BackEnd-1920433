using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiArtistas.Validaciones;

namespace WebApiArtistas.Entidades
{
    public class Artista : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "el campo de nombre {0} es requerido")]
        [StringLength(maximumLength: 10, ErrorMessage = "El campo {0} solo puede tener 10 caracteres")]
        //[PrimeraLetraMayuscula]
        //si no cumple todas las validaciones no entra entonces tenemos q documentar el mapeado 
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

        [NotMapped]
        public int Menor { get; set; }

        [NotMapped]
        public int Mayor { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //Para que se ejecuten debe de primero cumplirse con las reglas por Atributo Ejemplo: Range
            // Tomar a consideración que primero se ejecutaran las validaciones mappeadas en los atributos
            // y posteriormente las declaradas en la entidad
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre[0].ToString();

                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayuscula",
                        new String[] { nameof(Nombre) });
                }
            }

            if (Menor > Mayor)
            {
                yield return new ValidationResult("Este valor no puede ser mas grande que el campo Mayor",
                    new String[] { nameof(Menor) });
            }
        }
    }
}
