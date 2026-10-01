using LVDMaintenanceManager.DTOs;
using LVDMaintenanceManager.Models;
using LVDMaintenanceManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace LVDMaintenanceManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquipmentController : ControllerBase
{
    private readonly IEquipmentService _service;

    public EquipmentController(IEquipmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Equipment>> GetAll()
    {
        try
        {
            var equipment = _service.GetAllEquipment();
            return Ok(equipment);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred fetching equipment.", detail = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public ActionResult<Equipment> GetById(int id)
    {
        try
        {
            var item = _service.GetEquipmentById(id);
            if (item == null)
            {
                return NotFound(new { message = $"Equipment with ID {id} not found." });
            }
            return Ok(item);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred fetching equipment.", detail = ex.Message });
        }
    }

    [HttpPost]
    public ActionResult<Equipment> Create([FromBody] CreateEquipmentDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var created = _service.CreateEquipment(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred creating equipment.", detail = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] CreateEquipmentDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var success = _service.UpdateEquipment(id, dto);
            if (!success)
            {
                return NotFound(new { message = $"Equipment with ID {id} not found for update." });
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred updating equipment.", detail = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var success = _service.DeleteEquipment(id);
            if (!success)
            {
                return NotFound(new { message = $"Equipment with ID {id} not found for deletion." });
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred deleting equipment.", detail = ex.Message });
        }
    }
}
