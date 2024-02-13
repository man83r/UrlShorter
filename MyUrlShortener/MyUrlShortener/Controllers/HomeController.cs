using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyUrlShortener.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyUrlShortener.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UrlShortenerContext _context;

        public HomeController(ILogger<HomeController> logger, UrlShortenerContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GoShortUrl(string id)
        {
            if (id.Length == 4)
            {
                string shortUrl = id;
                var findShotUrl = _context.UrlShorts.FirstOrDefault(x => x.ShortUrl == shortUrl);
                if (findShotUrl != null)
                {
                    return Redirect(findShotUrl.UserUrl);
                }
                else
                {
                    ViewBag.Message = "Извините, запрашиваемая короткая ссылка не сушествует.";
                    return View();
                }
            }
            else
            {
                ViewBag.Message = "Неверный формат короткой ссылки. Ссылка должна содержать 4 символа. Разрешеннные символы: 0-9, A-Z, a-z.";
                return View();
            }
        }

        public IActionResult ConvertToShort(string userUrl)
        {
            if (ValidationUrl(userUrl))
            {
                var lastUrl = _context.UrlShorts.FirstOrDefault();
                if (lastUrl == null)
                {
                    var newUrl = new UrlShort();
                    newUrl.ShortUrl = "0001";
                    newUrl.UserUrl = userUrl;
                    _context.UrlShorts.Add(newUrl);
                    _context.SaveChanges();
                    return Json(newUrl.ShortUrl);
                }
                var findShotUrl = _context.UrlShorts.FirstOrDefault(x => x.UserUrl == userUrl);
                if (findShotUrl == null)
                {
                    lastUrl = _context.UrlShorts.OrderByDescending(x => x.Id).FirstOrDefault();
                    if (lastUrl.ShortUrl == "zzzz")
                    {
                        return Json("Извините, сервис больше не создаёт новых коротких ссылок.");
                    }
                    findShotUrl = new UrlShort();
                    findShotUrl.ShortUrl = GetShortUrl(lastUrl.ShortUrl);
                    findShotUrl.UserUrl = userUrl;
                    _context.UrlShorts.Add(findShotUrl);
                    _context.SaveChanges();
                }
                return Json(findShotUrl.ShortUrl);
            }
            else
            {
                return Json("Извините, Вы ввели неправильный адрес. Сайт по указанному адресу не существует.");
            }
        }

        private bool ValidationUrl(string userUrl)
        {
            try
            {
                Uri uri = new Uri(userUrl);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch
            {
                return false;
            }
            return true;
        }

        private string GetShortUrl(string lastShortUrl)
        {
            byte[] shortUrlbyte = lastShortUrl.Select(c => (byte)c).ToArray();
            for (int i = shortUrlbyte.Length-1; i >=0; i--)
            {
                if (shortUrlbyte[i] == 122)          //если разряд имеет максимальное значение, то сбросим в ноль
                {                                   //и перейдем к следующему
                    shortUrlbyte[i] = 48;            //соответствует 0
                    continue;
                }
                if (shortUrlbyte[i] >= 48 & shortUrlbyte[i] < 57)
                {
                    shortUrlbyte[i]++;
                    break;  //увеличим на 1 и прервём выполнение цикла
                }
                if (shortUrlbyte[i] == 57)
                {
                    shortUrlbyte[i] = 65;
                    break;
                }
                if (shortUrlbyte[i] >= 65 & shortUrlbyte[i] < 90)
                {
                    shortUrlbyte[i]++;
                    break;  //увеличим на 1 и прервём выполнение цикла
                }
                if (shortUrlbyte[i] == 90)
                {
                    shortUrlbyte[i] = 97;
                    break;  //увеличим на 1 и прервём выполнение цикла
                }
                if (shortUrlbyte[i] >= 97 & shortUrlbyte[i] < 122)
                {
                    shortUrlbyte[i]++;
                    break;  //увеличим на 1 и прервём выполнение цикла
                }
            }
            string result = System.Text.Encoding.Default.GetString(shortUrlbyte);
            return result;
        }

    }
}
