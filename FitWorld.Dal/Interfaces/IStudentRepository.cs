using FitWorld.Dal.Data;

namespace FitWorld.Dal.Interfaces
{
    public interface IStudentRepository
    {
        //void RegisterStudent(StudentData data);
        StudentData? RegisterStudent(StudentData data);

        //StudentData LogInStudent(string email, string password);
        StudentData? LogInStudent(string email, string password);
    }
}
