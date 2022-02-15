namespace WebApiArtistas.Entidades
{
    public class Artista
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public List<Dato> datos { get; set; }
    }
}
