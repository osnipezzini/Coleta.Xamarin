using AppColeta.Data;
using AppColeta.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppColeta.ViewModels
{
    class ImportFileViewModel : BaseViewModel
    {
        private string filename;
        private FileResult fullpath;
        public Command ChooseFileCommand { get; }
        public Command StartImportCommand { get; }
        public ImportFileViewModel()
        {
            Title = "Importar arquivo";
            ChooseFileCommand = new Command(async () => await PickAndShow());
            StartImportCommand = new Command(async () => await ImportFile());
        }
        public string Filename { get => filename; set => SetProperty(ref filename, value); }

        async Task<bool> PickAndShow()
        {
            
            var options = new PickOptions
            {
                FileTypes = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new string[] { "text/txt", "text/csv", "text/text"} },
                    { DevicePlatform.iOS, new string[] { "public.image" } },
                    { DevicePlatform.UWP, new string[] { ".jpg", ".png" } },
                }),
                PickerTitle = "Selecione o arquivo para ser importado"
            };
            try
            {
                var result = await FilePicker.PickAsync(options);
                if (result != null)
                {
                    Filename = result.FileName;
                    fullpath = result;

                    return true;
                }
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }
            return false;
        }

        async Task ImportFile()
        {
            var contexto = new AppDbContext();
            var stream = await fullpath.OpenReadAsync();
            var readerStream = new StreamReader(stream);
            var contents = await readerStream.ReadToEndAsync();
            string[] file = contents.Split('\n');
            foreach (string line in file)
            {
                char splitter = line.Contains(";") ? ';' : ',';
                var reader = line.Replace("\r", "").Split(splitter);
                double custo = 0;
                double venda = 0;

                if (reader.Length >= 4)
                    double.TryParse(reader[3].Replace('.', ','), out custo);
                if (reader.Length >= 3)
                    double.TryParse(reader[2].Replace('.', ','), out venda);

                var produto = await contexto.Produtos.FirstOrDefaultAsync(x => x.Codigo == reader[0]);

                if (produto == null)
                    produto = new Produto();

                produto.Codigo = reader[0];
                produto.Nome = reader[1];
                produto.PrecoVenda = venda;
                produto.PrecoCusto = custo;
                if (produto.Id > 0)
                    contexto.Produtos.Update(produto);
                else
                    contexto.Produtos.Add(produto);
            }
            var reg = await contexto.SaveChangesAsync();

            await Shell.Current.DisplayAlert("Produtos importados", $"Foram importados {reg} produtos.", "Ok");
        }
    }
}
