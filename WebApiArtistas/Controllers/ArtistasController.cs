using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiArtistas.Entidades;

namespace WebApiArtistas.Controllers
{
    [ApiController]
    [Route("api/artistas")]
    public class ArtistasController: ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public ArtistasController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Artista>>> Get()
        {
            return await dbContext.Artistas.Include(x => x.datos).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Artista artista)
        {
            dbContext.Add(artista);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Artista artista, int id)
        {
            if(artista.Id != id)
            {
                return BadRequest("El id del artista no coicide con el establecido");
            }

            dbContext.Update(artista);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Artistas.AnyAsync(x => x.Id == id);
            if(!exist)
            {
                return NotFound();
            }

            dbContext.Remove(new Artista()
            {
                Id = id
            });
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
