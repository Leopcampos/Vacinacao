using Fiotec.Vaicinacao.API.Models;

namespace Fiotec.Vaicinacao.API.Repositories.Interfaces
{
    public interface IRelatorioRepository
    {
        Task<IEnumerable<Relatorio>> BuscarTodosAsync();
        Task<IEnumerable<Relatorio>> BuscarRelatoriosPorDataAsync(DateTime data);
        Task CriarRelatorioAsync(Relatorio relatorio);
    }
}