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


        [HttpGet] //api/artistas
        [HttpGet("listado")] //api/artistas/listado
        [HttpGet("/listado")] // /listado
        public async Task<ActionResult<List<Artista>>> Get()
        {
            return await dbContext.Artistas.Include(x => x.datos).ToListAsync();
        }



        /* -----------------------------------------------------
        * -----------------------------------------------------
        *                     CLASE 21/02/21
        * -----------------------------------------------------
        * -----------------------------------------------------*/

        [HttpGet("primero")] //api/artistas/primero
        public async Task<ActionResult<Artista>> PrimerArtista([FromHeader] int valor, [FromQuery] string artista, [FromQuery] int artistaId)
        {
            return await dbContext.Artistas.FirstOrDefaultAsync();
        }

        [HttpGet("primero2")] //api/artistas/primero
        public ActionResult<Artista> PrimerArtistaD()
        {
            return new Artista() { Nombre = "Dos" };
        }




        /*[HttpGet("{id:int}")]
         public async Task<ActionResult<Artista>> Get(int id)
         {
            var artista =  await dbContext.Artistas.FirstOrDefaultAsync(x => x.Id == id);

             if(artista == null)
             {
                 return NotFound();
             }
             return artista;
         }*/

        //  CON STRING 

        [HttpGet("{nombre}")]
        public async Task<ActionResult<Artista>> Get([FromRoute]string nombre)
        {
            var artista = await dbContext.Artistas.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));

            if (artista == null)
            {
                return NotFound();
            }
            return artista;
        }

        // VAROPS PARAMETROS
        [HttpGet("{id:int}/{param=Drake}")]
        //[HttpGet("{id:int}/{param?}")]
        public async Task<ActionResult<Artista>> Get(int id, string param)
        {
            var artista = await dbContext.Artistas.FirstOrDefaultAsync(x => x.Id == id);

           if (artista == null)
            {
                return NotFound();
            }
            return artista;
        }

        /* -----------------------------------------------------
        * -----------------------------------------------------
        *                     CLASE 21/02/21
        * -----------------------------------------------------
        * -----------------------------------------------------*/
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Artista artista)
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
