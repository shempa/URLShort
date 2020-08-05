using System;
namespace UrlShort.Models
{
    public class Url
    {
        public int Id { get; set; }

        public Uri UrlLong { get; set; }
        public Uri UrlShort { get; set; }
        public string Hash { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        
        
    }
}
