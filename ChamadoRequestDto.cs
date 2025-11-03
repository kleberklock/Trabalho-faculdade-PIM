using System.ComponentModel.DataAnnotations;

namespace SeuProjetoApi.Models
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
    }
}