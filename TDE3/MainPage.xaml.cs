
using TDE3.Pages;

namespace TDE3
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void AbrirClientes(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ClientesPage));
        }

        private async void AbrirProdutos(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ProdutosPage));
        }

        private async void AbrirCompras(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ComprasPage));
        }
    }
}
