using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiAutoresConStartup.DTOs;

namespace WebApiAutoresConStartup.Controllers.V1
{

    [ApiController]
    [Route("Api/V1")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RootController : ControllerBase
    {
        public IAuthorizationService AuthorizationService { get; }


        public RootController(IAuthorizationService authorizationService)
        {
            AuthorizationService = authorizationService;
        }


        [HttpGet(Name = "ObtenerRoot")]
        public async Task<ActionResult<IEnumerable<DatoHATEOAS>>> Get()
        {
            var datoHateoas = new List<DatoHATEOAS>();

            var esAdmin = await AuthorizationService.AuthorizeAsync(User, "esAdmin");

            datoHateoas.Add(new DatoHATEOAS(enlace: Url.Link("ObtenerRoot", new { }), descripcion: "self", metodo: "GET"));
            datoHateoas.Add(new DatoHATEOAS(enlace: Url.Link("obtenerAutores", new { }), descripcion: "autores", metodo: "GET"));

            if (esAdmin.Succeeded)
            {
                datoHateoas.Add(new DatoHATEOAS(enlace: Url.Link("crearAutor", new { }), descripcion: "autor-crear", metodo: "POST"));
                datoHateoas.Add(new DatoHATEOAS(enlace: Url.Link("crearLibro", new { }), descripcion: "libro crear", metodo: "POST"));
            }

            return datoHateoas;
        }
    }
}
