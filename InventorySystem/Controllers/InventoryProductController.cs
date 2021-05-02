using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTO.Request;
using Domain.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace InventorySystem.Controllers
{
  
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class InventoryProductController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inventoryProductService"></param>
        public InventoryProductController(IInventoryProductService inventoryProductService)
        {
            InventoryProductService = inventoryProductService;
        }

        /// <summary>
        /// 
        /// </summary>
        public IInventoryProductService InventoryProductService { get; set; }

        /// <summary>
        /// Add inventory product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType((StatusCodes.Status200OK))]
        [ProducesResponseType((StatusCodes.Status400BadRequest))]
        public async Task<IActionResult> Add([FromBody] InventoryProductDTO request)
        {
            var res = await InventoryProductService.Add(request);
            return Ok(res);
        }
    }
}
