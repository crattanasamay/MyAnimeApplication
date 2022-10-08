using Newtonsoft.Json;
using WebApplication1.Models.AnimeApi;
using MyAnimeListRoot = WebApplication1.Models.MyAnimeClientApiModels.Root;
using MyAnime = WebApplication1.Models.AnimeApi.Anime;
using WebApplication1.Models;
using System.Runtime.CompilerServices;

namespace WebApplication1.Clients
{
    public class MyAnimeClient : IMyAnimeClient

    {
        private const string MyAnimeListUrl = "X-MAL-CLIENT-ID";



        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public MyAnimeClient(HttpClient httpClient,IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<Boolean> GetAnimeRating(int id)
        {
            try
            {
                string animeUrl = $"https://api.myanimelist.net/v2/anime/{id}?fields=mean"; //get anime rating tbh idk why it's mean but w/e
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(animeUrl),
                    Headers =
                    {
                        {MyAnimeListUrl, _config["MyAnimeListApiKey"]  }
                    }
                };

                using var response = await _httpClient.SendAsync(request);
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    dynamic? json = JsonConvert.DeserializeObject<MyAnimeListRoot>(body);
                    if (json == null) return false;
                    if (json.mean > 7.00)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch(Exception e)
            {
                return false;
            }
        
        }

 
        public async Task<SingleAnimeModel> GetSingleAnimeInfo(int id)
        {

            try
            {
                string animeUrl = $"https://api.myanimelist.net/v2/anime/{id}?fields=id,title,main_picture,alternative_titles,start_date,end_date,synopsis,mean,rank,popularity,num_list_users,num_scoring_users,nsfw,created_at,updated_at,media_type,status,genres,my_list_status,num_episodes,start_season,broadcast,source,average_episode_duration,rating,pictures,background,related_anime,related_manga,recommendations,studios,statistics";
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(animeUrl),
                    Headers =
                        {
                            {MyAnimeListUrl, _config["MyAnimeListApiKey"]}
                        }

                };
                using var response = await _httpClient.SendAsync(request);
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    dynamic? json = JsonConvert.DeserializeObject<SingleAnimeModel>(body);
                    return json;
                };
            }

            catch (Exception e)
            {
                // Handle Error
            }
            return null;

        }
    }

  

    public interface IMyAnimeClient
    {
        public Task<Boolean> GetAnimeRating(int id);
        public Task<SingleAnimeModel> GetSingleAnimeInfo(int id);
    }

}
