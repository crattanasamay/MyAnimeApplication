using Newtonsoft.Json;
using WebApplication1.Models.AnimeApi;

namespace WebApplication1.Clients
{
    public class AnimeClient : IAnimeClient
    {

        private const string AnimeListAuthorization = "X-RapidAPI-Key";
        private const string AnimeListHost = "X-RapidAPI-Host";
        private const string AnimeListHostUrl = "jikan1.p.rapidapi.com";


        private const string baseUrl = "https://jikan1.p.rapidapi.com/";

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;


        public AnimeClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<Root?> GetAnimeBySeason(string season, string year)
        {
            try
            {
                string animeUrl = $"{baseUrl}season/{year}/{season}";
                //string animeUrl = "https://jikan1.p.rapidapi.com/season/2022/spring";
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(animeUrl),
                    Headers =
                    {
                        { AnimeListAuthorization, _config["JikanApiKey"]},
                        { AnimeListHost, AnimeListHostUrl },
                    },
                };
                using (var response = await _httpClient.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject<Root>(body);
                    return json;
                };
            }
            catch(Exception e)
            {
                return null;
            }
            
        }


        public async Task<Boolean> GetAnimeRatingSeven(int id)
        {
            try
            {
                string animeUrl = $"{baseUrl}/anime/{id}";
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(animeUrl),
                    Headers =
                    {
                        { AnimeListAuthorization, _config["JikanApiKey"]},
                        { AnimeListHost, AnimeListHostUrl },
                    }
                };
                using (var response = await _httpClient.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    return true;
                };
            }
            catch(Exception e)
            {
                // find exception here
            }
            return false;
        }


      


    }

    public interface IAnimeClient
    {
        public Task<Root?> GetAnimeBySeason(string season,string year);

        public Task<Boolean> GetAnimeRatingSeven(int id);
    }
}
