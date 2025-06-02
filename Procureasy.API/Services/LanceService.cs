using Microsoft.EntityFrameworkCore;
using Procureasy.API.Data;
using Procureasy.API.Dtos.Lance;
using Procureasy.API.Models;
using Procureasy.API.Models.Enums;
using Procureasy.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);
            if (usuario == null)
                return (false, "Usuário inválido.");

            var leilao = await _context.Leiloes.FirstOrDefaultAsync(l => l.Id == dto.LeilaoId);
            if (leilao == null)
                return (false, "Leilão inválido.");

            // 🔒 VALIDAÇÃO DO VALOR
            if (dto.Valor > leilao.PrecoInicial)
                return (false, $"O valor do lance não pode exceder o valor inicial do leilão (R$ {leilao.PrecoInicial}).");

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

        public async Task<bool> UpdateStatusAsync(int id, bool vencedor)
        {
            if (!vencedor)
                return false;

            var lance = await _context.Lances
                .Include(l => l.Leilao)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lance == null)
                return false;

            lance.Vencedor = true;
            _context.Entry(lance).State = EntityState.Modified;

            if (lance.Leilao != null)
            {
                lance.Leilao.Status = StatusLeilao.ENCERRADO;
                lance.Leilao.DataAtualizacao = DateTime.UtcNow;
                _context.Entry(lance.Leilao).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}