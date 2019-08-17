using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace webapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParametersController : ControllerBase
    {
        // GET api/values
        [HttpGet("{component}")]
        public ActionResult<IEnumerable<string>> Get(string component)
        {
            return new string[] { "value1", "value2" };
        }

        // PUT api/values/5
        [HttpPut("{component}")]
        public void Put(int component, [FromBody] string value)
        {

        }
    }
}
