using System.ComponentModel.DataAnnotations;

namespace FitWorldApi.Models.Forms
{
    public class StudentEmailSearchForm
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [StringLength(maximumLength: 320, MinimumLength = 5)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,}$")]
        public string? Email { get; set; }
    }
}
