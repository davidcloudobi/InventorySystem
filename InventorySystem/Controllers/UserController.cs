using System;
using System.Threading.Tasks;
using Domain.DTO.Request;
using Domain.Interface;
using Domain.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
  //  [Authorize]
    public class UserController : Controller
    {
        public UserController(IUserService userService)
        {
            UserService = userService;
        }

        public IUserService UserService { get; set; }

    /// <summary>
    /// Add a user to a business
    /// </summary>
    /// <param name="businessId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
        [HttpPost("{businessId}/create")]
        [ProducesResponseType((StatusCodes.Status200OK))]
        [ProducesResponseType((StatusCodes.Status400BadRequest))]
        public async Task<IActionResult> Create([FromRoute] Guid businessId , [FromBody]UserRequest request)
        {
            var res = await UserService.CreateUser(businessId, request);
            return Ok(res);
        }

    }
}