using Microsoft.AspNetCore.Mvc;
using OrderSystem.SDK.Contract;
using OrderModels.School;

namespace OrderSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ISchoolService _schoolService;
        public StudentController(ILogger<OrderSystemController> logger, ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents(CancellationToken cancellationToken)
        {
            var response = await _schoolService.GetAll(cancellationToken);
            if (response.Any()) return NotFound();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] Student request, CancellationToken cancellationToken)
        {
            var response = await _schoolService.CreateStudent(request, cancellationToken);
            return Ok(response);
        }

        [HttpPut("/{studentId}")]
        public async Task<IActionResult> EditStudent(int studentId, [FromBody] Student request, CancellationToken cancellationToken)
        {
            var response = await _schoolService.EditStudent(request, cancellationToken);
            if(response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteStudent(int studentId, CancellationToken cancellationToken)
        {
            var response = await _schoolService.DeleteStudent(studentId, cancellationToken);
            return Ok(response);
        }
    }
}
