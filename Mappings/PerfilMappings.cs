using AutoMapper;
using Unach.Codesi.Encuestas.Aplicacion.DTOs;
using Unach.Codesi.Encuestas.Persistencia.Core.Models;

namespace Unach.Codesi.Encuestas.Api.Mappings
{
    public class PerfilMappings : Profile
    {
        public PerfilMappings()
        {
            CreateMap<Encuesta, EntidadRegistroEncuesta>();
            CreateMap<EntidadRegistroEncuesta, Encuesta>();
            CreateMap<Resultado, EntidadRegistroResultado>();
            CreateMap<EntidadRegistroResultado, Resultado>();
            CreateMap<Usuario, EntidadRegistroUsuario>();
            CreateMap<EntidadRegistroUsuario, Usuario>();
        }
    }
}
