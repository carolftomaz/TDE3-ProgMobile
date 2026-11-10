
using SQLite;

namespace TDE3.Models
{
    public class Produto
    {
        [PrimaryKey, AutoIncrement]
        public int CodigoProduto { get; set; }

        [MaxLength(50)]
        public string NomeProduto { get; set; } = "";

        [MaxLength(50)]
        public string Tipo { get; set; } = "";

        public decimal Preco { get; set; }

        public int QtdEstoque { get; set; }
    }
}
