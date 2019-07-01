using InkaPharmacy.Api.Common.Domain.Specification;
using System;
using System.Linq.Expressions;

namespace InkaPharmacy.Api.Customers.Infrastructure.Persistence.NHibernate.Specification
{
    public class GetUniqueCustomerForUpdate : Specification<Customer>
    {
        private readonly string _DocumentNumber;
        private readonly long _Id;

        public GetUniqueCustomerForUpdate(string DocumentNumber, long Id)
        {
            _DocumentNumber = DocumentNumber;
            _Id = Id;
        }

        public override Expression<Func<Customer, bool>> ToExpression()
        {
            return customer => (customer.Document_Number.Equals(_DocumentNumber) && !customer.Id.Equals(_Id));
        }
    }
}
