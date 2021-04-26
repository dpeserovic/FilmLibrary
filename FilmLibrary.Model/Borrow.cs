using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmLibrary.Model
{
    public class Borrow
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string userId { get; set; }
        [ForeignKey(nameof(Film))]
        public int? FilmID { get; set; }
        public Film Film { get; set; }
    }
}
