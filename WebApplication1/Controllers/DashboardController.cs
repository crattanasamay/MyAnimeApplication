using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Clients;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.AnimeApi;
using WebApplication1.Models.JikanAnimeApi;
using Anime = WebApplication1.Models.Anime;
using MyAnime = WebApplication1.Models.AnimeApi.Anime;

namespace WebApplication1.Controllers
{

    [Authorize(Policy = "RequireUserRole")]

    public class DashboardController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly IJikanApiClient _jikanAnimeClient;
        private readonly IMyAnimeClient _myAnimeClient;


        public DashboardController(ApplicationDbContext db, IJikanApiClient jikanAnimeClient, IMyAnimeClient myAnimeClient)
        {
            _db = db;
            _jikanAnimeClient = jikanAnimeClient;
            _myAnimeClient = myAnimeClient;
        }

        [Route("Dashboard")]

        public async Task<IActionResult> Index()
        {
            try
            {
                List<string> seasons = new(){"Winter","Spring","Summer","Fall"};
                AnimeNavigationModel model = new(){
                    Seasons = seasons,
                    CurrentYear = 2022
                };


                // if user is authenticated make sure to login
                if (User.Identity.IsAuthenticated)
                {
                    return View(model);
                }
                // else delete their cookie and redirect to login page
                await HttpContext.SignOutAsync("MyCookieAuth");

                return RedirectToAction("Login", "UserManagement");
            }
            catch(Exception e)
            {

            }

            return View("Error");


        }



        [HttpPost]
        [Route("AnimeSeason")]


        public async Task<IActionResult> AnimeSeason(string? id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    // handle null
                }
                int index = id.IndexOf('_');
                string season = id.Substring(0,index).ToLower();
                string year = id.Substring(index + 1);
                JikanSeasonModel anime = await _jikanAnimeClient.GetAnimeBySeason(season, year);
                if(anime == null)
                {
                    return View("Dashboard");
                }
          
                List<Datum> animeList = _jikanAnimeClient.FilterAnime(anime);
               return PartialView("_AnimeSeasonPartial",animeList);
            }
            catch(Exception ex)
            {

                // handle exception
            }
            return View("Error");
        }

        [HttpPost]
        [Route("AnimeHistory")]
        public async Task<IActionResult> AnimeHistory()
        {
            try
            {
                List<UserAnime> userHistoryList = _db.UserAnime.Where(u => u.UserName == User.Identity.Name).ToList();
                List<AnimeHistoryModel> animeHistoryList = new List<AnimeHistoryModel>();
                foreach(var item in userHistoryList)
                {
                    SingleAnimeModel obj = await _myAnimeClient.GetSingleAnimeInfo(item.AnimeId);
                    animeHistoryList.Add(new AnimeHistoryModel
                    {
                        AnimeId = obj.id,
                        AnimeImg = obj.main_picture.medium,
                        AnimeRating = obj.mean,
                        AnimeSeason = $"{char.ToUpper((obj.start_season.season)[0])}{(obj.start_season.season).Substring(1)} {obj.start_season.year}",
                        AnimeTitle = obj.title
                    });
                }
                return PartialView("_AnimeHistoryPartial",animeHistoryList);
            }
            catch(Exception e)
            {
                // Handle Error
            }
            return View("Error");
        }




        [HttpPost]
        [Route("AnimeLikedPartial")]
        public async Task<IActionResult> AnimeLikedPartial()
        {
            try
            {
                List<UserAnime> row = _db.UserAnime.Where(x => x.UserName == User.Identity.Name).ToList();
                int counterAbove = 0;
                int counterBelow = 0;
                foreach (var obj in row)
                {
                    var check = await _myAnimeClient.GetAnimeRating(obj.AnimeId);
                    if (check > 7.00){
                        counterAbove++;
                    }
                    else{
                        counterBelow++;
                    }
                }
                int[] list = { counterAbove, counterBelow };
                return PartialView("_AnimeLikedPartial", list);

            }
            catch(Exception e)
            {

            }
            return View("Error");
          
        }

        // Like button add to database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LikeAnime(int id)
        {
            try
            {

                if(User.Identity == null)
                {
                    return View("Index", "Home");
                }
                var row = _db.UserAnime.Where(x => x.UserName == User.Identity.Name && x.AnimeId == id).ToList();
                // SQL QUERY Select * FROM UserAnime WHERE UserName = $"{User.Identity.Name} AND AnimeId = id"
                if (!row.Any())
                {
                    UserAnime obj = new()
                    {
                        UserName = User.Identity.Name,
                        AnimeId = id,
                        AnimeDateAdded = DateTime.Now,
                        Rating = await _myAnimeClient.GetAnimeRating(id),
                       

                    };
                    await _db.UserAnime.AddAsync(obj);
                    _db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("LikeButton", "Already Liked");
                }
                if (User.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Dashboard");
                else
                {
                    return View("Index", "Dashboard");
                }
            }
            catch(Exception e)
            {
                return View("Error");
            }
        }
    }  
}
