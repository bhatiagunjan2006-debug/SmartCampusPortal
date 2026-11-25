using Microsoft.AspNetCore.Mvc;
using SmartCampusPortal.Data;
using SmartCampusPortal.Helpers;
using SmartCampusPortal.Models;

namespace SmartCampusPortal.Controllers
{
    /// <summary>REST API - uses the SAME ServiceRequestDAL / database as the MVC pages.</summary>
    [ApiController]
    [Route("api/[controller]")]   // => api/ServiceApi
    public class ServiceApiController : ControllerBase
    {
        private readonly ServiceRequestDAL _dal;
        private readonly ILogger<ServiceApiController> _logger;

        public ServiceApiController(ServiceRequestDAL dal, ILogger<ServiceApiController> logger)
        {
            _dal = dal;
            _logger = logger;
        }

        private ObjectResult DbError(Exception ex)
        {
            _logger.LogError(ex, "API database error");
            return StatusCode(StatusCodes.Status503ServiceUnavailable,
                new ProblemDetails { Title = "Database unavailable", Status = 503,
                                     Detail = "Could not access the database." });
        }

        /// <summary>Retrieve all service requests.</summary>
        // GET api/ServiceApi
        [HttpGet]
        [ProducesResponseType(typeof(List<ServiceRequest>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            try { return Ok(_dal.GetAll()); }
            catch (Exception ex) { return DbError(ex); }
        }

        /// <summary>Retrieve one service request by id.</summary>
        // GET api/ServiceApi/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ServiceRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            try
            {
                var item = _dal.GetById(id);
                return item == null ? NotFound(new { message = $"Request {id} not found." }) : Ok(item);
            }
            catch (Exception ex) { return DbError(ex); }
        }

        /// <summary>Create a new service request (Status = Pending, RequestDate = now).</summary>
        // POST api/ServiceApi
        [HttpPost]
        [ProducesResponseType(typeof(ServiceRequest), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] ServiceRequest request)
        {
            // [ApiController] already returns 400 automatically when Required rules fail.
            try
            {
                int id = _dal.Insert(request);
                var created = _dal.GetById(id);
                return CreatedAtAction(nameof(GetById), new { id }, created);
            }
            catch (Exception ex) { return DbError(ex); }
        }

        /// <summary>Update the status of an existing request (Pending / In Progress / Completed).</summary>
        // PUT api/ServiceApi/UpdateStatus/5
        [HttpPut("UpdateStatus/{id:int}")]
        [ProducesResponseType(typeof(ServiceRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateStatus(int id, [FromBody] UpdateStatusDto dto)
        {
            if (!Lookups.Statuses.Contains(dto.Status))
            {
                ModelState.AddModelError(nameof(dto.Status), "Status must be Pending, In Progress or Completed.");
                return ValidationProblem(ModelState);
            }

            try
            {
                if (!_dal.UpdateStatus(id, dto.Status))
                    return NotFound(new { message = $"Request {id} not found." });
                return Ok(_dal.GetById(id));
            }
            catch (Exception ex) { return DbError(ex); }
        }
    }
}
