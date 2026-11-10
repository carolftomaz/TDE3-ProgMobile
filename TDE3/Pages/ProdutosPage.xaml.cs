
using System.Globalization;
using TDE3.Data;
using TDE3.Models;

namespace TDE3.Pages
{
    public partial class ProdutosPage : ContentPage
    {
        BancoDados banco = new BancoDados();
        Produto produtoSelecionado;

        public ProdutosPage()
        {
            InitializeComponent();
            CarregarProdutos();
        }

        void CarregarProdutos()
        {
            listaProdutos.ItemsSource = banco.ListarProdutos();
        }

        async void SalvarProduto(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNomeProduto.Text) ||
                string.IsNullOrWhiteSpace(txtTipo.Text) ||
                string.IsNullOrWhiteSpace(txtPreco.Text) ||
                string.IsNullOrWhiteSpace(txtEstoque.Text))
            {
                await DisplayAlert("Atenção", "Preencha todos os campos.", "OK");
                return;
            }

            string precoTexto = txtPreco.Text.Trim().Replace(',', '.');

            if (txtNomeProduto.Text.Trim().Length > 50 ||
                txtTipo.Text.Trim().Length > 50 ||
                !decimal.TryParse(precoTexto, NumberStyles.Number,
                    CultureInfo.InvariantCulture, out decimal preco) ||
                preco <= 0 ||
                !int.TryParse(txtEstoque.Text, out int estoque) ||
                estoque < 0)
            {
                await DisplayAlert("Atenção", "Verifique o preço e o estoque.", "OK");
                return;
            }

            Produto produto = produtoSelecionado ?? new Produto();

            produto.NomeProduto = txtNomeProduto.Text.Trim();
            produto.Tipo = txtTipo.Text.Trim();
            produto.Preco = preco;
            produto.QtdEstoque = estoque;

            banco.SalvarProduto(produto);

            await DisplayAlert("Sucesso", "Produto salvo!", "OK");

            Limpar();
            CarregarProdutos();
        }

        void SelecionarProduto(object sender, SelectionChangedEventArgs e)
        {
            produtoSelecionado = e.CurrentSelection.FirstOrDefault() as Produto;

            if (produtoSelecionado != null)
            {
                txtNomeProduto.Text = produtoSelecionado.NomeProduto;
                txtTipo.Text = produtoSelecionado.Tipo;
                txtPreco.Text = produtoSelecionado.Preco.ToString("F2");
                txtEstoque.Text = produtoSelecionado.QtdEstoque.ToString();
            }
        }

        async void ExcluirProduto(object sender, EventArgs e)
        {
            Button botao = (Button)sender;
            Produto produto = (Produto)botao.CommandParameter;

            bool possuiItens = banco.ListarItens()
                .Any(i => i.CodigoProduto == produto.CodigoProduto);

            if (possuiItens)
            {
                await DisplayAlert(
                    "Atenção",
                    "Este produto está vinculado a uma compra e não pode ser excluído.",
                    "OK");
                return;
            }

            bool confirmar = await DisplayAlert(
                "Excluir",
                "Deseja excluir este produto?",
                "Sim",
                "Não");

            if (confirmar)
            {
                banco.ExcluirProduto(produto);
                Limpar();
                CarregarProdutos();
            }
        }

        void LimparCampos(object sender, EventArgs e)
        {
            Limpar();
        }

        void Limpar()
        {
            txtNomeProduto.Text = "";
            txtTipo.Text = "";
            txtPreco.Text = "";
            txtEstoque.Text = "";
            produtoSelecionado = null;
            listaProdutos.SelectedItem = null;
        }
    }
}
