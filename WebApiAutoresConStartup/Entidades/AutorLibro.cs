namespace WebApiAutoresConStartup.Entidades
{
    public class AutorLibro
    {
        public int libroId { get; set; }
        public int AutorId { get; set; }
        public int Orden { get; set; }
        public Libros libros { get; set; }
        public Autor autor { get; set; }
    }
}
