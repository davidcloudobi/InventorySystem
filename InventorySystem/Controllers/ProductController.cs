using System;
using System.Threading.Tasks;
using Domain.DTO.Request;
using Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProductController : Controller
    {
        public ProductController(IProductService productService)
        {
            ProductService = productService;
        }

        public IProductService ProductService { get; set; }

        /// <summary>
        /// Add product
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{businessId}/create")]
        public async Task<IActionResult> Add([FromRoute]Guid businessId, [FromForm]ProductRequest request)
        {
            var res = await ProductService.Add(businessId, request);
            return Ok(res);
        }

        /// <summary>
        /// Get products
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>

        [HttpGet("{businessId}/products")]
        public async Task<IActionResult> GetProducts([FromRoute] Guid businessId)
        {
            var response = await ProductService.GetProducts(businessId);
            return Ok(response);
        }

        /// <summary>
        /// get single product
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("{businessId}/product/{productId}")]
        public async Task<IActionResult> GetProduct([FromRoute] Guid businessId, [FromRoute] Guid productId)
        {
            var response = await ProductService.GetProduct(businessId, productId);
            return Ok(response);
        }

        /// <summary>
        /// Update Price
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="productId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{businessId}/product/price/update/{productId}")]
        public async Task<IActionResult> UpdatePrice([FromRoute] Guid businessId, [FromRoute] Guid productId, [FromBody] UpdateProductPrice request)
        {
            var response = await ProductService.UpdatePrice(businessId, productId, request);
            return Ok(response);
        }

        /// <summary>
        /// delete a product
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete("{businessId}/product/delete/{productId}")]
        public async Task<IActionResult> DeleteOneProduct([FromRoute] Guid businessId, [FromRoute] Guid productId)
        {
            var response = await ProductService.DeleteOneProduct(businessId, productId);
            return Ok(response);
        }

        /// <summary>
        /// delete a product
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete("{businessId}/products/delete")]
        public async Task<IActionResult> DeleteOneProduct([FromRoute] Guid businessId, [FromBody] DeleteProductRequest request)
        {
            var response = await ProductService.DeleteProducts(businessId, request);
            return Ok(response);
        }




        /// <summary>
        /// Get products
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>

        //[Authorize]
        [HttpGet("products/{name}")]
        public async Task<IActionResult> FilterProducts([FromRoute] string name)
        {
            var response = await ProductService.FilterProducts(name);
            return Ok(response);
        }
    }
}