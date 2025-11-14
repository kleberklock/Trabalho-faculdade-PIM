// Adicionando a propriedade IdEmpresarial para rastrear quem abriu o chamado.
using System.ComponentModel.DataAnnotations;

namespace SistemaChamadosApi.Models // Namespace correto: Models
{
    public class ChamadoRequestDto
    {
        [Required]
        public string Assunto { get; set; } = string.Empty;

        [Required]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        public string Prioridade { get; set; } = string.Empty; 

        [Required]
        public string Categoria { get; set; } = string.Empty; 
        
        // NOVO: Necessário para identificar o solicitante no back-end
        [Required]
        public string IdEmpresarial { get; set; } = string.Empty;
    }
}