namespace WebApplication1.Models.ChartModels.TopOneHundredChart
{
    public class TopOneHundredChartModel
    {
        public int AnimeId { get; set; }
        public string Title { get; set; }
        public double Rating { get; set; }
        public int Year { get; set; }

        public int Rank { get; set; }
        public string genreString { get; set; }

    }
}
