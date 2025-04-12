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
    public class LeiloesController : ControllerBase
    {
        private readonly ProcurEasyContext _context;

        public LeiloesController(ProcurEasyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Leilao> Get()
        {
            return _context.Leiloes;
        }

        [HttpGet("{id}")]
        public Leilao? GetById(int id)
        {
            return _context.Leiloes.FirstOrDefault(leilao => leilao.Id == id);
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