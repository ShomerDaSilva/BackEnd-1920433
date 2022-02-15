using Microsoft.EntityFrameworkCore;
using WebApiArtistas.Entidades;

namespace WebApiArtistas
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Artista> Artistas { get; set; }
        public DbSet<Dato> Dato { get; set; }
    }
}
