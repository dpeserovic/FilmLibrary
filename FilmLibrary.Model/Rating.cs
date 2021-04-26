using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmLibrary.Model
{
    public class Rating
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Rate { get; set; }
        public virtual ICollection<Film> Films { get; set; }
    }
}
