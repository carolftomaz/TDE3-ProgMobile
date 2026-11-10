
using SQLite;

namespace TDE3.Models
{
    public class Compra
    {
        [PrimaryKey, AutoIncrement]
        public int CodigoCompra { get; set; }

        public DateTime DataCompra { get; set; } = DateTime.Today;

        public decimal ValorTotal { get; set; }

        [Indexed]
        public int CodigoCliente { get; set; }
    }
}
