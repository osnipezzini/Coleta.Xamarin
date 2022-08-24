using SOColeta.Models;
using SOColeta.ViewModels;

using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SOColeta.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OpenedInventarioPage : ContentPage
	{
		private readonly OpenedInventarioViewModel viewModel;

		public OpenedInventarioPage()
		{
			InitializeComponent();
			BindingContext = viewModel = Module.GetService<OpenedInventarioViewModel>();
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await viewModel.OnAppearing();
		}

		private void InventarioSelected(object sender, SelectionChangedEventArgs e)
		{
			if (e.CurrentSelection.Any() && e.CurrentSelection.First() is Inventario inventario)
				viewModel.EditInventarioCommand.Execute(inventario);
		}
	}
}