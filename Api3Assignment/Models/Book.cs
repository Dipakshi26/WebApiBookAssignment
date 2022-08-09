using System.ComponentModel.DataAnnotations;

namespace Api3Assignment.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Zoner { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        public int Cost { get; set; }
      
        public string? ImageUrl { get; set; }

    }
}
