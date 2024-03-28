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

        public IActionResult ViewImage(int id)
        {
            ImageRepository ir = new ImageRepository(_connectionString);
            SubmitViewModel svm = new SubmitViewModel { Image = ir.GetByID(id) };

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