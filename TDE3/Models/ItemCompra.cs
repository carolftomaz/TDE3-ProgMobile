
using SQLite;

namespace TDE3.Models
{
    public class ItemCompra
    {
        [PrimaryKey, AutoIncrement]
        public int Codigo { get; set; }

        [Indexed]
        public int CodigoProduto { get; set; }

        [Indexed]
        public int CodigoCompra { get; set; }

        public int Quantidade { get; set; }

        public decimal ValorParcial { get; set; }
    }
}
