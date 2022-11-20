using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AsyncInn.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenitiesController: ControllerBase
    {
        // GET: api/<AmenityController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AmenityController>/5
        [HttpGet("{id}")]
        public string Get( int id )
        {
            return "value";
        }

        // POST api/<AmenityController>
        [HttpPost]
        public void Post( [FromBody] string value )
        {
        }

        // PUT api/<AmenityController>/5
        [HttpPut("{id}")]
        public void Put( int id, [FromBody] string value )
        {
        }

        // DELETE api/<AmenityController>/5
        [HttpDelete("{id}")]
        public void Delete( int id )
        {
        }
    }
}
