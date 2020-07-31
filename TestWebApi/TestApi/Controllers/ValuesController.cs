using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly IEnumerable<string> values;
        public ValuesController(IConfiguration config)
        {
             values = config.GetSection("Values").Get<IEnumerable<string>>();
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return values;
        }
    }
}
