namespace WebApplication1.Models
{
    public class SeasonListModel
    {
        public List<string> SeasonList { get; set; } = new List<string>{ "Winter", "Fall", "Summer", "Spring" };
        public int CurrentYear { get; set; } = 2022;
    }
}
