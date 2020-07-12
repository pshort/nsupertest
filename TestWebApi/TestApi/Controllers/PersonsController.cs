
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using TestApi.Models;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonsController : ControllerBase
    {

        [HttpPost]
        public IActionResult CreatePerson([FromBody]CreatePersonRequest request)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState.ValidationState);
            }

            return Ok(new CreatePersonResponse 
            {
                Id = 10,
                Name = request.Name,
                Age = request.Age
            });
        }
    }
}