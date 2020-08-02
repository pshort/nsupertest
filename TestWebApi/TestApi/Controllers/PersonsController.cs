
using System.Linq;
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
        public IActionResult CreatePerson([FromBody]CreatePersonRequest request, int? setId = null)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState.ValidationState);
            }

            if(Request.Headers.ContainsKey("nameOverride"))
            {
                request.Name = Request.Headers["nameOverride"].First();
            }

            return Ok(new CreatePersonResponse 
            {
                Id = setId != null ? setId.Value : 10,
                Name = request.Name,
                Age = request.Age
            });
        }
    }
}