using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WebApiAutoresConStartup.DTOs;
using WebApiAutoresConStartup.Entidades;
using WebApiAutoresConStartup.Utility;

namespace WebApiAutoresConStartup.Controllers.V1
{

    [ApiController]
    //[Route("api/V1/autores")]
    [Route("api/autores")]
    [CabeceraEstaPresente("x-version", "1")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAuthorizationService authorizationService;

        public AutoresController(ApplicationDbContext context, IMapper mapper, IAuthorizationService authorizationService)
        {
            this.context = context;
            this.mapper = mapper;
            this.authorizationService = authorizationService;
        }

        [HttpGet(Name = "obtenerAutoresV1")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public async Task<ActionResult<List<AutorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Autores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var autores = await queryable.OrderBy(autor => autor.name).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);
        }

        
        [HttpGet("{id:int}", Name = "obtenerAutorV1")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AutorDTOConLibros>> GetById(int id, [FromHeader] string MostrarHATEOAS)
        {
            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");

            var autor = await context.Autores
                .Include(autorDB => autorDB.autorLibro)
                .ThenInclude(autorLibro => autorLibro.libros)
                .FirstOrDefaultAsync(autorBD => autorBD.Id == id);

            if (autor == null)
                return NotFound($"El id: {id} no existe");

            var dto =  mapper.Map<AutorDTOConLibros>(autor);
            return dto;

        }

       

        [HttpGet("{nombre}", Name = "obtenerAutorPorNombreV1")]
        public async Task<ActionResult<List<AutorDTO>>> GetByName([FromRoute] string nombre)
        {
            var autor = await context.Autores.Where(autorDB => autorDB.name.Contains(nombre)).ToListAsync();

            return mapper.Map<List<AutorDTO>>(autor);
        }

        [HttpPost(Name = "crearAutorV1")]
        public async Task<ActionResult> Post(AutorCreacionDTO autorCreacionDTO)
        {
            var autor = mapper.Map<Autor>(autorCreacionDTO);

            context.Add(autor);
            await context.SaveChangesAsync();

            var autorDTO = mapper.Map<AutorDTO>(autor);
            return CreatedAtRoute("ObtenerAutorV1", new { id = autor.Id }, autorDTO);
        }

        [HttpPut("{id:int}", Name = "actualizarAutorV1")]
        public async Task<ActionResult> Patch(AutorCreacionDTO autorCreacionDTO, int id)
        {

            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            var autor = mapper.Map<Autor>(autorCreacionDTO);  
            autor.Id = id;


            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Borra un autor
        /// </summary>
        /// <param name="id">Id del autor a borrar</param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "borrarAutorV1")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
