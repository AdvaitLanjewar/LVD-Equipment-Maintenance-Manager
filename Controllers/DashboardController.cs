using LVDMaintenanceManager.DTOs;
using LVDMaintenanceManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace LVDMaintenanceManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IEquipmentService _service;

    public DashboardController(IEquipmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<DashboardDto> GetDashboard()
    {
        try
        {
            var stats = _service.GetDashboardStats();
            return Ok(stats);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred fetching dashboard statistics.", detail = ex.Message });
        }
    }
}
