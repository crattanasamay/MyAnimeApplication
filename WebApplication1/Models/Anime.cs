using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Anime
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Season { get; set; }

        public string Summary { get; set; }

        

    }
}
