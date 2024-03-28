using Microsoft.AspNetCore.Mvc;
using PasswordPics.data;
using PasswordPics.web.Models;
using System.Diagnostics;
using System.Text.Json;

namespace PasswordPics.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string _connectionString = @"Data Source=.\sqlexpress; Initial Catalog=Images; Integrated Security=true;";
       

        public HomeController(IWebHostEnvironment webHostEnviroment)
        {
            _webHostEnvironment = webHostEnviroment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Submit(IFormFile image, string password)
        {
            ImageRepository ir = new ImageRepository(_connectionString);

            Image here = new Image
            {
                ImageTitle = image.FileName,
                Password = password,
                ImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", image.FileName)
            };

            using FileStream fs = new FileStream(here.ImagePath, FileMode.Create);
            image.CopyTo(fs);

            int id = ir.Add(here);

            SubmitViewModel svm = new SubmitViewModel { Image = ir.GetByID(id) };

            return View(svm);
        }

        private static int _number;
        public IActionResult ViewImage(int id)
        {
            ImageRepository ir = new ImageRepository(_connectionString);
            SubmitViewModel svm = new SubmitViewModel { Image = ir.GetByID(id) };

            //------session----------

            int? number = HttpContext.Session.GetInt32("number");
            int count = 0;

            if (number != null)
            {
                count = number.Value;
            }

            svm.SessionCount = count;

            HttpContext.Session.SetInt32("number", count + 1);

            //why does it make sense that count++ does not work but count + 1 does?

            //-----------session try again-----------

            //int? sessionNum = HttpContext.Session.GetInt32("number");

            //if(sessionNum == null)
            //{
            //    sessionNum = 1;
            //}

            //HttpContext.Session.SetInt32("number", sessionNum.Value + 1);

            //svm.SessionCount = sessionNum.Value;

            //---------mine-----------

            string cookieNumber = Request.Cookies["number"];
            int cookieCount = 1;

            if (cookieNumber != null)
            {
                cookieCount = int.Parse(cookieNumber);
            }

            //string c = (cookieCount++).ToString();
            //Console.WriteLine(c);
            //why wouldn't this version up here work? if i uncomment it, my cookie counter
            //jumped twice as fast as my static counter, but if i plug it in to my appended
            //cookies, it does not work. what in the world? why not? figure out, maybe
            //also, what does ++ do? doesn't it add 1? so why when i do cookiecount++ does it 
            //not work and start at 2?

            Response.Cookies.Append("number", $"{cookieCount + 1}");

            svm.CookieCount = cookieCount;

            //----------his------------

            //var hisCookieNumber = Request.Cookies["number"];
            //int hisCookieCount = 1;

            //if (hisCookieNumber != null)
            //{
            //    hisCookieCount = int.Parse(hisCookieNumber);
            //}

            //Response.Cookies.Append("number", $"{hisCookieCount + 1}");
            //svm.CookieCount = hisCookieCount;

            //-------static--------------

            svm.StaticCount = _number;

            _number++;

            return View(svm);
        }
    }

    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonSerializer.Deserialize<T>(value);
        }
    }
}