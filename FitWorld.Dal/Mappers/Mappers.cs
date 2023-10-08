using FitWorld.Dal.Data;

using System.Data;

namespace FitWorld.Dal.Mappers
{
    public static class Mappers
    {
        public static StudentData DbToStudent(this IDataRecord record)
        {
            return new StudentData()
            {
                StudentId = (int)record["StudentId"],
                Email = (string)record["Email"],
            };
        }
        public static ClassData DbToClass(this IDataRecord record)
        {
            return new ClassData()
            {
                ClassId = (int)record["ClassId"],
                MartialArtId = (int)record["MartialArtId"],
                InstructorId = (int)record["InstructorId"],
                DateTime = (DateTime)record["DateTime"],
                PricePerHour = (decimal)record["PricePerHour"],
            };
        }
        public static EquipmentData DbToEquipment(this IDataRecord record)
        {
            return new EquipmentData()
            {
                EquipmentId = (int)record["EquipmentId"],
                Name = (string)record["Name"],
                Price = (decimal)record["Price"],
            };
        }
        public static InstructorData DbToInstructor(this IDataRecord record)
        {
            return new InstructorData()
            {
                InstructorId = (int)record["InstructorId"],
                FirstName = (string)record["FirstName"],
                LastName = record["LastName"] is DBNull ? null : (string)record["LastName"],
            };
        }
        public static MartialArtData DbToMartialArt(this IDataRecord record)
        {
            return new MartialArtData()
            {
                MartialArtId = (int)record["MartialArtId"],
                Name = (string)record["Name"],
            };
        }
    }
}
