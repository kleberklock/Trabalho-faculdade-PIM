// Este arquivo mapeia a tabela SOLICITACAO_SERVICO no banco de dados.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaChamadosApi.Models
{
    [Table("SOLICITACAO_SERVICO")]
    public class SolicitacaoServico
    {
        [Key]
        [Column("ID_SOLICITACAO")]
        public int IdSolicitacao { get; set; }

        [Column("ID_SERVICO")]
        public int IdServico { get; set; }

        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [Column("DATA_SOLICITACAO")]
        public DateTime DataSolicitacao { get; set; } = DateTime.Now;

        [Required]
        [Column("DESCRICAO")]
        [StringLength(255)]
        public string Descricao { get; set; } = string.Empty;

        [Column("STATUS")]
        [StringLength(20)]
        public string Status { get; set; } = "Pendente";

        [Column("PRIORIDADE")]
        [StringLength(15)]
        public string Prioridade { get; set; } = "Média";

        [Column("ANALISE_IA")]
        [StringLength(255)]
        public string? AnaliseIA { get; set; }
        
        // Propriedades de navegação
        [ForeignKey("IdServico")]
        public Servico? Servico { get; set; }
        
        [ForeignKey("IdUsuario")]
        public Usuario? Usuario { get; set; }
    }
}