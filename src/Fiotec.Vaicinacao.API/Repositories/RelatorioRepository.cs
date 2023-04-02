using Fiotec.Vaicinacao.API.Data;
using Fiotec.Vaicinacao.API.Models;
using Fiotec.Vaicinacao.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fiotec.Vaicinacao.API.Repositories
{
    public class RelatorioRepository : IRelatorioRepository
    {
        private readonly SqlContext _context;

        public RelatorioRepository(SqlContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Relatorio>> BuscarTodosAsync()
        {
            return await _context.Relatorios
                .Include(r => r.Solicitante)
                .ToListAsync();
        }

        //public async Task<IEnumerable<Relatorio>> BuscarRelatoriosPorDataAsync(DateTime data)
        //{
        //    return await _context.Relatorios.Where(r => r.DataAplicacao == data && r.Fabricante == "Pfizer" && r.UF == "RJ").ToListAsync();
        //}
        public Task<IEnumerable<Relatorio>> BuscarRelatoriosPorDataAsync(DateTime data)
        {
            throw new NotImplementedException();
        }

        public async Task CriarRelatorioAsync(Relatorio relatorio)
        {
            // Verifica se já existe um usuário com esse nome e CPF
            var usuario = await _context.Solicitantes.FirstOrDefaultAsync(u => u.Nome == relatorio.Solicitante.Nome && u.Cpf == relatorio.Solicitante.Cpf);

            // Se não existe, adiciona o usuário
            if (usuario == null)
            {
                await _context.Solicitantes.AddAsync(relatorio.Solicitante);
            }
            else
            {
                // Se já existe, atualiza o objeto relatório com a referência para o usuário existente
                relatorio.Solicitante = usuario;
            }

            // Adiciona o relatório ao contexto
            await _context.Relatorios.AddAsync(relatorio);

            // Salva as alterações no banco de dados
            await _context.SaveChangesAsync();
        }
    }
}