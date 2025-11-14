// Este arquivo mapeia a tabela SERVICO no banco de dados.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaChamadosApi.Models
{
    [Table("SERVICO")]
    public class Servico
    {
        [Key]
        [Column("ID_SERVICO")]
        public int IdServico { get; set; }

        [Column("DATA_SERVICO")]
        public DateTime DataServico { get; set; } = DateTime.Now;

        [Required]
        [Column("DESCRICAO_SERVICO")]
        [StringLength(100)]
        public string DescricaoServico { get; set; } = string.Empty;

        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }
        
        // Propriedade de navegação (opcional, mas bom para relacionamentos)
        [ForeignKey("IdUsuario")]
        public Usuario? Usuario { get; set; }
    }
}