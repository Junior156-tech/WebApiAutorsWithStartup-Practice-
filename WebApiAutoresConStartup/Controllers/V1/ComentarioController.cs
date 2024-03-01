using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutoresConStartup.DTOs;
using WebApiAutoresConStartup.Entidades;
using WebApiAutoresConStartup.Utility;

namespace WebApiAutoresConStartup.Controllers.V1
{
    [ApiController]
    [Route("api/V1/libros/{libroId:int}/comentarios")]
    public class ComentarioController : ControllerBase
    {
        public ApplicationDbContext Context { get; }
        public IMapper Mapper { get; }
        public UserManager<IdentityUser> UserManager { get; }

        public ComentarioController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager )
        {
            Context = context;
            Mapper = mapper;
            UserManager = userManager;
        }

        [HttpGet(Name = "obtenerComentariosLibros")]
        public async Task<ActionResult<List<ComentarioDTO>>> Get (int libroId, [FromQuery] PaginacionDTO paginacionDTO)
        {

            var existeLibro = await Context.Libros.AnyAsync(librDB => librDB.id == libroId);

            if (!existeLibro)
                return NotFound();


            var queryable = Context.Comentarios.Where(comentariosDB => comentariosDB.libroId == libroId).AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var comentario = await queryable.OrderBy(comentario => comentario.id)
                .Paginar(paginacionDTO).ToListAsync();

            return Mapper.Map<List<ComentarioDTO>>(comentario);   
        }

        [HttpGet("id:int", Name = "obtenerComentario")]
        public ActionResult<ComentarioDTO> GetById(int id)
        {
            var comentario = Context.Comentarios.FirstOrDefaultAsync(comentarioDB => comentarioDB.id == id);

            if (comentario == null)
            {
                return NotFound();
            }

            return Mapper.Map<ComentarioDTO>(comentario);
        }

        [HttpPost(Name = "crearComentario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int libroId, ComentarioCreacionDTO comentarioCreactionDTO)
        {

            var emailClaim =  HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await UserManager.FindByEmailAsync(email);
            var usuarioID = usuario.Id;

            var existeLibro = await Context.Libros.AnyAsync(librDB => librDB.id == libroId);

            if (!existeLibro)
                return NotFound();

            var comentario = Mapper.Map<Comentario>(comentarioCreactionDTO);
            comentario.libroId = libroId;
            comentario.AutorId = usuarioID; 

            var comentarioDTO = Mapper.Map<ComentarioDTO>(comentario);
            Context.Add(comentario);
            await Context.SaveChangesAsync();
            return CreatedAtRoute("obtenerComentario", new {id = comentario.id, libroId = libroId }, comentarioDTO);
        }

        [HttpPut("{id:int}", Name = "actualizarComentario")]
        public async Task<ActionResult> Put (int libroId, int id, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var existeLibro = await Context.Libros.AnyAsync(librDB => librDB.id == libroId);

            if (!existeLibro)
                return NotFound();

            var existeComentario = await Context.Comentarios.AnyAsync(comentarioDB => comentarioDB.id == id);

            if (!existeComentario)
                return NotFound();

            var comentario = Mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.id = id;
            comentario.libroId = libroId;
            Context.Update(comentario);
            await Context.SaveChangesAsync();
            return NoContent(); 
        }

    }
}
