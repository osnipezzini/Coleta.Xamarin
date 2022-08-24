using SOColeta.Data;
using SOColeta.Models;

using System;
using System.Globalization;
using System.Linq;

using Xamarin.Forms;

namespace SOColeta.Converters
{
    internal class ColetaToProdutoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var appContext = Module.GetService<AppDbContext>();
            if (value is Coleta coleta)
            {
                var product = appContext.Produtos.FirstOrDefault(x => x.Codigo == coleta.Codigo);
                return product.Nome;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
