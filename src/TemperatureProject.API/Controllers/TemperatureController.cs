using System;
using Microsoft.AspNetCore.Mvc;
using TemperatureProject.Core.Services;

namespace TemperatureProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private readonly IDbService dbService;

        public TemperatureController(IDbService dbService)
        {
            this.dbService = dbService;
        }

        [HttpGet("getLatest")]
        public IActionResult GetLastTemperature()
        {
            return Ok(dbService.GetLast());
        }

        [HttpGet("get")]
        public IActionResult GetTemperatures(DateTime? from, DateTime? to)
        {
            if (to - from > TimeSpan.FromDays(7))
            {
                return BadRequest("Max date range is 7 days.");
            }
            return Ok(dbService.GetWhere(x => x.CreatedAt >= from && x.CreatedAt <= to));
        }

        [HttpGet("count")]
        public IActionResult CountTemperatures(DateTime? from, DateTime? to)
        {
            if (from.HasValue && to.HasValue)
            {
                return Ok(dbService.CountBetween(from.Value, to.Value));
            }
            if (from.HasValue)
            {
                return Ok(dbService.CountToNow(from.Value));
            }
            return Ok(dbService.CountAll());
        }
    }
}
