using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace AppColeta.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ColetaDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string codigo;
        private string quantidade;
        public string Id { get; set; }

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

        public string ItemId
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

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await ColetasStore.GetItemAsync(itemId);
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
