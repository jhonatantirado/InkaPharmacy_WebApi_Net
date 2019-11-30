using System;
using System.Collections.Generic;
using InkaPharmacy.Api.Common.Application;
using InkaPharmacy.Api.Common.Domain.Specification;
using InkaPharmacy.Api.Products.Application.Assembler;
using InkaPharmacy.Api.Product.Application.Dto;
using InkaPharmacy.Api.Product.Domain.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InkaPharmacy.Api.Product.Infrastructure.Persistence.NHibernate.Specification;
using Microsoft.AspNetCore.Authorization;
using InkaPharmacy.Api.Common.Application.Dto;

namespace InkaPharmacy.Api.Product.Controllers
{
   
    [Route("api/Products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _ProductRepository;
        private readonly ProductAssembler _ProductAssembler;
        ResponseHandler responseHandler;

        public ProductController(
            IUnitOfWork unitOfWork,
            IProductRepository ProductRepository,
            ProductAssembler ProductAssembler
            )
        {
            _unitOfWork = unitOfWork;
            _ProductRepository = ProductRepository;
            _ProductAssembler = ProductAssembler;
            responseHandler = new ResponseHandler();
        }

        [ProducesResponseType(typeof(ProductDto), 200)]
        [Route("/api/Products/FindByName")]
        [HttpGet]
        public IActionResult FindByProductName([FromQuery] string ProductName)
        {
            bool uowStatus = false;
            try
            {
                Product product = new Product();
                Notification notification = product.ValidateFindByProductName(ProductName);

                if (notification.HasErrors())
                {
                    return BadRequest(notification.ErrorMessage());
                }

                Specification<Product> specification = GetFindByName(ProductName);

                uowStatus = _unitOfWork.BeginTransaction();
                Product Product = _ProductRepository.FindByProductName(specification);
                _unitOfWork.Commit(uowStatus);

                ProductDto ProductDto = _ProductAssembler.FromProductToProductDto(Product);

                return StatusCode(StatusCodes.Status200OK, ProductDto);
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

        private Specification<Product> GetById(long ProductId)
        {
            Specification<Product> specification = Specification<Product>.All;
            specification = specification.And(new GetByIdBySpecification(ProductId));
            return specification;
        }

        private Specification<Product> GetFindByName(string ProductName)
        {
            Specification<Product> specification = Specification<Product>.All;
            specification = specification.And(new FindByNameBySpecification(ProductName));
            return specification;
        }

        private Specification<Product> GetLikeSearchByName(string ProductName)
        {
            Specification<Product> specification = Specification<Product>.All;
            specification = specification.And(new LikeSearchByNameSpecification(ProductName));
            return specification;
        }

        private Specification<Product> GetFindByCategory(long Category_id)
        {
            Specification<Product> specification = Specification<Product>.All;
            specification = specification.And(new FindByCategoryBySpecification(Category_id));
            return specification;
        }

        [ProducesResponseType(typeof(GridDto), 200)]
        [HttpGet]
        public IActionResult ProductsPaginated([FromQuery] int page = 0, [FromQuery] int size = 5)
        {
            bool uowStatus = false;
            try
            {
                uowStatus = _unitOfWork.BeginTransaction();
                GridDto products = _ProductRepository.GetListWithPageCounters(page, size);
                _unitOfWork.Commit(uowStatus);
                List<ProductDto> productsDTO = _ProductAssembler.FromListProductToListProductDto((List<Product>)products.Content);
                products.Content = productsDTO;
                return StatusCode(StatusCodes.Status200OK, products);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback(uowStatus);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiStringResponseDto(ex.Message));
            }

        }

        [ProducesResponseType(typeof(GridDto), 200)]
        [Route("/api/Products/LikeSearchByName")]
        [HttpGet]
        public IActionResult LikeSearchProductsPaginated([FromQuery] string productName, [FromQuery] int page = 0, [FromQuery] int size = 5)
        {
            bool uowStatus = false;
            try
            {
                uowStatus = _unitOfWork.BeginTransaction();
                Specification<Product> specification = GetLikeSearchByName(productName);
                GridDto products = _ProductRepository.GetListLikeSearchWithPageCounters(specification, page, size);
                _unitOfWork.Commit(uowStatus);
                List<ProductDto> productsDTO = _ProductAssembler.FromListProductToListProductDto((List<Product>)products.Content);
                products.Content = productsDTO;
                return StatusCode(StatusCodes.Status200OK, products);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback(uowStatus);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiStringResponseDto(ex.Message));
            }

        }


        [ProducesResponseType(typeof(ProductDto), 200)]
        [HttpGet("{ProductId}")]
        public IActionResult GetProductById(long ProductId)
        {
            bool uowStatus = false;
            try
            {
                Product product = new Product();
                Notification notification = product.ValidateGetProductById(ProductId);

                if (notification.HasErrors())
                {
                    return BadRequest(notification.ErrorMessage());
                }

                Specification<Product> specification = GetById(ProductId);

                uowStatus = _unitOfWork.BeginTransaction();
                product = _ProductRepository.GetById(specification);
                _unitOfWork.Commit(uowStatus);

                ProductDto ProductDto = _ProductAssembler.FromProductToProductDto(product);
                return StatusCode(StatusCodes.Status200OK, ProductDto);
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

        [Authorize]
        [HttpPost]
        public IActionResult Create([FromBody] ProductDto ProductDto)
        {
            Notification notification = new Notification();
            bool uowStatus = false;
            try
            {
                uowStatus = _unitOfWork.BeginTransaction();

                Product product = _ProductAssembler.FromProductDtoToProduct(ProductDto);
                notification = product.ValidateForSave();

                if (notification.HasErrors())
                {
                    return BadRequest(responseHandler.getAppCustomErrorResponse(notification.ErrorMessage()));
                }

                product.Status = 1;
                _ProductRepository.Create(product);
                _unitOfWork.Commit(uowStatus);

                var message = "Product " + product.Id + " created!";
                var messageForRabbitMQ = message + "---" + ProductDto.FirebaseClientKey;
                KipubitRabbitMQ.SendMessage(messageForRabbitMQ);
                return Ok(responseHandler.getOkCommandResponse(message, StatusCodes.Status201Created));
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

        [Authorize]
        [HttpPut("{ProductId}")]
        public IActionResult Update(int ProductId, [FromBody] ProductDto ProductDto)
        {
            Notification notification = new Notification();

            if (ProductId == 0)
            {
                notification.AddError("ProductId is missing");
                return BadRequest(responseHandler.getAppCustomErrorResponse(notification.ErrorMessage()));
            }

            bool uowStatus = false;
            try
            {
                uowStatus = _unitOfWork.BeginTransaction();

                Product product = _ProductAssembler.FromProductDtoToProduct(ProductDto);
                product.Id = ProductId;
                notification = product.ValidateForSave("U");

                if (notification.HasErrors())
                {
                    return BadRequest(responseHandler.getAppCustomErrorResponse(notification.ErrorMessage()));
                }
                _ProductRepository.Update(product);
                _unitOfWork.Commit(uowStatus);

                var message = "Product " + ProductId + " updated!";
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

        [Authorize]
        [HttpDelete("{ProductId}")]
        public IActionResult Delete(int ProductId)
        {
            Notification notification = new Notification();

            if (ProductId == 0)
            {
                notification.AddError("ProductId is missing");
                return BadRequest(responseHandler.getAppCustomErrorResponse(notification.ErrorMessage()));
            }

            bool uowStatus = false;
            try
            {
                Product product = new Product();
                uowStatus = _unitOfWork.BeginTransaction();
                Specification<Product> specification = GetById(ProductId);
                product = _ProductRepository.GetById(specification);

                if (product == null)
                {
                    notification.AddError("Product not found");
                    return BadRequest(responseHandler.getAppCustomErrorResponse(notification.ErrorMessage()));
                }

                product.Status = 0;
                _ProductRepository.Update(product);
                _unitOfWork.Commit(uowStatus);

                var message = "Product " + ProductId + " deleted!";
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

        [ProducesResponseType(typeof(List<ProductDto>), 200)]
        [Route("/api/Products/FindByCategory")]
        [HttpGet]
        public IActionResult FindByCategory([FromQuery] long Category_id, [FromQuery] int page = 0, [FromQuery] int size = 5)
        {
            bool uowStatus = false;
            try
            {
                Product product = new Product();
                Notification notification = product.ValidateFindByCategory(Category_id);

                if (notification.HasErrors())
                {
                    return BadRequest(notification.ErrorMessage());
                }

                Specification<Product> specification = GetFindByCategory(Category_id);

                uowStatus = _unitOfWork.BeginTransaction();
                List<Product> products = _ProductRepository.GetListFindByCategory(specification, page, size);
                _unitOfWork.Commit(uowStatus);
                List<ProductDto> productsDto = _ProductAssembler.FromListProductToListProductDto(products);
                return StatusCode(StatusCodes.Status200OK, productsDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(responseHandler.getAppCustomErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback(uowStatus);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiStringResponseDto(ex.Message));
            }
        }
    }
}