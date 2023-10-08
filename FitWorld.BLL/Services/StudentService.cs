using FitWorld.BLL.Interfaces;
using FitWorld.BLL.Mappers;
using FitWorld.BLL.Models;
using FitWorld.Dal.Interfaces;

namespace FitWorld.BLL.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public StudentModel? LogInStudent(string email, string password)
        {
            return _studentRepository.LogInStudent(email, password)?.DalToBllStudent();
        }

        //public void RegisterStudent(StudentModel model)
        public StudentModel? RegisterStudent(StudentModel model)
        {
            //_studentRepository.RegisterStudent(model.BllToDalStudent());
            return _studentRepository.RegisterStudent(model.BllToDalStudent())?.DalToBllStudent();
        }
    }
}
