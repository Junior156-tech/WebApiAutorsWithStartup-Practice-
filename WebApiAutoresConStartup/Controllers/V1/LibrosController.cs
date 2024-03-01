using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using WebApiAutoresConStartup.DTOs;
using WebApiAutoresConStartup.Entidades;

namespace WebApiAutoresConStartup.Controllers.V1
{

    [ApiController]
    [Route("api/V1/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}", Name = "obtenerLibro")]
        public async Task<ActionResult<LibrosDTOConAutores>> Get(int id)
        {
            var libro = await context.Libros
                .Include(autorLibro => autorLibro.autorLibro)
                .ThenInclude(autorDB => autorDB.autor)
                .FirstOrDefaultAsync(x => x.id == id);

            if(libro == null)
            {
                return NotFound();
            }

            libro.autorLibro = libro.autorLibro.OrderBy(x => x.Orden).ToList();

            return mapper.Map<LibrosDTOConAutores>(libro);
        }

        [HttpPost(Name = "crearLibro")]
        public async Task<ActionResult> Post(LibrosCreacionDTO librosCreacionDTO)
        {
            if (librosCreacionDTO.AutoresIds == null)
            {
                return BadRequest("No se puede crear un libro sin autores");
            }

            var autoresIds = await context.Autores.Where(autorDB => librosCreacionDTO.AutoresIds.Contains(autorDB.Id)).Select(x => x.Id).ToListAsync();

            if(librosCreacionDTO.AutoresIds.Count != autoresIds.Count)
            {
                return BadRequest("No existe uno de los autores enviados");
            }
            //bool exist = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);

            //if(!exist)
            //{
            //    return BadRequest($"No existe el autor de id: {libro.AutorId}");
            //}

            var libro = mapper.Map<Libros>(librosCreacionDTO);
            AsignarOrdenAutores(libro);


            var libroDTO = mapper.Map<LibrosDTO>(libro);

            context.Add(libro);
            await context.SaveChangesAsync();
            return CreatedAtRoute("obtenerLibro", new {id = libro.id }, libroDTO); 
        }

        [HttpPut("{id:int}", Name = "actualizarLibro")]
        public async Task<ActionResult> Put (int id, LibrosCreacionDTO librosCreacionDTO)
        {
            var libroDB = await context.Libros
                .Include(x => x.autorLibro)
                .FirstOrDefaultAsync(x => x.id == id);

            if(libroDB == null)
            {
                return NotFound();
            }

            libroDB = mapper.Map(librosCreacionDTO, libroDB);
            AsignarOrdenAutores(libroDB);

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "patchLibro" )]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibrosPatchDTO> patchDocument)
        {
            if(patchDocument == null)
            {
                return BadRequest();
            }

            var librosDB = context.Libros.FirstOrDefaultAsync(x => x.id == id);
            
            if(librosDB == null)
            {
                return NotFound();
            }

            var libroDTO = mapper.Map<LibrosPatchDTO>(librosDB);

            patchDocument.ApplyTo(libroDTO, ModelState);

            var esValido = TryValidateModel(librosDB);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            await mapper.Map(libroDTO, librosDB);

            await context.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete("{id:int}", Name = "borrarLibro")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Libros.AnyAsync(x => x.id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Libros { id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }

        private void AsignarOrdenAutores(Libros libro)
        {
            if (libro.autorLibro != null)
            {
                for (int i = 0; i < libro.autorLibro.Count; i++)
                {
                    libro.autorLibro[i].Orden = i;
                }
            }
        }

    }
}
