using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Procureasy.API.Dtos;
using Procureasy.API.Services.Interfaces;

namespace Procureasy.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly ITokenService _tokenService;
        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        public ActionResult Login([FromBody] LoginDto loginDto)
        {
            var token = _tokenService.GenerateToken(loginDto);
            if (token == "")
                return Unauthorized();
            return Ok(token);
        }
    }
}