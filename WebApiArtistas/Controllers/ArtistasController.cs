using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiArtistas.Entidades;
using WebApiArtistas.Filtros;
using WebApiArtistas.Services;

namespace WebApiArtistas.Controllers
{
    [ApiController]
    [Route("api/artistas")]
    public class ArtistasController: ControllerBase
    {
        private readonly IWebHostEnvironment env;
        private readonly string archivoPost = "nuevosRegistros.txt"; 
        private readonly string archivoGet = "registroConsultado.txt";
        private readonly ApplicationDbContext dbContext;
        private readonly IService service;
        private readonly ServiceTransient serviceTransient;
        private readonly ServiceScoped serviceScoped;
        private readonly ServiceSingleton serviceSingleton;
        private readonly ILogger<ArtistasController> logger;
        

        //inyeccion de dependecias 
        public ArtistasController(ApplicationDbContext context, IService service,
            ServiceTransient serviceTransient, ServiceScoped serviceScoped,
            ServiceSingleton serviceSingleton, ILogger<ArtistasController> logger, IWebHostEnvironment env)
        {
            this.dbContext = context;
            this.service = service;
            this.serviceTransient = serviceTransient;
            this.serviceScoped = serviceScoped;
            this.serviceSingleton = serviceSingleton;
            this.logger = logger;
            this.env = env;
        }



        [HttpGet("GUID")]
        [ServiceFilter(typeof(FiltroDeAccion))]    
        public ActionResult ObtenerGuid()
        {
            return Ok(new
            {
                AlumnosControllerTransient = serviceTransient.guid,
                ServiceA_Transient = service.GetTransient(),
                AlumnosControllerScoped = serviceScoped.guid,
                ServiceA_Scoped = service.GetScoped(),
                AlumnosControllerSingleton = serviceSingleton.guid,
                ServiceA_Singleton = service.GetSingleton()
            });

            //return mapper.Map<List<GetArtistaDTO>>(alumnos);
        }

        [HttpGet]
        [HttpGet("listado")]
        [HttpGet("/listado")]
        //[ResponseCache(Duration = 10)]
        //[Authorize]
        [ServiceFilter(typeof(FiltroDeAccion))]
        public async Task<ActionResult<List<Artista>>> Get()
        {
            //* Niveles de logs
            // Critical
            // Error
            // Warning
            // Information
            // Debug
            // Trace
            // *//
            throw new NotImplementedException();
            logger.LogInformation("Se obtiene el listado de alumnos");
            logger.LogWarning("Mensaje de prueba warning");
            service.EjecutarJob();
            return await dbContext.Artistas.Include(x => x.datos).ToListAsync();
        }



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




        [HttpGet("{id:int}")]
         public async Task<ActionResult<Artista>> Get(int id)
         {
            var artista =  await dbContext.Artistas.FirstOrDefaultAsync(x => x.Id == id);

             if(artista == null)
             {
                 return NotFound();
             }
            return artista;
         }

        //  CON STRING 

        [HttpGet("{nombre}")]
        public async Task<ActionResult<Artista>> Get([FromRoute]string nombre)
        {
            var artista = await dbContext.Artistas.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));

            if (artista == null)
            {
                logger.LogError("No se encuentra el alumno. ");
                return NotFound();
            }

            var ruta = $@"{env.ContentRootPath}\wwwroot\{archivoGet}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true)) { writer.WriteLine($@"{ artista.Id},{artista.Nombre}"); }
            return artista;
        }










        // VAROPS PARAMETROS
        /*[HttpGet("{id:int}/{param=Drake}")]
        //[HttpGet("{id:int}/{param?}")]
        public async Task<ActionResult<Artista>> Get(int id, string param)
        {
            var artista = await dbContext.Artistas.FirstOrDefaultAsync(x => x.Id == id);

           if (artista == null)
            {
                return NotFound();
            }
            return artista;
        }*/

       
        [HttpPost]
        
        public async Task<ActionResult> Post([FromBody] Artista artista)
        {

            var existeAlumnoMismoNombre = await dbContext.Artistas.AnyAsync(x => x.Nombre == artista.Nombre);

            if (existeAlumnoMismoNombre)
            {
                return BadRequest("Ya existe un artista con el mismo nombre");
            }

            var ruta = $@"{env.ContentRootPath}\wwwroot\{archivoPost}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true) ) { writer.WriteLine($@"{ artista.Id},{artista.Nombre}"); }
            dbContext.Add(artista);
            await dbContext.SaveChangesAsync();
            return Ok();
        }



        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Artista artista, int id)
        {
            var exist = await dbContext.Artistas.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            if (artista.Id != id)
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
