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
using Genre = WebApplication1.Models.SingleAnimeModel;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Newtonsoft.Json;
using WebApplication1.Models.ChartModels.TopOneHundredBarModel;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{

    [Authorize(Policy = "RequireUserRole")]

    public class DashboardController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly IJikanApiClient _jikanAnimeClient;
        private readonly IMyAnimeClient _myAnimeClient;
        private readonly ILogger<DashboardController> _logger;
        private readonly IConfiguration _configuration;


        public DashboardController(ApplicationDbContext db, IJikanApiClient jikanAnimeClient, IMyAnimeClient myAnimeClient, ILogger<DashboardController> logger, IConfiguration _iconfig)
        {
            _db = db;
            _jikanAnimeClient = jikanAnimeClient;
            _myAnimeClient = myAnimeClient;
            _logger = logger;
            _configuration = _iconfig;
        }

        [Route("Dashboard")]

        public async Task<IActionResult> Index()
        {
            try
            {
                AnimeNavigationModel model = new();


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
                // Fix this so DB sorts between below 7 and above 7 return list of count above 7 and below 7
                if(User.Identity == null) { return null; }

                string conn_string = _configuration["ConnectionStringAzure"];
                using (var conn = new SqlConnection(conn_string))
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "spUserAnime_GetSevenCount";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", User.Identity.Name);
                    conn.Open();
                    var result = cmd.ExecuteReader();


                    if(result.Read())
                    {
                        var under_seven = result.GetInt32(0);
                        var above_seven = result.GetInt32(1);
                        var total_liked = result.GetInt32(2);

                        var r = new LikedAnimeSeven
                        {
                            above = above_seven,
                            below = under_seven,
                            total = total_liked,
                        };
                        result.Close();
                        return PartialView("_AnimeLikedPartial", r);
                    }
                   
                
                  
                    
                }
                // List<UserAnime> row = _db.UserAnime.Where(x => x.UserName == User.Identity.Name).ToList();
                //int counterAbove = 0;
                //int counterBelow = 0;
                //foreach (var obj in row)
                //{
                    
                //    if (obj.Rating > 7.00){
                //        counterAbove++;
                //    }
                //    else{
                //        counterBelow++;
                //    }
                //}
                //int[] list = { counterAbove, counterBelow, counterAbove+counterBelow };
                //return PartialView("_AnimeLikedPartial", list);

            }
            catch(Exception e)
            {

            }
            return View("Error");
          
        }

        // Like button add to database
        [HttpPost]
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
                    var getAnimeInfo = await _myAnimeClient.GetSingleAnimeInfo(id);
                    var getGenreList = getAnimeInfo.genres;
                    string genreString = "";
                    foreach(var genre in getGenreList)
                    {
                        genreString += $"{genre.name},";
                    }
                    UserAnime obj = new()
                    {

                        UserName = User.Identity.Name,
                        AnimeId = id,
                        AnimeDateAdded = DateTime.Now,
                        Rating = await _myAnimeClient.GetAnimeRating(id),
                        genres = genreString,

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


        [HttpPost]
        [Route("UserAnimeChart")]
        public async Task<IActionResult> UserAnimeChartPartial()
        {
            try
            {

                List<UserAnime> row = _db.UserAnime.Where(x => x.UserName == User.Identity.Name).ToList();
                //List<SingleAnimeModel> myList = new List<SingleAnimeModel>();
                Dictionary<string, int> genreIndex = new();
                List<UserAnimeChartModel> myList = new();
                foreach (var item in row)
                {
                    // should fix this to do in the DB not during runtime
                    string[] genreList = _myAnimeClient.SplitGenreString(item.genres);
                    string[] genreListResult = genreList.Take(genreList.Length - 1).ToArray();
                   
                    foreach (var genre in genreListResult)
                    {
                        Console.WriteLine(genre);
                        //if the genre already exists in list 
                        if (genreIndex.ContainsKey(genre))
                        {
                            // grab the index inside genreIndex and add counter
                            myList[genreIndex[$"{genre}"]].counter++;
                        }
                        else
                        {
                            genreIndex[$"{genre}"] = myList.Count();
                            myList.Add(new UserAnimeChartModel
                            {
                                counter = 1,
                                genre = genre,
                            });
                        }
                    }
                }
               
                return PartialView("_UserAnimeLikeChartPartial", myList);
            }
            catch(Exception e)
            {
                return View();
            }

        }

        [HttpPost]
        [Route("AnimeSeasonGenreChartNav")]

        public IActionResult AnimeSeasonGenreNav() 
        {

            AnimeNavigationModel model = new();

            return PartialView("_SeasonalAnimeGenreCard",model);
        }

        [HttpPost]
        [Route("AnimeSeasonGenreChart")]

        public async Task<IActionResult> AnimeSeasonGenreChart(string? id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    _logger.LogDebug("anime id retrieved is ");

                    throw new ArgumentNullException(nameof(id));
                }
                int index = id.IndexOf('_');
                string season = id.Substring(0, index);
                string year = id.Substring(index + 1);
                var animeSeasonList = await _jikanAnimeClient.GetAnimeBySeason(season,year);


                return PartialView("_SeasonalAnimeGenreChart");
            }
            catch (Exception e)
            {

                return View("error");

            }

            return View("error");



        }

        
        [HttpPost]
        [Route("AverageScoreTopAnimeChart")]
        public async Task<IActionResult> AverageScoreTopAnimeChart()
        {
            try
            {
                var myChartModel = await _myAnimeClient.GetTopOneHundredAnime();
                List <TopOneHundredBarModel> myList= new(); 
                Dictionary<int, int> checkList = new(); // Stores the Year key and Index inside myList
                foreach(var obj in myChartModel)
                {
                    if (checkList.ContainsKey(obj.Year)){
                        myList[checkList[obj.Year]].counter++;
                        myList[checkList[obj.Year]].animeSumOverAll += obj.Rating;
                    }
                    else
                    {
                        checkList.Add(obj.Year, myList.Count()); // Add Year and Index
                        myList.Add(new TopOneHundredBarModel
                        {
                            counter = 1,
                            Year = obj.Year,
                            animeSumOverAll = obj.Rating,

                        }) ; 
                    }
                }
                myList.ToList().ForEach(c => c.mean = c.animeSumOverAll / c.counter);
                List<TopOneHundredBarModel> sortedList = myList.
                    OrderBy(x => x.Year).ToList();
                return PartialView("_TopAnimeChartPartial",sortedList);

            }
            catch (Exception e)
            {
                /// Return Some Error
            }

            return PartialView("_UserAnimeLikeChartPartial");
        }


        [HttpPost]
        [Route("RemoveUserAnime")]
        public async Task<IActionResult> RemoveUserAnime(int id)
        {

            var row = (from x in _db.UserAnime where x.UserName == User.Identity.Name && x.AnimeId == id select x).First();
            if(row != null)
            {
                _db.UserAnime.Remove(row);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Dashboard");



        }


    }  
}
