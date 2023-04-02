using Fiotec.Vaicinacao.API.Data;
using Fiotec.Vaicinacao.API.Models;
using Fiotec.Vaicinacao.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using static Fiotec.Vaicinacao.API.Models.Relatorio;

namespace Fiotec.Vaicinacao.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioController : ControllerBase
    {
        private readonly IRelatorioRepository _relatorioRepository;
        private readonly ISolicitanteRepository _solicitanteRepository;
        private readonly HttpClient _httpClient;

        public RelatorioController(IRelatorioRepository relatorioRepository, ISolicitanteRepository usuarioRepository, HttpClient httpClient)
        {
            _relatorioRepository = relatorioRepository;
            _solicitanteRepository = usuarioRepository;
            _httpClient = httpClient;
        }

        [HttpGet("relatorio/{nome}/{cpf}/{data}")]
        public async Task<IActionResult> GetRelatorioAsync(string nome, string cpf, string data)
        {
            try
            {
                // Verificar se o solicitante existe
                var solicitante = await _solicitanteRepository.BuscarPorNomeCpfAsync(nome, cpf);
                if (solicitante == null)
                {
                    // Se o solicitante não existir, cria um novo
                    solicitante = new Solicitante { Nome = nome, Cpf = cpf };
                    await _solicitanteRepository.CriarSolicitanteAsync(solicitante);
                }

                // Validar data
                if (!DateTime.TryParseExact(data, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataFormatada))
                    return BadRequest("Data inválida");

                // Gerar relatório
                string url = "https://imunizacao-es.saude.gov.br/_search?scroll=1m";
                string user = "imunizacao_public";
                string password = "qlto5t&7r_@+#Tlstigi";

                using var client = new HttpClient();
                var authBytes = Encoding.ASCII.GetBytes($"{user}:{password}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));

                client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
                var content = new StringContent("{\"size\":10000,\"query\":{\"bool\":{\"must\":[{\"match\":{\"vacina_fabricante_nome\":\"PFIZER\"}},{\"match\":{\"paciente_endereco_uf\":\"RJ\"}}]}}}", Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var hits = JsonConvert.DeserializeObject<JObject>(responseBody)["hits"]["hits"].Children();
                var dadosVacinas = hits
                    .Select(h => h["_source"].ToObject<Relatorio>())
                    .Where(r => r.DataAplicacao == dataFormatada)
                    .ToList();

                // Somar a quantidade total de vacinas
                int quantidadeTotalDeVacinas = Regex.Matches(responseBody, "\"vacina_fabricante_nome\":\"PFIZER\"").Count;


                // Criar um novo relatório com as informações agregadas
                var relatorio = new Relatorio
                {
                    DataSolicitacao = DateTime.Now,
                    DescricaoRelatorio = $"Relatório Vacinas Pfizer aplicadas no RJ_{data}",
                    DataAplicacao = dataFormatada,
                    QuantidadeTotalDeVacinas = quantidadeTotalDeVacinas,
                    Solicitante = new Solicitante
                    {
                        Nome = nome,
                        Cpf = cpf,
                    }
                };

                // Salvar o novo relatório no banco de dados
                await SalvarRelatorio(relatorio);

                return Ok(relatorio);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task SalvarRelatorio(Relatorio relatorio)
        {
            try
            {
                // Salvar relatório no banco de dados
                await _relatorioRepository.CriarRelatorioAsync(relatorio);
            }
            catch (Exception ex)
            {
                // Tratar exceção aqui
                throw ex;
            }
        }
    }
}