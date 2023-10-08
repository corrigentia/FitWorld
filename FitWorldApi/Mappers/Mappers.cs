using FitWorld.BLL.Models;

using FitWorldApi.Models;
using FitWorldApi.Models.Forms;

namespace FitWorldApi.Mappers
{
    public static class Mappers
    {
        internal static StudentModel ApiToBllStudent(this StudentRegisterForm form)
        {
            return new StudentModel()
            {
                Email = form.Email,
                Password = form.Password,
            };
        }
        public static StudentViewModel BllToApiStudent(this StudentModel model)
        {
            return new StudentViewModel()
            {
                StudentId = model.StudentId,
                Email = model.Email,
            };
        }
        internal static EquipmentModel ApiToBllEquipment(this EquipmentCreationForm form)
        {
            return new EquipmentModel()
            {
                Name = form.Name,
                Price = form.Price,
            };
        }
        public static EquipmentViewModel BllToApiEquipment(this EquipmentModel model)
        {
            return new EquipmentViewModel()
            {
                EquipmentId = model.EquipmentId,
                Name = model.Name,
                Price = model.Price,
            };
        }
        internal static MartialArtModel ApiToBllMartialArt(this MartialArtCreationForm form)
        {
            return new MartialArtModel()
            {
                Name = form.Name,
            };
        }
        public static MartialArtViewModel BllToApiMartialArt(this MartialArtModel model)
        {
            return new MartialArtViewModel()
            {
                MartialArtId = model.MartialArtId,
                Name = model.Name,
            };
        }
        internal static InstructorModel ApiToBllInstructor(this InstructorCreationForm form)
        {
            return new InstructorModel()
            {
                FirstName = form.FirstName,
                LastName = form.LastName,
            };
        }
        public static InstructorViewModel BllToApiInstructor(this InstructorModel model)
        {
            return new InstructorViewModel()
            {
                InstructorId = model.InstructorId,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };
        }
        internal static ClassModel ApiToBllClass(this ClassCreationForm form)
        {
            return new ClassModel()
            {
                MartialArtId = form.MartialArtId,
                InstructorId = form.InstructorId,
                DateTime = form.DateTime,
                PricePerHour = form.PricePerHour,
            };
        }
        public static ClassViewModel BllToApiClass(this ClassModel model)
        {
            return new ClassViewModel()
            {
                ClassId = model.ClassId,
                MartialArtId = model.MartialArtId,
                InstructorId = model.InstructorId,
                DateTime = model.DateTime,
                PricePerHour = model.PricePerHour,
            };
        }
    }
}
