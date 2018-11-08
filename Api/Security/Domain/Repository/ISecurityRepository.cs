﻿namespace InkaPharmacy.Api.Security.Domain.Repository
{
    using InkaPharmacy.Api.Common.Domain.Specification;
    using InkaPharmacy.Api.Employee.Domain.Entity;
    using System.Collections.Generic;

    public interface ISecurityRepository
    {
        //void Create(Employee empleado);
        //void Delete(Employee empleado);
        //Employee Read(int id);

        List<Employee> GetList(
         Specification<Employee> specification);

    }
}
