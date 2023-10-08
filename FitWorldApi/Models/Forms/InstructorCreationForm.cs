using System.ComponentModel.DataAnnotations;

namespace FitWorldApi.Models.Forms
{
    public class InstructorCreationForm
    {
        [Required]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        public string? FirstName { get; set; }
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        public string? LastName { get; set; }
    }
}
