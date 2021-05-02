using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTO.Request;
using Domain.Interface;

namespace InventorySystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        public AuthController(IAuthLogin authLogin)
        {
            AuthLogin = authLogin;
        }

        public IAuthLogin AuthLogin { get; set; }

        /// <summary>
        /// User Login
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login/{businessId}")]
        [ProducesResponseType((StatusCodes.Status200OK))]
        public async Task<IActionResult> Login([FromRoute]Guid businessId, [FromBody] AuthLoginRequest request)
        {
            var response = await AuthLogin.Login(businessId, request);
            return Ok(response);
        }
    }
}
