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
    public class BrandController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="brandService"></param>
        public BrandController(IBrandService brandService)
        {
            BrandService = brandService;
        }

        /// <summary>
        /// 
        /// </summary>
        public IBrandService BrandService { get; set; }

        /// <summary>
        /// add new brand
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{businessId}/create")]
        public async Task<IActionResult> Create([FromRoute]Guid businessId,[FromBody] BrandRequest request)
        {
            var res = await BrandService.Add(businessId, request);
            return Ok(res);
        }

        /// <summary>
        /// get brands
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        [HttpGet("{businessId}/all")]
        public async Task<IActionResult> GetAllBrands([FromRoute]Guid businessId)
        {
            var res = await BrandService.GetBrands(businessId);
            return Ok(res);
        }
    }
}