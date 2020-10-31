using AppColeta.ViewModels;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LicensePage : ContentPage
    {
        LicenseViewModel ViewModel => BindingContext as LicenseViewModel;
        public LicensePage()
        {
            InitializeComponent();
        }

        private void TextDocument_Focused(object sender, FocusEventArgs e)
        {
            if (ViewModel.Document != null)
                ViewModel.Document = Regex.Replace(ViewModel.Document, "[^0-9a-zA-Z]+", "");
        }

        private void TextDocument_Unfocused(object sender, FocusEventArgs e)
        {
            if (ViewModel.Document != null)
            {
                string doc = Regex.Replace(ViewModel.Document, "[^0-9a-zA-Z]+", "");
                if (doc.Length == 11)
                    ViewModel.Document = $"{doc.Substring(0, 3)}/{doc.Substring(2, 6)}/{doc.Substring(5, 9)}-{doc.Substring(8, 11)}";
                if (doc.Length == 14)
                    ViewModel.Document = $"{doc.Substring(0, 2)}.{doc.Substring(2, 3)}.{doc.Substring(5, 3)}/{doc.Substring(8, 4)}-{doc.Substring(12, 2)}";
            }
        }

        private void TextPassword_Completed(object sender, System.EventArgs e)
        {
            if (ViewModel.LicenseGenerateCommand.CanExecute(null))
                ViewModel.LicenseGenerateCommand.Execute(null);
        }

        private void TextDocument_Completed(object sender, System.EventArgs e)
        {
            TextPassword.Focus();
        }
    }
}