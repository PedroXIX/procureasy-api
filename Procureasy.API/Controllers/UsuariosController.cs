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
    public class UsuariosController : ControllerBase
    {
        private readonly ProcurEasyContext _context;

        public UsuariosController(ProcurEasyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Usuario> Get()
        {
            return _context.Usuarios;
        }

        [HttpGet("{id}")]
        public Usuario? GetById(int id)
        {
            return _context.Usuarios.FirstOrDefault(usuario => usuario.Id == id);
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