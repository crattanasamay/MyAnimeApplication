using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using WebApplication1.Clients;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.AnimeApi;
using Anime = WebApplication1.Models.Anime;

namespace WebApplication1.Controllers
{
    public class AnimeController : Controller
    {

        private readonly ApplicationDbContext _db;
       

        public AnimeController(ApplicationDbContext db, IAnimeClient animeClient)
        {
            _db = db;
        }

        public IActionResult Index()
        {
        
            IEnumerable<Anime> objAnimeList = _db.Anime;
            return View(objAnimeList);
        }

        //GET Action
        public IActionResult Create()
        {
            
            return View();
        }
        //POST Action
        [HttpPost]
        [ValidateAntiForgeryToken] //Help Cross Site Scripting
        public IActionResult Create(Anime obj)
        {
            if(obj.Name == obj.Season.ToString())
            {
                ModelState.AddModelError("Name", "Anime name is the same as Anime Season.");
            }
            if (ModelState.IsValid)
            {
                _db.Anime.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
                //If you want to go to a different controller return RedirectToAction("{Method}", "{Controller}");
            }
            return View();

        }


      
    }
}
