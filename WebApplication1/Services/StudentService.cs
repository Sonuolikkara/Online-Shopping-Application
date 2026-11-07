using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class StudentService : IStudentService
    {
        private List<Student> students=new List<Student>()
        {
            new Student {
                Id=1, 
                Name="Sonu",
                Age=20 },
            new Student { 
                Id=2,
                Name="Silla",
                Age=22
            },
            new Student { 
                Id=3,
                Name="Siya",
                Age=29
            }
        };

        public List<Student> GetStudents()
        {
            return students;
        }
        public Student GetStudentById(int id)
        {
            return students.FirstOrDefault(x => x.Id == id);
        }
        public Student AddStudent(Student student)
        {
            students.Add(student);
            return student;
        }
    }
}