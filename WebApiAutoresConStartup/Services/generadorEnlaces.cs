using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using WebApiAutoresConStartup.DTOs;

namespace WebApiAutoresConStartup.Services
{
    public class generadorEnlaces
    {
        public IAuthorizationService AuthorizationService { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        public IActionContextAccessor ActionContextAccessor { get; }

        public generadorEnlaces(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor, IActionContextAccessor actionContextAccessor)
        {
            AuthorizationService = authorizationService;
            HttpContextAccessor = httpContextAccessor;
            ActionContextAccessor = actionContextAccessor;
        }

        private async Task<bool> EsAdmin()
        {
            var httpcontext = HttpContextAccessor.HttpContext;
            var resultado = await AuthorizationService.AuthorizeAsync(httpcontext.User, "esAdmin");
            return resultado.Succeeded;
        }

        private IUrlHelper ConstruirURLHelper()
        {
            var factoria = HttpContextAccessor.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
            return factoria.GetUrlHelper(ActionContextAccessor.ActionContext);
        }

        public async Task GenerarEnlaces(AutorDTO autorDTO)
        {
            var esAdmin = await EsAdmin();
            var Url = ConstruirURLHelper();

            autorDTO.Enlaces.Add(new DatoHATEOAS(
                enlace: Url.Link("obtenerAutor", new { }), descripcion: "self", metodo: "GET"));

            if (esAdmin)
            {
                autorDTO.Enlaces.Add(new DatoHATEOAS(
                enlace: Url.Link("borrarAutor", new { }), descripcion: "self", metodo: "DELETE"));

                autorDTO.Enlaces.Add(new DatoHATEOAS(
                enlace: Url.Link("actualizarAutor", new { }), descripcion: "actualizar-Autor", metodo: "PUT"));
            }

        }
    }
}
