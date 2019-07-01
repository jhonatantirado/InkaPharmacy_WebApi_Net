using System;
using System.Collections.Generic;
using InkaPharmacy.Api.Common.Application;
using InkaPharmacy.Api.Common.Application.Dto;
using InkaPharmacy.Api.Common.Domain.Specification;
using InkaPharmacy.Api.Customers.Application.Assembler;
using InkaPharmacy.Api.Customers.Application.Dto;
using InkaPharmacy.Api.Customers.Domain.Repository;
using InkaPharmacy.Api.Customers.Infrastructure.Persistence.NHibernate.Specification;
using InkaPharmacy.Api.Common.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InkaPharmacy.Api.Common.Application.Enum;

namespace InkaPharmacy.Api.Customers.Controllers
{
    [Authorize]
    [Route("api/Customers")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _customerRepository;
        private readonly CustomerAssembler _customerAssembler;
        ResponseHandler responseHandler;

        public CustomerController(
            IUnitOfWork unitOfWork,
            ICustomerRepository customerRepository,
            CustomerAssembler customerAssembler
            )
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
            _customerAssembler = customerAssembler;
            responseHandler = new ResponseHandler();
        }

        [ProducesResponseType(typeof(GridDto), 200)]
        [HttpGet]
        public IActionResult CustomersPaginated([FromQuery] int page = 0, [FromQuery] int size = 5)
        {
            bool uowStatus = false;
            try
            {
                uowStatus = _unitOfWork.BeginTransaction();
                GridDto customers = _customerRepository.GetListWithPageCounters(page, size);
                _unitOfWork.Commit(uowStatus);
                List<CustomerDto> productsDTO = _customerAssembler.FromListCustomerToListCustomerDto((List<Customer>)customers.Content);
                customers.Content = productsDTO;
                return StatusCode(StatusCodes.Status200OK, customers);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback(uowStatus);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiStringResponseDto(ex.Message));
            }

        }

        [ProducesResponseType(typeof(GridDto), 200)]
        [Route("/api/Customers/LikeSearchByNameAndDocumentNumber")]
        [HttpGet]
        public IActionResult LikeSearchByNameAndDocumentNumberCustomersPaginated([FromQuery] string Name,[FromQuery] string DocumentNumber, [FromQuery] int page = 0, [FromQuery] int size = 5)
        {
            bool uowStatus = false;
            try
            {
                Specification<Customer> specification = LikeSearchByNameAndDocumentNumber(Name,DocumentNumber);
                uowStatus = _unitOfWork.BeginTransaction();
                GridDto customers = _customerRepository.GetListSearchLikeByNameAndDocumentNumberWithPageCounters(specification,page, size);
                _unitOfWork.Commit(uowStatus);
                List<CustomerDto> productsDTO = _customerAssembler.FromListCustomerToListCustomerDto((List<Customer>)customers.Content);
                customers.Content = productsDTO;
                return StatusCode(StatusCodes.Status200OK, customers);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback(uowStatus);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiStringResponseDto(ex.Message));
            }

        }

        [ProducesResponseType(typeof(CustomerDto), 200)]
        [Route("/api/Customers/FindByDocumentNumber")]
        [HttpGet]
        public IActionResult FindByDocumentNumber([FromQuery] string DocumentNumber)
        {
            bool uowStatus = false;
            try
            {
                Customer customer = new Customer();
                Notification notification = customer.validateFindByDocumentNumber(DocumentNumber);

                if (notification.HasErrors())
                {
                    throw new ArgumentException(notification.ErrorMessage());
                }

                Specification<Customer> specification = GetFindByDocumentNumber(DocumentNumber);
                uowStatus = _unitOfWork.BeginTransaction();
                customer = _customerRepository.FindByDocumentNumber(specification);
                _unitOfWork.Commit(uowStatus);
                CustomerDto customerDto = _customerAssembler.FromCustomerToCustomerDto(customer);
                return StatusCode(StatusCodes.Status200OK, customerDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(responseHandler.getAppCustomErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback(uowStatus);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, responseHandler.getAppExceptionResponse());
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] CustomerDto customerDto)
        {
            Notification notification = new Notification();
            bool uowStatus = false;
            try
            {
                uowStatus = _unitOfWork.BeginTransaction();

                Customer customer = _customerAssembler.FromCustomerDtoToCustomer(customerDto);
                notification = customer.ValidateForSave();

                if (notification.HasErrors())
                {
                    logger.Message(notification.ErrorMessage(),LogLevel.FunctionalError);
                    return BadRequest(responseHandler.getAppCustomErrorResponse(notification.ErrorMessage()));
                }

                Specification<Customer> specification = GetUniqueCustomer(customer.Document_Number);
                var uniqueCustomer = _customerRepository.GetUniqueCustomer(specification);

                if (uniqueCustomer != null)
                {
                    notification.AddError("Customer already registered");
                    return BadRequest(responseHandler.getAppCustomErrorResponse(notification.ErrorMessage()));
                }

                customer.Status = 1;
                _customerRepository.Create(customer);
                _unitOfWork.Commit(uowStatus);

                var message = "Customer " + customer.Id + " created!";
                KipubitRabbitMQ.SendMessage(message);
                logger.Message(message, LogLevel.FunctionalMessage);

                return Ok(responseHandler.getOkCommandResponse(message, StatusCodes.Status201Created));
            }
            catch (ArgumentException ex)
            {
                logger.Message(ex.Message, LogLevel.Error);
                return BadRequest(responseHandler.getAppCustomErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback(uowStatus);
                Console.WriteLine(ex.StackTrace);
                logger.Message(ex.StackTrace, LogLevel.Debug);
                return StatusCode(StatusCodes.Status500InternalServerError, responseHandler.getAppExceptionResponse());
            }
        }

        [HttpPut("{CustomerId}")]
        public IActionResult Update(int CustomerId, [FromBody] CustomerDto CustomerDto)
        {
            Notification notification = new Notification();

            if (CustomerId == 0)
            {
                notification.AddError("CustomerId is missing");
                return BadRequest(responseHandler.getAppCustomErrorResponse(notification.ErrorMessage()));
            }

            bool uowStatus = false;
            try
            {

                Customer customer = new Customer();
                uowStatus = _unitOfWork.BeginTransaction();
                customer = _customerAssembler.FromCustomerDtoToCustomer(CustomerDto);
                customer.Id = CustomerId;
                notification = customer.ValidateForSave("U");

                ThrowErrors(notification);

                if (notification.HasErrors())
                {
                    return BadRequest(responseHandler.getAppCustomErrorResponse(notification.ErrorMessage()));
                }

                Specification<Customer> specification = GetUniqueCustomerForUpdate(customer.Document_Number, customer.Id);
                var uniqueCustomer = _customerRepository.GetUniqueCustomer(specification);

                if (uniqueCustomer != null)
                {
                    notification.AddError("Other customer already have the same Document Number");
                    return BadRequest(responseHandler.getAppCustomErrorResponse(notification.ErrorMessage()));
                }

                _customerRepository.Update(customer);
                _unitOfWork.Commit(uowStatus);

                var message = "Customer " + CustomerId + " updated!";
                return Ok(responseHandler.getOkCommandResponse(message, StatusCodes.Status200OK));

            }
            catch (ArgumentException ex)
            {
                return BadRequest(responseHandler.getAppCustomErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback(uowStatus);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, responseHandler.getAppExceptionResponse());
            }
        }

        [HttpDelete("{CustomerId}")]
        public IActionResult Delete(int CustomerId)
        {
            Notification notification = new Notification();

            if (CustomerId == 0)
            {
                notification.AddError("CustomerId is missing");
                return BadRequest(responseHandler.getAppCustomErrorResponse(notification.ErrorMessage()));
            }

            bool uowStatus = false;
            try
            {
                Customer customer = new Customer();
                uowStatus = _unitOfWork.BeginTransaction();
                Specification<Customer> specification = GetById(CustomerId);
                customer = _customerRepository.GetById(specification);

                if (customer == null)
                {
                    notification.AddError("Customer not found");
                    return BadRequest(responseHandler.getAppCustomErrorResponse(notification.ErrorMessage()));

                }

                customer.Status = 0;
                _customerRepository.Update(customer);
                _unitOfWork.Commit(uowStatus);

                var message = "Customer " + CustomerId + " deleted!";
                return Ok(responseHandler.getOkCommandResponse(message, StatusCodes.Status200OK));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(responseHandler.getAppCustomErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback(uowStatus);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, responseHandler.getAppExceptionResponse());
            }
        }

        [ProducesResponseType(typeof(CustomerDto), 200)]
        [HttpGet("{CustomerId}")]
        public IActionResult GetCustomerById(long CustomerId)
        {
            bool uowStatus = false;
            try
            {
                Customer customer = new Customer();
                Notification notification = new Notification();

                if (CustomerId == 0)
                {
                    notification.AddError("CustomerId is missing");
                    return BadRequest(notification.ErrorMessage());
                }

                Specification<Customer> specification = GetById(CustomerId);

                uowStatus = _unitOfWork.BeginTransaction();
                customer = _customerRepository.GetById(specification);
                _unitOfWork.Commit(uowStatus);

                CustomerDto customerDto = _customerAssembler.FromCustomerToCustomerDto(customer);
                return StatusCode(StatusCodes.Status200OK, customerDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(responseHandler.getAppCustomErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback(uowStatus);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, responseHandler.getAppExceptionResponse());
            }
        }

        private Specification<Customer> GetById(long CustomerId)
        {
            Specification<Customer> specification = Specification<Customer>.All;
            specification = specification.And(new GetByIdBySpecification(CustomerId));
            return specification;
        }
        private Specification<Customer> GetFindByDocumentNumber(string DocumentNumber)
        {
            Specification<Customer> specification = Specification<Customer>.All;
            specification = specification.And(new FindByDocumentNumberBySpecification(DocumentNumber));
            return specification;
        }

        private Specification<Customer> LikeSearchByNameAndDocumentNumber(string Name,string DocumentNumber)
        {
            Specification<Customer> specification = Specification<Customer>.All;

            if (!string.IsNullOrEmpty(Name)) {
                specification = specification.And(new LikeSearchByNameSpecification(Name));
            }

            if (!string.IsNullOrEmpty(DocumentNumber))
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    specification = specification.Or(new LikeSearchByDocumentNumberSpecification(DocumentNumber));
                }

                if (string.IsNullOrEmpty(Name))
                {
                    specification = specification.And(new LikeSearchByDocumentNumberSpecification(DocumentNumber));
                }

            }

            return specification;
        }

        private Specification<Customer> GetUniqueCustomer(string DocumentNumber)
        {
            Specification<Customer> specification = Specification<Customer>.All;

            if (!string.IsNullOrEmpty(DocumentNumber))
            {
                specification = specification.And(new GetUniqueCustomer(DocumentNumber));
            }

            return specification;
        }

        private Specification<Customer> GetUniqueCustomerForUpdate(string DocumentNumber, long Id)
        {
            Specification<Customer> specification = Specification<Customer>.All;

            if (!string.IsNullOrEmpty(DocumentNumber))
            {
                specification = specification.And(new GetUniqueCustomerForUpdate(DocumentNumber, Id));
            }

            return specification;
        }

    }
}