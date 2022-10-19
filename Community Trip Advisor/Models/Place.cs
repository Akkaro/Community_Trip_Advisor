using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Community_Trip_Advisor.Models
{
    public class Place
    {
        public int ID { get; set; }
        [Required]

        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Type { get; set; }

        public string ImgPath { get; set; }

        public string ImgTitle { get; set; }

        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }

        public List<Comment> Comments {get; set;}

        public float RatingValue { get; set; }


    }
}
