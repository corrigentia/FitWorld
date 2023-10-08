namespace FitWorld.BLL.Models
{
    public class ClassModel
    {
        public int ClassId { get; set; }
        public int MartialArtId { get; set; }
        public int InstructorId { get; set; }
        public DateTime DateTime { get; set; }
        public decimal PricePerHour { get; set; }
    }
}
