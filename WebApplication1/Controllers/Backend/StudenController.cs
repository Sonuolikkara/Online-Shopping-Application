using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Properties.Controllers.Backend
{
    [Route("api/[controller]")]
    [ApiController]
   [Authorize(Roles = "Admin,Superadmin")]
    public class StudentController : ControllerBase

    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok(_studentService.GetStudents());
        }
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = _studentService.GetStudentById(id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }
        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            var newStudent = _studentService.AddStudent(student);

            return Ok(newStudent);
        }
    }
}
