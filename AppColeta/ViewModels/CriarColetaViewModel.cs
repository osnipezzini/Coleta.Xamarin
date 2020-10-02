using AppColeta.Models;
using AppColeta.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace AppColeta.ViewModels
{
    public class CriarColetaViewModel : BaseViewModel
    {
        private string codigo;
        private string quantidade;

        public CriarColetaViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            ReadCodeCommand = new Command(OpenScan);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }
        private async void OpenScan(object obj)
        {
            var scanner = DependencyService.Get<IQrCodeScanningService>();
            var result = await scanner.ScanAsync();
            if (!string.IsNullOrEmpty(result))
            {
                // Sua logica.
                Codigo = result;
                return;
            }
        }
        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(codigo)
                && !String.IsNullOrWhiteSpace(quantidade) && double.TryParse(quantidade, out double valor);
        }

        public string Codigo
        {
            get => codigo;
            set => SetProperty(ref codigo, value);
        }

        public string Quantidade
        {
            get => quantidade;
            set => SetProperty(ref quantidade, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command ReadCodeCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Coleta newItem = new Coleta()
            {
                Id = Guid.NewGuid().ToString(),
                Codigo = codigo,
                Quantidade = double.Parse(quantidade),
                InventarioId = App.Inventario.Id,
                Inventario = App.Inventario
            };

            await ColetasStore.AddItemAsync(newItem);

            Codigo = string.Empty;
            Quantidade = string.Empty;
        }
    }
}
