using Fiotec.Vaicinacao.API.Models;

namespace Fiotec.Vaicinacao.API.Repositories.Interfaces
{
    public interface ISolicitanteRepository
    {
        Task<Solicitante> BuscarPorIdAsync(int id);
        Task<Solicitante> BuscarPorCpfAsync(string cpf);
        Task<IEnumerable<Solicitante>> BuscarSolicitantesAsync();
        Task<Solicitante> BuscarPorNomeCpfAsync(string nome, string cpf);
        Task CriarSolicitanteAsync(Solicitante solicitante);
        Task AtualizarSolicitanteAsync(Solicitante solicitante);
        Task DeletarSolicitanteAsync(int id);
    }
}