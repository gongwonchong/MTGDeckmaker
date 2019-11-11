using System.Net;
using Android.App;
using Android.OS;
using Android.Content;
using Java.Interop;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scryfall;

namespace App2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.main);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ImageButton search = FindViewById<ImageButton>(Resource.Id.imageButton2);
            EditText sText = FindViewById<EditText>(Resource.Id.editText1);
            search.Click += delegate
            {
                try
                {
                    string req = "https://api.scryfall.com/cards/search?q=" + sText.Text + "&include_multilingual=true";
                    JObject data = Common.GetSrc(req);
                    var intent = new Intent(this, typeof(SearchActivity));
                    intent.PutExtra("CardResult", JsonConvert.SerializeObject(data));
                    StartActivity(intent);
                }
                catch (WebException e)
                {
                    Toast.MakeText(Application.Context, e.Message, ToastLength.Long).Show();
                }
                catch (System.Exception e)
                {
                    Toast.MakeText(Application.Context, e.Message, ToastLength.Long).Show();
                }
            };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}