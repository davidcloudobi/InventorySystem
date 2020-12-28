﻿using System;
using System.Threading.Tasks;
using Domain.DTO.Request;
using Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OutletController : Controller
    {
        public OutletController(IOutletService outletService)
        {
            OutletService = outletService;
        }

        public IOutletService OutletService { get; set; }

        /// <summary>
        /// add outlet
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{businessId}")]
        public async Task<IActionResult> Create([FromRoute]Guid businessId , [FromBody] OutletRequest request)
        {
            var res = await OutletService.Create(businessId, request);
            return Ok(res);
        }
    }
}