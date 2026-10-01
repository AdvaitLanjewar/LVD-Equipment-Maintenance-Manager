using LVDMaintenanceManager.DTOs;
using LVDMaintenanceManager.Models;
using LVDMaintenanceManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace LVDMaintenanceManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaintenanceController : ControllerBase
{
    private readonly IEquipmentService _service;

    public MaintenanceController(IEquipmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<MaintenanceRequest>> GetAll()
    {
        try
        {
            var requests = _service.GetAllMaintenanceRequests();
            return Ok(requests);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred fetching maintenance requests.", detail = ex.Message });
        }
    }

    [HttpGet("high-priority")]
    public ActionResult<IEnumerable<MaintenanceRequest>> GetHighPriority()
    {
        try
        {
            var requests = _service.GetHighPriorityMaintenanceRequests();
            return Ok(requests);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred fetching high priority requests.", detail = ex.Message });
        }
    }

    [HttpPost]
    public ActionResult<MaintenanceRequest> Create([FromBody] CreateMaintenanceDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var created = _service.CreateMaintenanceRequest(dto);
            return CreatedAtAction(nameof(GetAll), null, created);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred creating maintenance request.", detail = ex.Message });
        }
    }
}
