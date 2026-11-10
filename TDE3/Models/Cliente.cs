
using SQLite;

namespace TDE3.Models
{
    public class Cliente
    {
        [PrimaryKey, AutoIncrement]
        public int CodigoCliente { get; set; }

        [MaxLength(50)]
        public string Nome { get; set; } = "";

        public int RG { get; set; }

        [MaxLength(14)]
        public string Telefone { get; set; } = "";
    }
}
