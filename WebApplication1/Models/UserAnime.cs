using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class UserAnime
    {
        public int Id { get; set; }

        public string? UserName { get; set; }
        public int AnimeId { get; set; }

        public DateTime AnimeDateAdded { get; set; }
        
        //public ICollection<User> Anime { get; set; }



    }
}
