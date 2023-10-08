using FitWorld.BLL.Models;

namespace FitWorld.BLL.Interfaces
{
    public interface IStudentService
    {
        //void RegisterStudent(StudentModel model);
        StudentModel? RegisterStudent(StudentModel model);
        StudentModel? LogInStudent(string email, string password);
    }
}
