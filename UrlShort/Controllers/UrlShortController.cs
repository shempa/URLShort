using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrlShort.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace UrlShort.Controllers
{
    public class UrlShortController : Controller
    {
        UrlContext db;

        public UrlShortController(UrlContext context)
        {
            db = context;
        }
        
        public ActionResult AddUrl()
        {
            return View();
        }

        public ActionResult DelUrl(int id)
        {
            Url url = db.Urls.Find(id);
            db.Urls.Remove(url);
            db.SaveChanges();
            return Redirect("~/UrlShort/GetUrls");
        }

        public ActionResult EditUrl(int id)
        {
            Url url = db.Urls.Find(id);
            return View(url);
        }

        public ActionResult NewUrl(Url url)
        {
            db.Entry(url).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectPermanent("~/UrlShort/GetUrls");
        }

        public ActionResult GetUrls()
        {
            return View(db.Urls);
        }


        public ActionResult SavUrl(Uri UrlLong)
        {
            Url url = new Url();
            string shorte = GenerateShortUrl(); //вызов генератора случайного URL

            string urlLong = UrlLong.ToString();
            var arrByte = ASCIIEncoding.ASCII.GetBytes(urlLong);  
            var arrHash =new MD5CryptoServiceProvider().ComputeHash(arrByte);
            var hash=ByteArrayToString(arrHash);
            url.Hash = hash;

            Url repeatUrl = db.Urls.FirstOrDefault(h => h.Hash == url.Hash);
            if(repeatUrl!=null)
            {
                return Redirect("https://localhost:5001/Home/Privacy");
            }
            else
            {
                url.UrlLong = UrlLong;
                url.Date = DateTime.Now;
                db.Urls.Add(url);
                db.SaveChanges();

            }
            return Redirect("~/UrlShort/GetUrls");
        }

        public static string ByteArrayToString(byte[] arrInput)
        {
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (int i=0;i<arrInput.Length;i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        public static string GenerateShortUrl()
        {
            string urlsafe = string.Empty;
            Enumerable.Range(48, 75)
              .Where(i => i < 58 || i > 64 && i < 91 || i > 96)
              .OrderBy(o => new Random().Next())
              .ToList()
              .ForEach(i => urlsafe += Convert.ToChar(i));
            string token = urlsafe.Substring(new Random().Next(0, urlsafe.Length), new Random().Next(2, 6));
            return token;
        }
    }
}