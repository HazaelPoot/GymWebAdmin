using AutoMapper;
using GymApi.Domain.Dtos;
using GymApi.Domain.Entities;

namespace GymApi.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Gimnasios
            CreateMap<Gimnasio, DtoGimnasio>().ReverseMap();
            #endregion

            #region Login
            CreateMap<Gimnasio, DtoLogin>().ReverseMap();
            #endregion

            #region Usuario
            CreateMap<Usuario, DtoUsuario>().ReverseMap();
            #endregion

            #region Servicios
            CreateMap<Servicio, DtoServicio>()
                .ForMember(d =>
                    d.NombreGimnasio,
                    opt => opt.MapFrom(o => o.IdGimnasioNavigation.Nombre)
                );

            CreateMap<DtoServicio, Servicio>()
                .ForMember(d =>
                    d.IdGimnasioNavigation,
                    opt => opt.Ignore()
                );
            #endregion

            #region Horarios
            CreateMap<Horario, DtoHorario>()
                .ForMember(d =>
                    d.NombreGimnasio,
                    opt => opt.MapFrom(o => o.IdGimnasioNavigation.Nombre)
                );

            CreateMap<DtoHorario, Horario>()
                .ForMember(d =>
                    d.IdGimnasioNavigation,
                    opt => opt.Ignore()
                );
            #endregion

            #region Miembros
            CreateMap<Miembro, DtoMiembro>()
                .ForMember(d => d.Nombre, opt => opt.MapFrom(o => o.IdUsuarioNavigation.Nombre))
                .ForMember(d => d.Apellido, opt => opt.MapFrom(o => o.IdUsuarioNavigation.Apellido))
                .ForMember(d => d.Contacto, opt => opt.MapFrom(o => o.IdUsuarioNavigation.Contacto))
                .ForMember(d => d.FechaInscripcion, opt => opt.MapFrom(o => o.IdUsuarioNavigation.FechaInscripcion))
                .ForMember(d => d.NombreGimnasio, opt => opt.MapFrom(o => o.IdGimnasioNavigation.Nombre));

            CreateMap<DtoMiembro, Miembro>()
                .ForMember(d =>
                    d.IdUsuarioNavigation,
                    opt => opt.Ignore()
                );
            #endregion

            #region Suscripciones
            CreateMap<Suscripcion, DtoSuscripcion>()
                .ForMember(d => d.Nombre, opt => opt.MapFrom(o => o.IdMiembroNavigation.IdUsuarioNavigation.Nombre))
                .ForMember(d => d.Apellido, opt => opt.MapFrom(o => o.IdMiembroNavigation.IdUsuarioNavigation.Apellido))
                .ForMember(d => d.NombreServicio, opt => opt.MapFrom(o => o.IdServicioNavigation.Nombre))
                .ForMember(d => d.MontoPagar, opt => opt.MapFrom(o => o.IdServicioNavigation.Costo));

            CreateMap<DtoSuscripcion, Suscripcion>()
                .ForMember(d => d.IdMiembroNavigation, opt => opt.Ignore())
                .ForMember(d => d.IdServicioNavigation, opt => opt.Ignore());
            #endregion
        }
    }
}