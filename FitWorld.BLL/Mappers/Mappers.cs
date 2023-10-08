using FitWorld.BLL.Models;
using FitWorld.Dal.Data;

namespace FitWorld.BLL.Mappers
{
    public static class Mappers
    {
        internal static StudentData BllToDalStudent(this StudentModel model)
        {
            return new StudentData()
            {
                StudentId = model.StudentId,
                Email = model.Email,
                Password = model.Password,
            };
        }
        public static StudentModel? DalToBllStudent(this StudentData data)
        {
            return data is null
                ? null
                : new StudentModel()
                {
                    StudentId = data.StudentId,
                    Email = data.Email,
                };
        }
        internal static EquipmentData BllToDalEquipment(this EquipmentModel model)
        {
            return new EquipmentData()
            {
                EquipmentId = model.EquipmentId,
                Name = model.Name,
                Price = model.Price,
            };
        }
        public static EquipmentModel? DalToBllEquipment(this EquipmentData data)
        {
            return data is null
                ? null
                : new EquipmentModel()
                {
                    EquipmentId = data.EquipmentId,
                    Name = data.Name,
                    Price = data.Price,
                };
        }
        internal static MartialArtData BllToDalMartialArt(this MartialArtModel model)
        {
            return new MartialArtData()
            {
                MartialArtId = model.MartialArtId,
                Name = model.Name,
            };
        }
        public static MartialArtModel? DalToBllMartialArt(this MartialArtData data)
        {
            return data is null
                ? null
                : new MartialArtModel()
                {
                    MartialArtId = data.MartialArtId,
                    Name = data.Name,
                };
        }
        internal static InstructorData BllToDalInstructor(this InstructorModel model)
        {
            return new InstructorData()
            {
                InstructorId = model.InstructorId,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };
        }
        public static InstructorModel? DalToBllInstructor(this InstructorData data)
        {
            return data is null
                ? null
                : new InstructorModel()
                {
                    InstructorId = data.InstructorId,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                };
        }
        internal static ClassData BllToDalClass(this ClassModel model)
        {
            return new ClassData()
            {
                ClassId = model.ClassId,
                MartialArtId = model.MartialArtId,
                InstructorId = model.InstructorId,
                DateTime = model.DateTime,
                PricePerHour = model.PricePerHour,
            };
        }
        public static ClassModel? DalToBllClass(this ClassData data)
        {
            return data is null
                ? null
                : new ClassModel()
                {
                    ClassId = data.ClassId,
                    MartialArtId = data.MartialArtId,
                    InstructorId = data.InstructorId,
                    DateTime = data.DateTime,
                    PricePerHour = data.PricePerHour,
                };
        }
    }
}
