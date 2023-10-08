using System.ComponentModel.DataAnnotations;

namespace FitWorldApi.Models.Forms
{
    public class ClassCreationForm
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int MartialArtId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int InstructorId { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Range(0.01, 99999999998.99)]
        public decimal PricePerHour { get; set; }
    }
}
