using SOColeta.ViewModels;

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
	}
}