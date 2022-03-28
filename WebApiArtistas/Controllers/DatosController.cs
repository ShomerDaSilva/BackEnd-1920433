using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiArtistas.Entidades;

namespace WebApiArtistas.Controllers
{
    [ApiController]
    [Route("api/datos")]
    public class DatosController: ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<DatosController> log;
        public DatosController (ApplicationDbContext context, ILogger<DatosController> log)
        {
            this.dbContext = context;
            this.log = log;
        }

        [HttpGet]
        [HttpGet("/listadoDato")]
        public async Task<ActionResult<List<Dato>>> GetAll()
        {
            return await dbContext.Dato.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Dato>> GetById(int id)
        {
            log.LogInformation("EL ID ES: " + id);
            return await dbContext.Dato.FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Dato dato)
        {
            var existeArtista = await dbContext.Artistas.AnyAsync(x=>x.Id == dato.ArtistaId);
            if (!existeArtista)
            {
                return BadRequest($"No existe el Artista con id:{dato.ArtistaId}");
            }

            dbContext.Add(dato);
            await dbContext.SaveChangesAsync();
            return Ok();

        } 

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Dato dato, int id)
        {
            var exist = await dbContext.Dato.AnyAsync(x => x.Id == id);
            if(!exist)
            {
                return NotFound("El dato especificado no existe. ");
            }

            if(dato.Id != id)
            {
                return BadRequest("El id del dato no coincide con el establecido en la url. ");
            }

            dbContext.Update(dato);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Dato.AnyAsync(x =>x.Id == id);
            if(!exist)
            {
                return NotFound("El Recurso no fue encontrado");
            }

            dbContext.Remove(new Dato { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
