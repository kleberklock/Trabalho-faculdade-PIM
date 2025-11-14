// Nome do Arquivo: models/Produto.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaChamadosApi.Models // Namespace correto: Models
{
    [Table("PRODUTO")]
    public class Produto
    {
        [Key]
        [Column("ID_PRODUTO")]
        public int IdProduto { get; set; }

        [Required]
        [Column("NOME_PRODUTO")]
        public string NomeProduto { get; set; } = string.Empty;

        [Required]
        [Column("PRECO_PRODUTO", TypeName = "decimal(10, 2)")] // Correção do atributo duplicado
        public decimal PrecoProduto { get; set; }

        [Required]
        [Column("DATA_FABRICACAO")]
        public DateTime DataFabricacao { get; set; }

        [Required]
        [Column("VALIDADE")]
        public DateTime Validade { get; set; }

        [Required]
        [Column("DISPONIVEL")]
        public bool Disponivel { get; set; }
    }
}