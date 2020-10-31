using AppColeta.Data;
using AppColeta.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SOTechLib.Licensing;
using SOTechLib.Licensing.Models;
using SOTechLib.Utils;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppColeta
{
    public static class Helpers
    {
        private static LicenseStatus status_license;
        public static async Task ErrorMessage(string message, string title = "Erro") => await Application.Current.MainPage.DisplayAlert(title, message, "Ok");
        public static async Task InfoMessage(string message, string title = "Sucesso") => await Application.Current.MainPage.DisplayAlert(title, message, "Ok");
        public static async Task WarnMessage(string message, string title = "Atenção") => await Application.Current.MainPage.DisplayAlert(title, message, "Ok");
        public static ERPLicense License
        {
            get
            {
                byte[] public_key;
                string save_folder = DependencyService.Get<IAppPath>().Path;
                string license_file = Path.Combine(save_folder, "license.lic");

                //Read public key from assembly
                Assembly _assembly = Assembly.GetExecutingAssembly();
                using (MemoryStream _mem = new MemoryStream())
                {
                    _assembly.GetManifestResourceStream("AppColeta.ellitedevDDNS.cer").CopyTo(_mem);
                    public_key = _mem.ToArray();
                }
                return LicenseHandler.ParseLicenseFromBASE64String(
                        typeof(ERPLicense),
                        File.ReadAllText(license_file),
                        public_key,
                        out status_license) as ERPLicense;
            }
        }

        internal static LicenseStatus LicenseStatus { get => status_license; set => status_license = value; }

        public static async Task CheckLicense()
        {
            //Initialize variables with default values
            LicenseStatus _status = LicenseStatus.INVALID;
            string save_folder = DependencyService.Get<IAppPath>().Path;
            string license_file = Path.Combine(save_folder, "license.lic");
            string license_log = Path.Combine(save_folder, "license_log");
            //Check if the XML license file exists
            if (File.Exists(license_file) && File.Exists(license_log))
            {
                var created_date = Helpers.License.CreateDateTime;
                _status = Helpers.LicenseStatus;
                var log = JObject.Parse(Helper.Decrypt(File.ReadAllText(license_log)));
                var last_check = log.Value<DateTime>("last_check");
                var attempts = log.Value<int>("attempts");

                try
                {
                    last_check = Helper.GetHora();
                    attempts = 0;
                }
                catch (Exception ex)
                {
                    attempts += 1;
                }
                switch (Helpers.License.Type)
                {
                    case LicenseTypes.TKT:
                        if (License.ValidUntil < last_check || attempts > 5)
                            _status = LicenseStatus.TRIALEXPIRED;
                        break;
                    default:
                        if (License.ValidUntil < last_check || attempts > 10)
                            _status = LicenseStatus.TRIALEXPIRED;
                        break;
                }
                var logNew = JObject.FromObject(new { last_check = last_check, attempts = attempts });
                File.WriteAllText(license_log, Helper.Encrypt(logNew.ToString()));

                switch (_status)
                {
                    case LicenseStatus.VALID:
                        return;
                    case LicenseStatus.TRIALEXPIRED:
                        await WarnMessage("Período de testes expirou, efetue o registro novamente para usar o sistema.", "Registro expirado");
                        Application.Current.MainPage = new LicensePage();
                        break;
                    default:
                        Application.Current.MainPage = new LicensePage();
                        break;
                }
            }
            else
            {
                await ErrorMessage("Não foi possível validar a licença, verifique o acesso a internet e tente novamente.", "Licença não verificada");
                Environment.Exit(0);
            }

        }
        public static async Task<bool> GetLicense(string doc, string uuid, string password, bool new_registry = true)
        {
            ERPLicense license = new ERPLicense();
            string save_folder = DependencyService.Get<IAppPath>().Path;
            string license_file = Path.Combine(save_folder, "license.lic");
            string license_log = Path.Combine(save_folder, "license_log");
            var status = LicenseStatus.UNDEFINED;
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(Helper.API_URI);
                var licenseReq = new
                {
                    AppName = "AppColetaMobile",
                    Doc = doc,
                    UUID = uuid,
                    Password = password,
                    New = new_registry,
                    Type = Helper.TKT ? LicenseTypes.TKT : LicenseTypes.Single
                };

                var data = JsonConvert.SerializeObject(licenseReq);
                var request = await client.PostAsync("api/licenses", new StringContent(data, Encoding.UTF8, "application/json"));
                var response = await request.Content.ReadAsStringAsync();
                if (request.StatusCode == HttpStatusCode.OK)
                {
                    byte[] public_key;
                    //Read public key from assembly
                    Assembly _assembly = Assembly.GetExecutingAssembly();
                    using (MemoryStream _mem = new MemoryStream())
                    {
                        _assembly.GetManifestResourceStream("AppColeta.ellitedevDDNS.cer").CopyTo(_mem);
                        public_key = _mem.ToArray();
                    }
                    license = LicenseHandler.ParseLicenseFromBASE64String(typeof(ERPLicense), response, public_key, out status) as ERPLicense;
                }
                else if (request.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await WarnMessage(response, "Não licenciado");
                }
                switch (status)
                {
                    case LicenseStatus.VALID:
                        var log = JObject.FromObject(new { last_check = license.CreateDateTime, attempts = 0 });
                        File.WriteAllText(license_log, Helper.Encrypt(log.ToString()));
                        //If license if valid, save the license string into a local file
                        File.WriteAllText(license_file, response);
                        await InfoMessage("Licença instalada com sucesso.", "Sistema licenciado");
                        return true;
                    case LicenseStatus.INVALID:
                        await WarnMessage("Licença inválida! Entre em contato com o suporte para regularizar a situação.", "Não licenciado");
                        break;
                    case LicenseStatus.CRACKED:
                        await ErrorMessage("Verificado uma alteração ilegal na licença", "Cópia ilegal");
                        break;
                    case LicenseStatus.TRIALEXPIRED:
                        await WarnMessage("Licença expirada! Entre em contato com o suporte para efetuar uma renovação.", "Não licenciado");
                        break;
                    default:
                        throw new Exception("Ocorreu um erro inesperado ao buscar a licença");
                }
            }
            catch (Exception exc)
            {
                await ErrorMessage("Erro ao gerar licença. \n" + exc.Message + "\n" + exc.StackTrace, ".: ERRO FATAL :.");
            }

            return false;
        }
    }
}
