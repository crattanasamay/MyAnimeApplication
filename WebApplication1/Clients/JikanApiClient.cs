using WebApplication1.Models.JikanAnimeApi;
using RestSharp;
using Microsoft.DotNet.MSIdentity.Shared;
using Newtonsoft.Json;

namespace WebApplication1.Clients
{
    public class JikanApiClient  : IJikanApiClient
    {
        public JikanApiClient()
        {
      
        }

        public async Task<JikanSeasonModel> GetAnimeBySeason(string season,string year)
        {
            var b = new JikanSeasonModel();

            try
            {
                string url = $"https://api.jikan.moe/v4/seasons/{year}/{season}";
                var client = new RestClient(url);
                var request = new RestRequest();

                var response = await client.GetAsync(request);

                var body = JsonConvert.DeserializeObject<JikanSeasonModel>(response.Content);

                return body;
            }
            catch(Exception e)
            {
                // return exception
            }
            return b;

        }

        public List<Datum> FilterAnime(JikanSeasonModel anime)
        {
            try
            {
                List<Datum> animeList = anime.data;

                for (int i = 0; i < animeList.Count; i++)
                {
                    var animeItem = animeList[i]; ;

                    if (animeItem.rating == "Rx - Hentai")
                    {
                        animeList.RemoveAt(i);
                    }
                }
                return animeList;

            }
            catch (Exception e)
            {
                // return some error
            }
            return null;
        }
    }

    public interface IJikanApiClient
    {
        public Task<JikanSeasonModel> GetAnimeBySeason(string season,string year);

        public List<Datum> FilterAnime(JikanSeasonModel obj);
    }
}
