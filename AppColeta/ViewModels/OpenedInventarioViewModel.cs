using Acr.UserDialogs;

using SOColeta.Models;
using SOColeta.Services;

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SOColeta.ViewModels
{
    class OpenedInventarioViewModel : ViewModelBase
    {
        private readonly IStockService stockService;

        public ObservableCollection<Inventario> Inventarios { get; }
        public Command LoadInventariosCommand { get; }
        public Command CreateInventarioCommand { get; }
        public Command<Inventario> EditInventarioCommand { get; }
        public Command<Inventario> DeleteInventarioCommand { get; }

        public OpenedInventarioViewModel(IStockService stockService)
        {
            this.stockService = stockService;
            Inventarios = new ObservableCollection<Inventario>();
            LoadInventariosCommand = new Command(async () => await LoadInventarios());
            CreateInventarioCommand = new Command(async () => await CreateInventario());
            EditInventarioCommand = new Command<Inventario>(async (inventario) => await EditInventario(inventario));
            DeleteInventarioCommand = new Command<Inventario>(async (inventario) => await DeleteInventario(inventario));
        }
        public override async Task OnAppearing()
        {
            await LoadInventarios();
        }
        private async Task DeleteInventario(Inventario inventario)
        {
            var confirm = await Shell.Current
                .DisplayActionSheet("Tem certeza que deseja excluir o inventário selecionado ?", "Sim", "Não");
            if (confirm == "Sim")
                using (var loading = UserDialogs.Instance.Loading("Carregando coleta..."))
                {
                    await stockService.DeleteInventario(inventario);
                    await LoadInventarios();
                }
        }

        private async Task CreateInventario()
        {
            using (var loading = UserDialogs.Instance.Loading("Iniciando inventário ..."))
            {
                var inventario = await stockService.CreateInventario();
                await EditInventario(inventario);
            }
        }
        private async Task EditInventario(Inventario inventario)
        {
            using (var loading = UserDialogs.Instance.Loading("Editando inventário ..."))
            {
                await GoToAsync($"/CriarInventario?InventarioId={inventario.Id}");
            }
        }

        private async Task LoadInventarios()
        {
            using (var loading = UserDialogs.Instance.Loading("Carregando inventários ..."))
            {
                Inventarios.Clear();

                await foreach (var inventario in stockService.GetOpenedInventarios())
                {
                    Inventarios.Add(inventario);
                }
            }
        }
    }
}
