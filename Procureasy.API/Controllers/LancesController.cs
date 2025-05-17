using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Procureasy.API.Data;
using Procureasy.API.Models;

namespace Procureasy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LancesController : ControllerBase
    {
        private readonly ProcurEasyContext _context;

        public LancesController(ProcurEasyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Lance> Get()
        {
            return _context.Lances;
        }

        [HttpGet("{id}")]
        public Lance? GetById(int id)
        {
            return _context.Lances.FirstOrDefault(lance => lance.Id == id);
        }
        
        [HttpPost]
        public string Post()
        {
            return "exemplo de post";
        }
        
        [HttpPut("{id}")]
        public string Put(int id)
        {
            return $"exemplo de put com id = {id}";
        }

        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            return $"exemplo de Delete com id = {id}";
        }

    }
}