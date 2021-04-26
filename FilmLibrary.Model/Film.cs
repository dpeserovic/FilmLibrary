using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmLibrary.Model
{
    public class Film
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Director { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [ForeignKey(nameof(Rating))]
        public int? RatingID { get; set; }
        public Rating Rating { get; set; }
        [ForeignKey(nameof(Genre))]
        public int? GenreID { get; set; }
        public Genre Genre { get; set; }

    }
}
