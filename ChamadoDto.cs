// Nome do arquivo: models/ChamadoDto.cs
// DTO = Data Transfer Object
// Isso representa um chamado que a API envia de VOLTA para o front-end.

namespace SeuProjetoApi.Models
{
    public class ChamadoDto
    {
        public int IdSolicitacao { get; set; }
        public string Assunto { get; set; } = string.Empty;
        public string SolicitanteNome { get; set; } = string.Empty; // Para a tela de Gerente
        public DateTime DataSolicitacao { get; set; }
        public string Prioridade { get; set; } = string.Empty; // "Alta", "Media", "Baixa"
        public string Status { get; set; } = string.Empty; // "Pendente", "Em andamento", "Concluido"
    }
}