﻿using AutoMapper;
using InkaPharmacy.Api.Common.Domain.ValueObject;
using InkaPharmacy.Api.Customers;

namespace InkaPharmacy.Api.Security.Application.Assembler
{
    using InkaPharmacy.Api.Employees.Domain.Entity;
    using InkaPharmacy.Api.Employees.Application.Dto;

    public class EmployeeProfile : Profile
    {

        public EmployeeProfile()
        {

            CreateMap<Employee, EmployeeDto>()

                /*  CreateMap<EmpleadoLoginDto, Employee>()
                      .ForMember(
                          dest => dest.ci,
                          x => x.MapFrom(src => src.ci)
                      )
                      .ForMember(
                          dest => dest.nombre,
                          x => x.MapFrom(src => src.nombre)
                      )
                     .ForMember(
                          dest => dest.paterno,
                          x => x.MapFrom(src => src.paterno)
                      )
                   .ForMember(
                          dest => dest.materno,
                          x => x.MapFrom(src => src.materno)
                      )
                       .ForMember(
                          dest => dest.direccion,
                          x => x.MapFrom(src => src.direccion)
                      )
                      .ForMember(
                          dest => dest.telefono,
                          x => x.MapFrom(src => src.telefono)
                      )
                         .ForMember(
                          dest => dest.telefono,
                          x => x.MapFrom(src => src.telefono)
                      )
                           .ForMember(
                          dest => dest.id_tienda/* tienda,
                          x => x.MapFrom(src => src.id_tienda)
                      )
                           .ForMember(
                          dest => dest.id_perfil /*perfil,
                          x => x.MapFrom(src => src.id_perfil)
                      )
                             .ForMember(
                          dest => dest.usu,
                          x => x.MapFrom(src => src.usu)
                      )
                           .ForMember(
                          dest => dest.clave,
                          x => x.MapFrom(src => src.clave)
                      )
                          .ForMember(
                          dest => dest.correo,
                          x => x.MapFrom(src => src.correo)
                      )
                       .ForMember(
                          dest => dest.estado,
                          x => x.MapFrom(src => src.estado)
                      )*/ ;
        }
    }
}
