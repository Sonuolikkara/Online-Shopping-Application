using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IStudentService
    {
        List<Student> GetStudents();
        Student GetStudentById(int id);

        Student AddStudent(Student student);
    }
}