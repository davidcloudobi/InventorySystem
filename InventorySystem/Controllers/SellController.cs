using System;
using System.Threading.Tasks;
using Domain.DTO.Request;
using Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SellController : Controller
    {
        public SellController(ISellService sellService)
        {
            SellService = sellService;
        }

        public ISellService SellService { get; set; }

        /// <summary>
        /// Add new sell
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{businessId}/create")]
        public async Task<IActionResult> Create([FromRoute]Guid businessId, [FromBody]SellRequestDTO request)
        {
            var response = await SellService.Add(businessId, request);
            return Ok(response);
        }
    }
}