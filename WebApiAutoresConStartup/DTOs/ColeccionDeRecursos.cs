namespace WebApiAutoresConStartup.DTOs
{
    public class ColeccionDeRecursos<T>: Recurso where T : Recurso
    {
        public List<T> valores { get; set; }
    }
}
