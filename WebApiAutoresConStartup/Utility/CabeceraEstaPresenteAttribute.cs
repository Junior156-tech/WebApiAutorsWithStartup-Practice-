using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace WebApiAutoresConStartup.Utility
{
    public class CabeceraEstaPresenteAttribute : Attribute, IActionConstraint
    {
        public int Order => 0;
        public string Cabecera { get; }
        public string Valor { get; }

        public CabeceraEstaPresenteAttribute(string cabecera, string valor)
        {
            Cabecera = cabecera;
            Valor = valor;
        }

        public bool Accept(ActionConstraintContext context)
        {
            var cabeceras = context.RouteContext.HttpContext.Response.Headers;

            if (!cabeceras.ContainsKey(Cabecera))
            {
                return false;
            }

            return string.Equals(cabeceras[Cabecera], Valor, StringComparison.OrdinalIgnoreCase);

        }
    }
}
