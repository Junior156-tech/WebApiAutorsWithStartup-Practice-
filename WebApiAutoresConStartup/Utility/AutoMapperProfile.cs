using AutoMapper;
using WebApiAutoresConStartup.DTOs;
using WebApiAutoresConStartup.Entidades;

namespace WebApiAutoresConStartup.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<Autor, AutorDTO>();
            CreateMap<Autor, AutorDTOConLibros>()
                .ForMember(autorDTO => autorDTO.libros, opciones => opciones.MapFrom(MapAutorDTOLibros));

            CreateMap<LibrosCreacionDTO, Libros>()
                .ForMember(libro => libro.autorLibro, opciones => opciones.MapFrom(MapAutoresLibros));
            CreateMap<Libros, LibrosDTO>();
            CreateMap<Libros, LibrosDTOConAutores>()
                .ForMember(LibrosDTO => LibrosDTO.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores));
            CreateMap<LibrosPatchDTO, Libros>().ReverseMap();

            CreateMap<ComentarioCreacionDTO, Comentario>();
            CreateMap<Comentario, ComentarioDTO>();
        }

        private List<LibrosDTO> MapAutorDTOLibros(Autor autor, AutorDTO autorDTO)
        {
            var resultado = new List<LibrosDTO>(); 

            if(autor.autorLibro == null) { return resultado; }

            foreach (var autorLibro in autor.autorLibro)
            {
                resultado.Add(new LibrosDTO()
                {
                    id = autorLibro.libroId,
                    Titulo = autorLibro.libros.Titulo
                });
            }

            return resultado;
        }

        private List<AutorDTO> MapLibroDTOAutores(Libros libro, LibrosDTO librosDTO)
        {
            var resultado = new List<AutorDTO>();

            if(libro.autorLibro == null) { return resultado; }

            foreach (var autorLibro in libro.autorLibro)
            {
                resultado.Add(new AutorDTO()
                {
                    Id = autorLibro.AutorId,
                    name = autorLibro.autor.name
                });
            }

            return resultado;
        }

        private List<AutorLibro> MapAutoresLibros(LibrosCreacionDTO librosCreacionDTO, Libros libro)
        {
            var resultado = new List<AutorLibro>();

            if (librosCreacionDTO.AutoresIds == null) { return resultado; }

            foreach (var autorId in librosCreacionDTO.AutoresIds)
            {
                resultado.Add(new AutorLibro() { AutorId = autorId });
            }

            return resultado;

        }


    }
}
