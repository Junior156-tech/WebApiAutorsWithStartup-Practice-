namespace WebApiAutoresConStartup.DTOs
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; }
        public int recordsPorPagina { get; set; }
        public int CantidadMaximaPorPagina { get; set; }

        public int RecordsPorPagina
        {
            get
            {
                return recordsPorPagina;
            }
            set
            {
                recordsPorPagina = (value > CantidadMaximaPorPagina) ? CantidadMaximaPorPagina : value;
            }
        }
    }
}
