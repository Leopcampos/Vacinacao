using System.Text.Json.Serialization;

namespace Fiotec.Vaicinacao.API.Models
{
    public class Solicitante
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }

        [JsonIgnore]
        public DateTime DataCriacao { get; set; }

        [JsonIgnore]
        public List<Relatorio>? Relatorios { get; set; }
    }
}