using System.Threading.Tasks;

namespace AppColeta.Services
{
    public interface IQrCodeScanningService
    {
        Task<string> ScanAsync();
    }
}
