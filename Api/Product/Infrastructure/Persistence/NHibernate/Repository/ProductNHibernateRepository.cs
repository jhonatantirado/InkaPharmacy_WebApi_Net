﻿using System;
using System.Collections.Generic;
using System.Linq;
using InkaPhatmacy.Api.Common.Domain.Specification;
using InkaPhatmacy.Api.Common.Infrastructure.Persistence.NHibernate;
using InkaPhatmacy.Api.Customers.Domain.Repository;

namespace InkaPhatmacy.Api.Customers.Infrastructure.Persistence.NHibernate.Repository
{
    using InkaPhatmacy.Api.Product;
    using InkaPhatmacy.Api.Product.Domain.Repository;

    class ProductNHibernateRepository : BaseNHibernateRepository<Product>, IProductRepository
    {
        public ProductNHibernateRepository(UnitOfWorkNHibernate unitOfWork) : base(unitOfWork)
        {
        }

        public Product FindByProductName(Specification<Product> specification)
        {
            Product customer = new Product();
            bool uowStatus = false;
            try
            {
                uowStatus = _unitOfWork.BeginTransaction();
                customer = _unitOfWork.GetSession().Query<Product>()
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

        public Product GetById(Specification<Product> specification)
        {
            Product customer = new Product();
            bool uowStatus = false;
            try
            {
                uowStatus = _unitOfWork.BeginTransaction();
                customer = _unitOfWork.GetSession().Query<Product>()
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

        public List<Product> GetList(int page = 0, int pageSize = 5)
        {
            List<Product> products = new List<Product>();
            bool uowStatus = false;
            try
            {
                uowStatus = _unitOfWork.BeginTransaction();
                products = _unitOfWork.GetSession().Query<Product>()
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
            return products;
        }

        public List<Product> GetListFindByCategory(Specification<Product> specification, int page = 0, int pageSize = 5)
        {
            List<Product> products = new List<Product>();
            bool uowStatus = false;
            try
            {
                       //.Where(specification.ToExpression())

                uowStatus = _unitOfWork.BeginTransaction();
                products = _unitOfWork.GetSession().Query<Product>()
                       .Where(specification.ToExpression())
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
            return products;
        }
    }
}
