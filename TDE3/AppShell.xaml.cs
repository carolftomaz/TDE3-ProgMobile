
using TDE3.Pages;

namespace TDE3
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ClientesPage), typeof(ClientesPage));
            Routing.RegisterRoute(nameof(ProdutosPage), typeof(ProdutosPage));
            Routing.RegisterRoute(nameof(ComprasPage), typeof(ComprasPage));
        }
    }
}
