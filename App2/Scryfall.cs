using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Scryfall
{
    public class WrongDataTypeExcpetion : System.Exception
    {
        public WrongDataTypeExcpetion() : base() { }
        public WrongDataTypeExcpetion(string message) : base(message) { }
        public WrongDataTypeExcpetion(string message, System.Exception inner) : base(message, inner) { }
    }
    class Common
    {
        public static JObject GetSrc(string requesturi)
        {
            WebRequest request = WebRequest.Create(requesturi);
            WebResponse response = request.GetResponse();
            StreamReader t = new StreamReader(response.GetResponseStream());
            return JObject.Parse(t.ReadToEnd());
        }
    }
    class Card
    {
        public string PrintedName { get; }
        public string Id { get; }
        public DateTime ReleasedAt { get; }
        public string imageurlnormal { get; }

        public Card(JToken src)
        {
            if ((string)src["object"] != "card")
            {
                throw new WrongDataTypeExcpetion();
            }
            this.PrintedName = (string)src["printed_name"];
            this.Id = (string)src["id"];
            string[] ReleaseDate = ((string)src["released_at"]).Split('-');
            this.ReleasedAt = new DateTime(int.Parse(ReleaseDate[0]), int.Parse(ReleaseDate[1]), int.Parse(ReleaseDate[2]));
            this.imageurlnormal = ((string)src["image_uris"]["normal"]).Split('?')[0];
        }
        public static List<Card> Cards(JObject src)
        {
            List<Card> result = new List<Card>();
            for (int i = 0; i < src["data"].Count(); i++)
            {
                JToken t = src["data"][i];
                result.Add(new Card(t));
            }
            return result;
        }
    }
}