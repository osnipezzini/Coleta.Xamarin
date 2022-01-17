using System.Threading.Tasks;

namespace SOColeta.Services
{
    public interface IQrCodeScanningService
    {
        Task<string> ScanAsync();
    }
}
