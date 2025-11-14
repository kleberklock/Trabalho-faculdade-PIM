namespace SistemaChamadosApi.Models // Namespace correto: Models
{
    public class ChamadoDto
    {
        public int IdSolicitacao { get; set; }
        public string Assunto { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty; 
        public string SolicitanteNome { get; set; } = string.Empty;
        public DateTime DataSolicitacao { get; set; }
        public string Prioridade { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}