using Newtonsoft.Json;
using WebApplication1.Models.AnimeApi;
using MyAnimeListRoot = WebApplication1.Models.MyAnimeClientApiModels.Root;
using MyAnime = WebApplication1.Models.AnimeApi.Anime;
using WebApplication1.Models;
using System.Runtime.CompilerServices;
using Polly.Retry;
using Polly;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;

namespace WebApplication1.Clients
{
    public class MyAnimeClient : IMyAnimeClient

    {
        private const string MyAnimeListUrl = "X-MAL-CLIENT-ID";



        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly AsyncRetryPolicy _asyncRetryPolicy;
        private readonly AsyncRetryPolicy _asyncRetryPolicyTime;

        public MyAnimeClient(HttpClient httpClient,IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
            _asyncRetryPolicy = Policy.Handle<HttpRequestException>().RetryAsync(3);
            _asyncRetryPolicyTime = Policy.Handle<HttpRequestException>().WaitAndRetryAsync(3, time => TimeSpan.FromSeconds(3));
        }

        public async Task<Double> GetAnimeRating(int id)
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

                return await _asyncRetryPolicyTime.ExecuteAsync(async () =>
                {
                    using var response = await _httpClient.SendAsync(request);
                    {
                        var body = await response.Content.ReadAsStringAsync();
                        dynamic? json = JsonConvert.DeserializeObject<MyAnimeListRoot>(body);
                        if (json == null) return 0.00;
                        return json.mean;
                    }

                });
               
            }
            catch(Exception e)
            {
                throw;
                /// Doing Something
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

                return await _asyncRetryPolicyTime.ExecuteAsync(async () =>
                {
                    using var response = await _httpClient.SendAsync(request);
                    {
                        response.EnsureSuccessStatusCode();
                        var body = await response.Content.ReadAsStringAsync();
                        dynamic? json = JsonConvert.DeserializeObject<SingleAnimeModel>(body);
                        if (json == null) return new SingleAnimeModel(); // return an empty model
                        return json;
                    };

                });
               
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
        public Task<Double> GetAnimeRating(int id);
        public Task<SingleAnimeModel> GetSingleAnimeInfo(int id);
    }

}
