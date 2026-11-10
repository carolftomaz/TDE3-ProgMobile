
using System.Collections.ObjectModel;
using TDE3.Data;
using TDE3.Models;

namespace TDE3.Pages
{
    public partial class ComprasPage : ContentPage
    {
        BancoDados banco = new BancoDados();

        Compra compraSelecionada;
        ItemCompra itemSelecionado;

        public ObservableCollection<Compra> Compras { get; set; } = new();
        public ObservableCollection<ItemCompra> Itens { get; set; } = new();

        public ComprasPage()
        {
            InitializeComponent();
            BindingContext = this;
            CarregarDados();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CarregarDados();
        }

        void CarregarDados()
        {
            int codigoCliente = (pickerCliente.SelectedItem as Cliente)?.CodigoCliente ?? 0;
            int codigoProduto = (pickerProduto.SelectedItem as Produto)?.CodigoProduto ?? 0;

            var clientes = banco.ListarClientes();
            var produtos = banco.ListarProdutos();

            pickerCliente.ItemsSource = clientes;
            pickerProduto.ItemsSource = produtos;

            pickerCliente.SelectedItem = clientes.FirstOrDefault(
                c => c.CodigoCliente == codigoCliente);

            pickerProduto.SelectedItem = produtos.FirstOrDefault(
                p => p.CodigoProduto == codigoProduto);

            CarregarCompras();
        }

        void CarregarCompras()
        {
            Compras.Clear();

            foreach (var compra in banco.ListarCompras())
            {
                Compras.Add(compra);
            }
        }

        void CarregarItens()
        {
            Itens.Clear();

            if (compraSelecionada == null)
                return;

            var itens = banco.ListarItens()
                .Where(i => i.CodigoCompra == compraSelecionada.CodigoCompra)
                .ToList();

            foreach (var item in itens)
            {
                Itens.Add(item);
            }

            decimal total = itens.Sum(i => i.ValorParcial);

            compraSelecionada.ValorTotal = total;
            banco.SalvarCompra(compraSelecionada);

            lblTotal.Text = $"Total: R$ {total:F2}";
            lblCompra.Text = $"Compra Nº {compraSelecionada.CodigoCompra}";

            CarregarCompras();
        }

        async void SalvarCompra(object sender, EventArgs e)
        {
            if (pickerCliente.SelectedItem is not Cliente cliente)
            {
                await DisplayAlert("Atenção", "Selecione um cliente.", "OK");
                return;
            }

            Compra compra = compraSelecionada ?? new Compra();

            compra.CodigoCliente = cliente.CodigoCliente;
            compra.DataCompra = dataCompra.Date ?? DateTime.Today;
            banco.SalvarCompra(compra);

            compraSelecionada = compra;

            CarregarItens();

            await DisplayAlert("Sucesso", "Compra salva!", "OK");
        }

        void NovaCompra(object sender, EventArgs e)
        {
            compraSelecionada = null;
            itemSelecionado = null;

            pickerCliente.SelectedItem = null;
            pickerProduto.SelectedItem = null;

            dataCompra.Date = DateTime.Today;
            txtQuantidade.Text = "";

            Itens.Clear();

            lblCompra.Text = "Nenhuma compra selecionada";
            lblTotal.Text = "Total: R$ 0,00";
        }

        async void SalvarItem(object sender, EventArgs e)
        {
            if (compraSelecionada == null)
            {
                await DisplayAlert("Atenção", "Salve uma compra primeiro.", "OK");
                return;
            }

            if (pickerProduto.SelectedItem is not Produto produto)
            {
                await DisplayAlert("Atenção", "Selecione um produto.", "OK");
                return;
            }

            if (!int.TryParse(txtQuantidade.Text, out int quantidade) ||
                quantidade <= 0)
            {
                await DisplayAlert("Atenção", "Informe uma quantidade válida.", "OK");
                return;
            }

            if (quantidade > produto.QtdEstoque)
            {
                await DisplayAlert("Atenção", "Quantidade maior que o estoque.", "OK");
                return;
            }

            if (itemSelecionado == null &&
                Itens.Any(i => i.CodigoProduto == produto.CodigoProduto))
            {
                await DisplayAlert("Atenção", "Este produto já está na compra. Edite o item existente.", "OK");
                return;
            }

            if (itemSelecionado != null &&
                Itens.Any(i => i.CodigoProduto == produto.CodigoProduto &&
                               i.Codigo != itemSelecionado.Codigo))
            {
                await DisplayAlert("Atenção", "Este produto já está na compra.", "OK");
                return;
            }

            ItemCompra item = itemSelecionado ?? new ItemCompra();

            item.CodigoCompra = compraSelecionada.CodigoCompra;
            item.CodigoProduto = produto.CodigoProduto;
            item.Quantidade = quantidade;
            item.ValorParcial = produto.Preco * quantidade;

            banco.SalvarItem(item);

            itemSelecionado = null;
            pickerProduto.SelectedItem = null;
            txtQuantidade.Text = "";

            CarregarItens();

            await DisplayAlert("Sucesso", "Item salvo!", "OK");
        }

        void EditarItem(object sender, EventArgs e)
        {
            Button botao = (Button)sender;
            itemSelecionado = (ItemCompra)botao.CommandParameter;

            var produtos = banco.ListarProdutos();

            pickerProduto.SelectedItem = produtos.FirstOrDefault(
                p => p.CodigoProduto == itemSelecionado.CodigoProduto);

            txtQuantidade.Text = itemSelecionado.Quantidade.ToString();
        }

        async void ExcluirItem(object sender, EventArgs e)
        {
            Button botao = (Button)sender;
            ItemCompra item = (ItemCompra)botao.CommandParameter;

            bool confirmar = await DisplayAlert(
                "Excluir",
                "Deseja excluir este item?",
                "Sim",
                "Não");

            if (confirmar)
            {
                banco.ExcluirItem(item);
                itemSelecionado = null;
                pickerProduto.SelectedItem = null;
                txtQuantidade.Text = "";

                CarregarItens();
            }
        }

        void LimparItem(object sender, EventArgs e)
        {
            itemSelecionado = null;
            pickerProduto.SelectedItem = null;
            txtQuantidade.Text = "";
        }

        void AbrirCompra(object sender, EventArgs e)
        {
            Button botao = (Button)sender;
            Compra compra = (Compra)botao.CommandParameter;

            compraSelecionada = compra;
            itemSelecionado = null;

            var clientes = banco.ListarClientes();

            pickerCliente.SelectedItem = clientes.FirstOrDefault(
                c => c.CodigoCliente == compra.CodigoCliente);

            dataCompra.Date = compra.DataCompra;

            pickerProduto.SelectedItem = null;
            txtQuantidade.Text = "";

            CarregarItens();
        }

        async void ExcluirCompra(object sender, EventArgs e)
        {
            Button botao = (Button)sender;
            Compra compra = (Compra)botao.CommandParameter;

            bool confirmar = await DisplayAlert(
                "Excluir",
                "Deseja excluir esta compra e seus itens?",
                "Sim",
                "Não");

            if (!confirmar)
                return;

            var itens = banco.ListarItens()
                .Where(i => i.CodigoCompra == compra.CodigoCompra)
                .ToList();

            foreach (var item in itens)
            {
                banco.ExcluirItem(item);
            }

            banco.ExcluirCompra(compra);

            NovaCompra(sender, e);
            CarregarCompras();

            await DisplayAlert("Sucesso", "Compra excluída!", "OK");
        }
    }
}
