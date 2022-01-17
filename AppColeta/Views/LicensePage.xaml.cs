using SOColeta.ViewModels;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SOColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LicensePage : ContentPage
    {
        private readonly LicenseViewModel viewModel;
        public LicensePage()
        {
            InitializeComponent();

            BindingContext = viewModel = Module.GetService<LicenseViewModel>();
        }

        private void TextDocument_Focused(object sender, FocusEventArgs e)
        {
            if (viewModel.Document != null)
                viewModel.Document = Regex.Replace(viewModel.Document, "[^0-9]+", "");
        }

        private void TextDocument_Unfocused(object sender, FocusEventArgs e)
        {
            if (viewModel.Document != null)
            {
                string doc = Regex.Replace(viewModel.Document, "[^0-9]+", "");
                if (doc.Length == 11)
                    viewModel.Document = $"{doc.Substring(0, 3)}/{doc.Substring(2, 6)}/{doc.Substring(5, 9)}-{doc.Substring(8, 11)}";
                if (doc.Length == 14)
                    viewModel.Document = $"{doc.Substring(0, 2)}.{doc.Substring(2, 3)}.{doc.Substring(5, 3)}/{doc.Substring(8, 4)}-{doc.Substring(12, 2)}";
            }
        }

        private void TextPassword_Completed(object sender, System.EventArgs e)
        {
            if (viewModel.LicenseGenerateCommand.CanExecute(null))
                viewModel.LicenseGenerateCommand.Execute(null);
        }

        private void TextDocument_Completed(object sender, System.EventArgs e)
        {
            TextPassword.Focus();
        }
    }
}