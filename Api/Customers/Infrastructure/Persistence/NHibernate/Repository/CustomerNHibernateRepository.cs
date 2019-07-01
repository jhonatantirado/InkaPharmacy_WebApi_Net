using System;
using System.Collections.Generic;
using System.Linq;
using InkaPharmacy.Api.Common.Application.Dto;
using InkaPharmacy.Api.Common.Domain.Specification;
using InkaPharmacy.Api.Common.Infrastructure.Persistence.NHibernate;
using InkaPharmacy.Api.Customers.Domain.Repository;

namespace InkaPharmacy.Api.Customers.Infrastructure.Persistence.NHibernate.Repository
{
    class CustomerCustomerNHibernateRepository : BaseNHibernateRepository<Customer>, ICustomerRepository
    {
        public CustomerCustomerNHibernateRepository(UnitOfWorkNHibernate unitOfWork) : base(unitOfWork)
        {
        }

        public Customer FindByDocumentNumber(Specification<Customer> specification)
        {
            Customer customer = new Customer();
            bool uowStatus = false;
            try
            {
                uowStatus = _unitOfWork.BeginTransaction();
                customer = _unitOfWork.GetSession().Query<Customer>()
                .Where(specification.ToExpression()).FirstOrDefault();
                _unitOfWork.Commit(uowStatus);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback(uowStatus);
                throw ex;
            }
            return customer;
        }

        public Customer GetById(Specification<Customer> specification)
        {
            Customer customer = new Customer();
            bool uowStatus = false;
            try
            {
                uowStatus = _unitOfWork.BeginTransaction();
                customer = _unitOfWork.GetSession().Query<Customer>()
                .Where(specification.ToExpression()).FirstOrDefault();
                _unitOfWork.Commit(uowStatus);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback(uowStatus);
                throw ex;
            }
            return customer;
        }

        public List<Customer> GetList(int page = 0, int pageSize = 5)
        {
            List<Customer> customers = new List<Customer>();
            bool uowStatus = false;
            try
            {
                uowStatus = _unitOfWork.BeginTransaction();
                customers = _unitOfWork.GetSession().Query<Customer>()
                        .Skip(page * pageSize)
                        .Take(pageSize)
                        .ToList();
                _unitOfWork.Commit(uowStatus);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback(uowStatus);
                throw ex;
            }
            return customers;
        }

        public GridDto GetListWithPageCounters(int page = 0, int pageSize = 5)
        {
            List<Customer> customers = new List<Customer>();
            GridDto result = new GridDto();
            bool uowStatus = false;
            try
            {
                uowStatus = _unitOfWork.BeginTransaction();
                customers = _unitOfWork.GetSession().Query<Customer>()
                        .Skip(page * pageSize)
                        .Take(pageSize)
                        .ToList();
                _unitOfWork.Commit(uowStatus);
                result = new GridDto
                {
                    Content = customers,
                    TotalRecords = CountTotalRecords(),
                    CurrentPage = page,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback(uowStatus);
                throw ex;
            }
            return result;
        }


        public GridDto GetListSearchLikeByNameAndDocumentNumberWithPageCounters(Specification<Customer> specification,int page = 0, int pageSize = 5)
        {
            List<Customer> customers = new List<Customer>();
            GridDto result = new GridDto();
            bool uowStatus = false;
            try
            {
                uowStatus = _unitOfWork.BeginTransaction();
                customers = _unitOfWork.GetSession().Query<Customer>()
                        .Where(specification.ToExpression())    
                        .Skip(page * pageSize)
                        .Take(pageSize)
                        .ToList();
                _unitOfWork.Commit(uowStatus);
                result = new GridDto
                {
                    Content = customers,
                    TotalRecords = CountTotalRecordsSearchLikeByNameAndDocumentNumber(specification),
                    CurrentPage = page,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback(uowStatus);
                throw ex;
            }
            return result;
        }

        public int CountTotalRecordsSearchLikeByNameAndDocumentNumber(Specification<Customer> specification)
        {
            int totalRecords;
            bool uowStatus = false;
            try
            {
                uowStatus = _unitOfWork.BeginTransaction();
                totalRecords = _unitOfWork.GetSession().Query<Customer>().Where(specification.ToExpression()).ToList().Count();
                _unitOfWork.Commit(uowStatus);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback(uowStatus);
                throw ex;
            }
            return totalRecords;
        }


        public Customer GetUniqueCustomer(Specification<Customer> specification)
        {
            Customer customer = new Customer();
            bool uowStatus = false;
            try
            {
                uowStatus = _unitOfWork.BeginTransaction();
                customer = _unitOfWork.GetSession().Query<Customer>()
                .Where(specification.ToExpression()).FirstOrDefault();
                _unitOfWork.Commit(uowStatus);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback(uowStatus);
                throw ex;
            }
            return customer;
        }

        public Customer GetUniqueCustomerForUpdate(Specification<Customer> specification)
        {
            Customer customer = new Customer();
            bool uowStatus = false;
            try
            {
                uowStatus = _unitOfWork.BeginTransaction();
                customer = _unitOfWork.GetSession().Query<Customer>()
                .Where(specification.ToExpression()).FirstOrDefault();
                _unitOfWork.Commit(uowStatus);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback(uowStatus);
                throw ex;
            }
            return customer;
        }


    }
}
