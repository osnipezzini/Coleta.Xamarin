﻿using SOColeta.ViewModels;

using Xamarin.Forms.Xaml;

namespace SOColeta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CriarInventarioPage
    {
        private readonly CriarInventarioViewModel viewModel;
        public CriarInventarioPage()
        {
            InitializeComponent();
            BindingContext = viewModel = Module.GetService<CriarInventarioViewModel>();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearing();
        }
    }
}