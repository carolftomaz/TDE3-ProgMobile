using SQLite;
using TDE3.Models;

namespace TDE3.Data
{
    public class BancoDados
    {
        private SQLiteConnection banco;

        public BancoDados()
        {
            string caminho = Path.Combine(
                FileSystem.AppDataDirectory,
                "loja.db3");

            banco = new SQLiteConnection(caminho);

            banco.CreateTable<Cliente>();
            banco.CreateTable<Produto>();
            banco.CreateTable<Compra>();
            banco.CreateTable<ItemCompra>();
        }

        public List<Cliente> ListarClientes()
        {
            return banco.Table<Cliente>().ToList();
        }

        public int SalvarCliente(Cliente cliente)
        {
            if (cliente.CodigoCliente == 0)
                return banco.Insert(cliente);

            return banco.Update(cliente);
        }

        public int ExcluirCliente(Cliente cliente)
        {
            return banco.Delete(cliente);
        }

        public List<Produto> ListarProdutos()
        {
            return banco.Table<Produto>().ToList();
        }

        public int SalvarProduto(Produto produto)
        {
            if (produto.CodigoProduto == 0)
                return banco.Insert(produto);

            return banco.Update(produto);
        }

        public int ExcluirProduto(Produto produto)
        {
            return banco.Delete(produto);
        }

        public List<Compra> ListarCompras()
        {
            return banco.Table<Compra>().ToList();
        }

        public int SalvarCompra(Compra compra)
        {
            if (compra.CodigoCompra == 0)
                return banco.Insert(compra);

            return banco.Update(compra);
        }

        public int ExcluirCompra(Compra compra)
        {
            return banco.Delete(compra);
        }

        public List<ItemCompra> ListarItens()
        {
            return banco.Table<ItemCompra>().ToList();
        }

        public int SalvarItem(ItemCompra item)
        {
            if (item.Codigo == 0)
                return banco.Insert(item);

            return banco.Update(item);
        }

        public int ExcluirItem(ItemCompra item)
        {
            return banco.Delete(item);
        }
    }
}
