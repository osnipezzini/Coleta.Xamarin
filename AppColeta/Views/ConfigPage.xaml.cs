using SOColeta.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SOColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigPage
    {
        private readonly ConfigViewModel viewModel;
        public ConfigPage()
        {
            InitializeComponent();
            BindingContext = viewModel = Module.GetService<ConfigViewModel>();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearing();
            switch (viewModel.Config.SwipeMode)
            {
                case SwipeMode.Reveal:
                    rdReveal.IsChecked = true;
                    break;
                case SwipeMode.Execute:
                    rdExecute.IsChecked = true;
                    break;
                default:
                    break;
            }
        }
        private void OnExecuteClicked(object sender, CheckedChangedEventArgs e)
        {
            viewModel.SetSwipeMode(SwipeMode.Execute);
        }
        private void OnRevealClicked(object sender, CheckedChangedEventArgs e)
        {
            viewModel.SetSwipeMode(SwipeMode.Reveal);
        }
    }
}