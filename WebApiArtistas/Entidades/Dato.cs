using WebApiArtistas.Validaciones;

namespace WebApiArtistas.Entidades
{
    public class Dato
    {
        public int Id { get; set; }

        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        public string Edad { get; set; }

        public int ArtistaId { set; get; }

        public Artista artista { set; get; }
    }
}
