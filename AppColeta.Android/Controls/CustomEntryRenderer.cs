using Android.Content;
using SOColeta.Droid.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace SOColeta.Droid.Controls
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Color color = Application.Current.RequestedTheme == OSAppTheme.Dark 
                    ? (Color)Application.Current.Resources["Gray600"] 
                    : (Color)Application.Current.Resources["Gray200"];
                Control.SetBackgroundColor(color.ToAndroid());
            }
        }
    }
}