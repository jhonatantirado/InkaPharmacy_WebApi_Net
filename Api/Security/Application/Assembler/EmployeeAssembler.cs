﻿using AutoMapper;

namespace InkaPhatmacy.Api.Security.Application.Assembler
{
    using InkaPhatmacy.Api.Empleado.Application.Dto;
    using InkaPhatmacy.Api.Empleado.Domain.Entity;
    using System.Collections.Generic;

    public class EmployeeAssembler
    {
        private readonly IMapper _mapper;

        public EmployeeAssembler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Employee toEntity(EmployeeDto empleadoLoginDto)
        {
            return _mapper.Map<Employee>(empleadoLoginDto);
        }

        public EmployeeDto toDto(Employee empleado)
        {
            return _mapper.Map<EmployeeDto>(empleado);
        }

        public List<EmployeeDto> toDtoList(List<Employee> movieList)
        {
            return _mapper.Map<List<Employee>, List<EmployeeDto>>(movieList);
        }

    }
}
