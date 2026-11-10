
using TDE3.Data;
using TDE3.Models;

namespace TDE3.Pages
{
    public partial class ClientesPage : ContentPage
    {
        BancoDados banco = new BancoDados();
        Cliente clienteSelecionado;

        public ClientesPage()
        {
            InitializeComponent();
            CarregarClientes();
        }

        void CarregarClientes()
        {
            listaClientes.ItemsSource = banco.ListarClientes();
        }

        async void SalvarCliente(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(txtRG.Text) ||
                string.IsNullOrWhiteSpace(txtTelefone.Text))
            {
                await DisplayAlert("Atenção", "Preencha todos os campos.", "OK");
                return;
            }

            if (txtNome.Text.Trim().Length > 50 ||
                txtTelefone.Text.Trim().Length > 14 ||
                !int.TryParse(txtRG.Text, out int rg) ||
                rg <= 0)
            {
                await DisplayAlert("Atenção", "Verifique os dados informados.", "OK");
                return;
            }

            Cliente cliente = clienteSelecionado ?? new Cliente();

            cliente.Nome = txtNome.Text.Trim();
            cliente.RG = rg;
            cliente.Telefone = txtTelefone.Text.Trim();

            banco.SalvarCliente(cliente);

            await DisplayAlert("Sucesso", "Cliente salvo!", "OK");

            Limpar();
            CarregarClientes();
        }

        void SelecionarCliente(object sender, SelectionChangedEventArgs e)
        {
            clienteSelecionado = e.CurrentSelection.FirstOrDefault() as Cliente;

            if (clienteSelecionado != null)
            {
                txtNome.Text = clienteSelecionado.Nome;
                txtRG.Text = clienteSelecionado.RG.ToString();
                txtTelefone.Text = clienteSelecionado.Telefone;
            }
        }

        async void ExcluirCliente(object sender, EventArgs e)
        {
            Button botao = (Button)sender;
            Cliente cliente = (Cliente)botao.CommandParameter;

            bool possuiCompras = banco.ListarCompras()
                .Any(c => c.CodigoCliente == cliente.CodigoCliente);

            if (possuiCompras)
            {
                await DisplayAlert(
                    "Atenção",
                    "Este cliente possui compras cadastradas e não pode ser excluído.",
                    "OK");
                return;
            }

            bool confirmar = await DisplayAlert(
                "Excluir",
                "Deseja excluir este cliente?",
                "Sim",
                "Não");

            if (confirmar)
            {
                banco.ExcluirCliente(cliente);
                Limpar();
                CarregarClientes();
            }
        }

        void LimparCampos(object sender, EventArgs e)
        {
            Limpar();
        }

        void Limpar()
        {
            txtNome.Text = "";
            txtRG.Text = "";
            txtTelefone.Text = "";
            clienteSelecionado = null;
            listaClientes.SelectedItem = null;
        }
    }
}
