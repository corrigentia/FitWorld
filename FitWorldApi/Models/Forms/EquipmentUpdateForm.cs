using System.ComponentModel.DataAnnotations;

namespace FitWorldApi.Models.Forms
{
    public class EquipmentUpdateForm
    {
        [Required]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 42, MinimumLength = 1)]
        public string? Name { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Range(0.01, 99999999998.99)]
        public decimal Price { get; set; }
    }
}
