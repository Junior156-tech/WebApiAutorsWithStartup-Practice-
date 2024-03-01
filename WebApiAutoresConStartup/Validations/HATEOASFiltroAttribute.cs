using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiAutoresConStartup.Validations
{
    public class HATEOASFiltroAttribute : ResultFilterAttribute
    {
        
        protected bool DebenIncluirHATEOAS(ResultExecutingContext context)
        {
            var resultado = context.Result as ObjectResult;

            if(!esRespuestaExitosa(resultado))
            {
                return false;
            }

            var cabecera = context.HttpContext.Response.Headers["MostrarHATEOAS"];

            if(cabecera.Count == 0)
            {
                return false;
            }

            var valor = cabecera[0];

            if(!valor.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            return true;
        }

        private bool esRespuestaExitosa(ObjectResult result)
        {
            if(result == null || result.Value == null) 
            { 
                return false;
            }

            if(result.StatusCode.HasValue && !result.StatusCode.Value.ToString().StartsWith("2")) 
            {
                return false;
            }

            return true;
        }

    }
}
