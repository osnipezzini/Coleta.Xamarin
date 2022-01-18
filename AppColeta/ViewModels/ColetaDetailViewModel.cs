using SOColeta.Services;

using SOTech.Mvvm;

using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace SOColeta.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ColetaDetailViewModel : ViewModelBase
    {
        public ColetaDetailViewModel(IDatabase database)
        {
            Title = "Coleta " + itemId;
            this.database = database;
        }
        private int itemId;
        private string codigo;
        private string quantidade;
        private readonly IDatabase database;

        public int Id { get; set; }

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

        public int ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(int itemId)
        {
            try
            {
                var item = await database.GetColetaAsync(itemId);
                Id = item.Id;
                Codigo = item.Codigo;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
