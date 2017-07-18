using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class BookForUpdateDto : BookForManipulationDto
    {
        [Required(ErrorMessage = "You should fill out a description")]
        public override string Description {
            get => base.Description;
            set => base.Description = value;
        }
    }
}