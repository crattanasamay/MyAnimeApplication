using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class User 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserName  { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Salt { get; set; }


        /// <summary>
        /// This will define our forein key in UserAnime 1 user can have many
        /// </summary>
        //public UserAnime UserAnime { get; set; }

   

        
    }
}
