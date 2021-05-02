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
    [Authorize]
    public class CustomerController : Controller
    {
        public CustomerController(ICustomerService customerService)
        {
            CustomerService = customerService;
        }

        public ICustomerService CustomerService { get; set; }


        [HttpPost("{businessId}/create")]
        [ProducesResponseType((StatusCodes.Status200OK))]
        [ProducesResponseType((StatusCodes.Status400BadRequest))]
        public async Task<IActionResult> Create([FromRoute]Guid businessId,  [FromBody] CustomerRequest request)
        {
            var res = await CustomerService.Add( businessId, request);
            return Ok(res);
        }
    }
}