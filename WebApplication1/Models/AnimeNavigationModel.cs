namespace WebApplication1.Models
{
    public class AnimeNavigationModel
    {
        public List<string> Seasons { get; set; }  = new(){ "Fall", "Summer", "Spring", "Winter" };

        public int CurrentYear { get; set; } = 2022;
    }
}
