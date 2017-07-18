using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public abstract class BookForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out a title")]
        [MaxLength(100, ErrorMessage = "The title should be less than 100 characters")]
        public string Title { get; set; }
        
        [MaxLength(500, ErrorMessage = "The description should be less than 500 characters yo!")]
        public virtual string Description { get; set; }
    }
}