using Fiotec.Vaicinacao.API.Data;
using Fiotec.Vaicinacao.API.Models;
using Fiotec.Vaicinacao.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fiotec.Vaicinacao.API.Repositories
{
    public class SolicitanteRepository : ISolicitanteRepository
    {
        private readonly SqlContext _context;

        public SolicitanteRepository(SqlContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Solicitante>> BuscarSolicitantesAsync()
        {
            return await _context.Solicitantes.ToListAsync();
        }

        public async Task<Solicitante> BuscarPorCpfAsync(string cpf)
        {
            return await _context.Solicitantes.FirstOrDefaultAsync(s => s.Cpf == cpf);
        }

        public async Task<Solicitante> BuscarPorIdAsync(int id)
        {
            return await _context.Solicitantes.FindAsync(id);
        }

        public async Task<Solicitante> BuscarPorNomeCpfAsync(string nome, string cpf)
        {
            return await _context.Solicitantes.FirstOrDefaultAsync(s => s.Nome == nome && s.Cpf == cpf);
        }

        public async Task CriarSolicitanteAsync(Solicitante solicitante)
        {
            _context.Solicitantes.Add(solicitante);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarSolicitanteAsync(Solicitante solicitante)
        {
            _context.Entry(solicitante).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletarSolicitanteAsync(int id)
        {
            var solicitante = await BuscarPorIdAsync(id);
            if (solicitante == null)
                throw new ArgumentException($"Solicitante com o ID {id} não encontrado.");

            _context.Solicitantes.Remove(solicitante);
            await _context.SaveChangesAsync();
        }
    }
}