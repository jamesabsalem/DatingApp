using DatingApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IValuesRepository _iValuesRepository;

        public ValuesController(IValuesRepository iValuesRepository)
        {
            _iValuesRepository = iValuesRepository;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult> GetValues()
        {
            var values = await _iValuesRepository.GetValues();
            return Ok(values);
        }

        // GET api/values/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetValue(int id)
        {
            var value = await _iValuesRepository.GetValueById(id);
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}