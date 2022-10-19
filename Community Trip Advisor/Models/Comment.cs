using System.ComponentModel.DataAnnotations;

namespace Community_Trip_Advisor.Models
{
    public class Comment
    {
   
        public int CommentID { get; set; }
        [Required]

        public int PlaceID { get; set; }
        public string CommmentDescription { get; set; }
        [Required]
        public int Rating { get; set; }

        public DateTime CommentedOn { get; set; }

        public string AddedBy { get; set; }

    }
}
