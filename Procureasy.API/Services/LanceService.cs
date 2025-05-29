using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Procureasy.API.Services.Interfaces;
using Procureasy.API.Dtos.Lance;
using Procureasy.API.Data;
using Procureasy.API.Models;

namespace Procureasy.API.Services
{

    public class LanceService : ILanceService
    {
        private readonly ProcurEasyContext _context;

        public LanceService(ProcurEasyContext context)
        {
            _context = context;
        }

        public async Task<List<LanceDto>> GetAllAsync()
        {
            return await _context.Lances
                .Select(l => new LanceDto
                {
                    Id = l.Id,
                    Valor = l.Valor,
                    Vencedor = l.Vencedor,
                    Observacao = l.Observacao,
                    UsuarioId = l.UsuarioId,
                    LeilaoId = l.LeilaoId,
                    DataCriacao = l.DataCriacao
                }).ToListAsync();
        }

        public async Task<LanceDto?> GetByIdAsync(int id)
        {
            var l = await _context.Lances.FindAsync(id);
            if (l == null) return null;

            return new LanceDto
            {
                Id = l.Id,
                Valor = l.Valor,
                Vencedor = l.Vencedor,
                Observacao = l.Observacao,
                UsuarioId = l.UsuarioId,
                LeilaoId = l.LeilaoId,
                DataCriacao = l.DataCriacao
            };
        }

        public async Task<(bool Success, string? Message)> CreateAsync(LanceCreateDto dto)
        {
            if (dto.Valor <= 0)
                return (false, "O valor do lance deve ser maior que zero.");

            if (!await _context.Usuarios.AnyAsync(u => u.Id == dto.UsuarioId))
                return (false, "Usuário inválido.");

            if (!await _context.Leiloes.AnyAsync(l => l.Id == dto.LeilaoId))
                return (false, "Leilão inválido.");

            var lance = new Lance
            {
                Valor = dto.Valor,
                Observacao = dto.Observacao,
                UsuarioId = dto.UsuarioId,
                LeilaoId = dto.LeilaoId,
                DataCriacao = DateTime.UtcNow,
                Vencedor = false
            };

            _context.Lances.Add(lance);
            await _context.SaveChangesAsync();

            return (true, null);
        }
    }
}