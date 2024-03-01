using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiAutoresConStartup.Services;
using WebApiAutoresConStartup.Validations;

namespace WebApiAutoresConStartup.DTOs
{
    public class HATEOASAutorFilterAttribute : HATEOASFiltroAttribute
    {
        public generadorEnlaces GeneradorEnlaces { get; }


        public HATEOASAutorFilterAttribute(generadorEnlaces generadorEnlaces)
        {
            GeneradorEnlaces = generadorEnlaces;
        }


        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var debeIncluir = DebenIncluirHATEOAS(context);

            if (!debeIncluir)
            {
                await next();
                return;
            }

            var resultado = context.Result as ObjectResult;
            var autorDTO = resultado.Value as AutorDTO;
            if (autorDTO == null)
            {
                var autoresDTO = resultado.Value as List<AutorDTO> ??
                    throw new ArgumentException("Se esperaba una instancia de AutorDTO o List<AutorDTO");

                autoresDTO.ForEach(async autor => await GeneradorEnlaces.GenerarEnlaces(autor));
            }
            else
            {
                await GeneradorEnlaces.GenerarEnlaces(autorDTO);
            }

            await next();

        }

    }
}
