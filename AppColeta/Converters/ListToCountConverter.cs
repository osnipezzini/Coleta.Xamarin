using Microsoft.EntityFrameworkCore;

using SOColeta.Data;
using SOColeta.Models;

using System;
using System.Globalization;
using System.Linq;

using Xamarin.Forms;

namespace SOColeta.Converters
{
    class ListToCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Inventario inventario)
            {
                var appDbContext = Module.GetService<AppDbContext>();
                var coletas = appDbContext.Inventarios
                    .Where(x => x.Id == inventario.Id)
                    .Include(x => x.ProdutosColetados)
                    .Select(x => x.ProdutosColetados)
                    .FirstOrDefault();
                if (coletas is null)
                    return 0;
                return coletas.Count;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
