using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using onine.core.Constants;
using onine.core.Entites;
using onine.core.Repositories;
using onine.core.Specifications;
using onine.Repository.Data;
using onineproject.DTOs;
using onineproject.Errors;
using onineproject.Helpers;
using Stripe.Checkout;
using System.Linq;
using System.Net;

namespace onineproject.Controllers
{

    public class ProductController : ApiBaseController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IGenericRepository<ProductBrand> _brandRepo;
        private readonly Oninecontext _Context;

        public ProductController(
            IGenericRepository<Product> productRepo,
            IMapper mapper,
            IGenericRepository<ProductType> typeRepo,
            IGenericRepository<ProductBrand> brandRepo,
            Oninecontext context)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _typeRepo = typeRepo;
            _brandRepo = brandRepo;
            _Context = context;
        }
        //GET ALL
        // [Authorize (AuthenticationSchemes= JwtBearerDefaults.AuthenticationScheme )]

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDTOs>>> GetProducts([FromQuery] ProductSpecParams parameters)
        {
            var spec = new ProductwithBrandandType(parameters);
            var products = await _productRepo.GetAllwithspecAsync(spec);
            var mappedProducts =
                               _mapper.Map<IReadOnlyList<ProductToReturnDTOs>>(products);

            var countSpec = new ProductWithFilterationForCount(parameters);
            var count = await _productRepo.GetCountWithSpecAsync(countSpec);

            return Ok(new Pagination<ProductToReturnDTOs>(
                parameters.PageIndex, parameters.PageSize, count, mappedProducts
                ));

        }
        //GET BY ID
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDTOs), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProductToReturnDTOs), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetProducts(int id)
        {
            var spec = new ProductwithBrandandType(id);
            var product = await _productRepo.GetByIdwithspecAsync(spec);
            var mappedProduct = _mapper.Map<ProductToReturnDTOs>(product);

            return Ok(mappedProduct);

        }
        //Get all types

        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var types = await _typeRepo.GetAllwithspecAsync();

            return Ok(types);
        }
        //Get all brands

        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _brandRepo.GetAllwithspecAsync();

            return Ok(brands);
        }


        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost]
        public async Task<ActionResult<ProductToReturnDTOs>> AddProduct(
    [FromBody] CreateProductDto model)
        {
            var product = _mapper.Map<Product>(model);

            await _productRepo.AddAsync(product);

            await _productRepo.SaveChangesAsync();

            var spec = new ProductwithBrandandType(product.Id);

            var productWithDetails =
                await _productRepo.GetByIdwithspecAsync(spec);

            var mappedProduct =
                _mapper.Map<ProductToReturnDTOs>(productWithDetails);

            return CreatedAtAction(
                nameof(GetProducts),
                new { id = product.Id },
                mappedProduct);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPut]
        public async Task<ActionResult> UpdateProduct(int productId, UpdateProductDto model)
        {
            var product = await _productRepo.GetById(productId);
            if (product == null)
            {
                return NotFound(new ApiResponse((int)HttpStatusCode.NotFound));
            }
            _mapper.Map(model, product);
            _productRepo.Update(product);
            await _productRepo.SaveChangesAsync();
            return NoContent();

        }
        [Authorize(Roles = AppRoles.Admin)]
        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteProduct(int productId)
        {
            var product = await _productRepo.GetById(productId);
            if (product == null)
            {
                return NotFound();
            }
            _productRepo.Delete(product);
            await _productRepo.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPut("{productId}/stock")]
        public async Task<ActionResult> UpdateStock(int productId, [FromBody] UpdateStockDto dto)
        {
            if (productId <= 0)
            {
                return BadRequest("Invalid product ID");
            }
            if (!dto.StockQuantity.HasValue)
            {
                return BadRequest("Stock quantity is required");
            }
            var product = await _Context.Products
                .AsNoTracking()
                .Where(p => p.Id == productId)
                .Select(P => new
                {
                    P.Id,
                    P.StockQuantatiy
                })
                .FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound($"Product with ID {productId} not found");
            }
                int newStockQuantity = dto.StockQuantity.Value;
                int affectedRows = await _Context.Products
                    .Where(p => p.Id == productId && p.StockQuantatiy == product.StockQuantatiy)
                    .ExecuteUpdateAsync(setters => setters
                    .SetProperty(
                        p => p.StockQuantatiy, newStockQuantity)
                        );
                if (affectedRows == 0)
                {
                    return Conflict("The product was updated by another user. Please refresh and try again.");
                }
                return Ok(new { message = "Stock quantity updated successfully", productId = productId, stockQuantity = newStockQuantity });


            }


        }
    }
