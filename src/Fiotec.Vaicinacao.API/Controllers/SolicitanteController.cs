using Fiotec.Vaicinacao.API.Data;
using Fiotec.Vaicinacao.API.Models;
using Fiotec.Vaicinacao.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiotec.Vaicinacao.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitanteController : ControllerBase
    {
        private readonly ISolicitanteRepository _solicitanteRepository;

        public SolicitanteController(ISolicitanteRepository solicitanteRepository)
        {
            _solicitanteRepository = solicitanteRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Solicitante>>> GetSolicitantes()
        {
            var Solicitantes = await _solicitanteRepository.BuscarSolicitantesAsync();

            return Ok(Solicitantes);
        }


        [HttpPost]
        public async Task<IActionResult> CriarSolicitanteAsync(Solicitante solicitante)
        {
            try
            {
                await _solicitanteRepository.CriarSolicitanteAsync(solicitante);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}