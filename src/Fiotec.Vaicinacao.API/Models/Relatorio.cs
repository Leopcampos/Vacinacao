using Newtonsoft.Json;

namespace Fiotec.Vaicinacao.API.Models
{
    public class Relatorio
    {
        public int Id { get; set; }

        [JsonProperty("descricao")]
        public string DescricaoRelatorio { get; set; }

        public DateTime DataSolicitacao { get; set; } = DateTime.Now;

        [JsonProperty("vacina_dataAplicacao")]
        public DateTime DataAplicacao { get; set; }

        [JsonIgnore]
        public int SolicitanteId { get; set; }

        public int QuantidadeTotalDeVacinas { get; set; }
        public Solicitante Solicitante { get; set; }
    }
}