using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using SampleEfCoreApp.Data;

namespace SampleEfCoreApp.Controllers
{
    [ApiController]
    [Route("students")]
    public class StudentsController : ControllerBase
    {
        private readonly SchoolContext _context;
        
        public StudentsController(SchoolContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public ActionResult Get()
        {
            var students = _context.Students.ToList();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public ActionResult Get([FromRoute] int id)
        {
            var student = _context.Students.FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }
    }
}