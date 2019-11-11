using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Scryfall;
using System;
using System.Collections.Generic;
using System.Net;

namespace App2
{
    [Activity(Label = "SearchActivity")]
    public class SearchActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.result);
                var data = JsonConvert.DeserializeObject<JObject>(Intent.GetStringExtra("CardResult"));
                var data_result = Card.Cards(data);
                var t = new List<string>();
                data_result.ForEach(delegate (Card c)
                {
                    t.Add(c.imageurlnormal);
                });
                // Create your application here
                var CardImageGrid = FindViewById<GridView>(Resource.Id.gridView1);
                CardImageGrid.Adapter = new CardAdapter(this, t);
            }
            catch (System.Exception e)
            {
                Toast.MakeText(Application.Context, e.Message, ToastLength.Long).Show();
                this.Finish();
            }
        }
    }
    public class CardAdapter : BaseAdapter
    {
        Context context;
        List<string> ori;

        public CardAdapter(Context c, List<string> src)
        {
            ori = src;
            context = c;
        }
        public override Java.Lang.Object GetItem(int position) => null;
        public override int Count => ori.Count;
        public override long GetItemId(int position) => 0;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ImageView imageView;

            if (convertView == null)
            {  // if it's not recycled, initialize some attributes
                imageView = new ImageView(context);
            }
            else
            {
                imageView = (ImageView)convertView;
            }
            imageView.SetImageBitmap(GetImageFromUri(ori[position]));
            return imageView;
        }
        public Bitmap GetImageFromUri(string url)
        {
            Bitmap imageBitmap = null;
            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }
            return imageBitmap;
        }
    }
}