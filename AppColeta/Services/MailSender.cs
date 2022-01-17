using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SOColeta.Services
{
    public abstract class MailSender
    {
        public static async Task SendEmail(string subject, string body, List<string> recipients, string filename = "")
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                if (!string.IsNullOrEmpty(filename))
                    message.Attachments.Add(new EmailAttachment(filename));
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                await Shell.Current.DisplayAlert("Não suportado.", "O envio de email não é suportado nesse dispositivo.", "OK");
                Debug.WriteLine(fbsEx.Message);
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }
    }
}
