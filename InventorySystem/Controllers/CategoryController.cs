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
    public class CategoryController : Controller
    {
        public CategoryController(ICategoryService categoryService)
        {
            CategoryService = categoryService;
        }

        public ICategoryService CategoryService { get; set; }

        /// <summary>
        /// add new category
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{businessId}/create")]
        public async Task<IActionResult> Create([FromRoute]Guid businessId, [FromBody]CategoryRequest request)
        {
            var res = await CategoryService.Add(businessId, request);
            return Ok(res);
        }


        /// <summary>
        /// get categories
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        [HttpGet("{businessId}/all")]
        public async Task<IActionResult> GetAllCatergories([FromRoute] Guid businessId)
        {
            var res = await CategoryService.GetCatergories(businessId);
            return Ok(res);
        }
    }
}