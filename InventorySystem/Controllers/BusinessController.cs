using System;
using System.Threading.Tasks;
using Domain.DTO.Request;
using Domain.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusinessController : Controller
    {
        public BusinessController(IBusinessService businessService)
        {
            BusinessService = businessService;
        }

        public IBusinessService BusinessService { get; set; }

        [HttpPost("create")]
        [ProducesResponseType((StatusCodes.Status200OK))]
        [ProducesResponseType((StatusCodes.Status400BadRequest))]
        public async Task<IActionResult> Create([FromBody]BusinessRequest request)
        {
            var res = await BusinessService.Add( request);
            return Ok(res);
        }

        [Authorize]
        /// <summary>
        /// Get users
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        [HttpGet("{businessId}/users")]
        [ProducesResponseType((StatusCodes.Status200OK))]
        [ProducesResponseType((StatusCodes.Status400BadRequest))]
        public async Task<IActionResult> GetUsers([FromRoute] Guid businessId)
        {
            var res = await BusinessService.GetUsers(businessId);
            return Ok(res);
        }


        [Authorize]
        /// <summary>
        /// Get business by name
        /// </summary>
        /// <param name="businessName"></param>
        /// <returns></returns>
        [HttpGet("name/{businessName}")]
        [ProducesResponseType((StatusCodes.Status200OK))]
        [ProducesResponseType((StatusCodes.Status404NotFound))]
        public async Task<IActionResult> GetBusinessByName([FromRoute] string businessName)
        {
            var res = await BusinessService.GetByName(businessName);
            return Ok(res);
        }

    }
}