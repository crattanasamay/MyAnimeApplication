namespace WebApplication1.Models
{
    public class AnimeNavigationModel
    {
        public List<string> Seasons { get; set; }  = new(){ "Fall", "Summer", "Spring", "Winter" };

        public int CurrentYear { get; set; } = 2022;

        public List<string> newSeasons { get; set; } = new() { "Winter","Spring","Summer","Fall"};

        public int newCurrentYear { get; set; } = 2023;

        public string currentSeason = "Winter";
    }
}
