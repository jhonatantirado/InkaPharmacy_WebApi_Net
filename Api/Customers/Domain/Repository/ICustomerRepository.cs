using InkaPharmacy.Api.Common.Application.Dto;
using InkaPharmacy.Api.Common.Domain.Specification;
using System.Collections.Generic;

namespace InkaPharmacy.Api.Customers.Domain.Repository
{
    public interface ICustomerRepository
    {
        Customer FindByDocumentNumber( Specification<Customer> specification);

        List<Customer> GetList(int page = 0,int pageSize = 5);

        void Create(Customer customer);

        void Update(Customer customer);

        Customer GetById(Specification<Customer> specification);
        GridDto GetListWithPageCounters(int page, int size);

        GridDto GetListSearchLikeByNameAndDocumentNumberWithPageCounters(Specification<Customer> specification,int page, int size);

        Customer GetUniqueCustomer(Specification<Customer> specification);

        Customer GetUniqueCustomerForUpdate(Specification<Customer> specification);
    }
}
