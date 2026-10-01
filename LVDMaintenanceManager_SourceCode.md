# LVD Equipment Maintenance Manager - Complete Source Code

This document contains the complete, unedited source code for the **LVD Equipment Maintenance Manager** ASP.NET Core Web API application.

---

## 1. Program.cs

```csharp
using LVDMaintenanceManager.Repositories;
using LVDMaintenanceManager.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection Setup
builder.Services.AddSingleton<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();

var app = builder.Build();

// Configure HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
```

---

## 2. Models/Equipment.cs

```csharp
namespace LVDMaintenanceManager.Models;

public class Equipment
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
```

---

## 3. Models/MaintenanceRequest.cs

```csharp
namespace LVDMaintenanceManager.Models;

public class MaintenanceRequest
{
    public int Id { get; set; }
    public int EquipmentId { get; set; }
    public string Problem { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}
```

---

## 4. DTOs/CreateEquipmentDto.cs

```csharp
using System.ComponentModel.DataAnnotations;

namespace LVDMaintenanceManager.DTOs;

public class CreateEquipmentDto
{
    [Required(ErrorMessage = "Equipment name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Serial number is required.")]
    public string SerialNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Location is required.")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Status is required.")]
    public string Status { get; set; } = "Active";
}
```

---

## 5. DTOs/CreateMaintenanceDto.cs

```csharp
using System.ComponentModel.DataAnnotations;

namespace LVDMaintenanceManager.DTOs;

public class CreateMaintenanceDto
{
    [Range(1, int.MaxValue, ErrorMessage = "A valid EquipmentId is required.")]
    public int EquipmentId { get; set; }

    [Required(ErrorMessage = "Problem description is required.")]
    public string Problem { get; set; } = string.Empty;

    [Required(ErrorMessage = "Priority is required.")]
    public string Priority { get; set; } = "Medium";
}
```

---

## 6. DTOs/DashboardDto.cs

```csharp
namespace LVDMaintenanceManager.DTOs;

public class DashboardDto
{
    public int TotalEquipment { get; set; }
    public int ActiveEquipment { get; set; }
    public int TotalMaintenanceRequests { get; set; }
    public int PendingMaintenance { get; set; }
    public int HighPriorityRequests { get; set; }
}
```

---

## 7. Repositories/IEquipmentRepository.cs

```csharp
using LVDMaintenanceManager.Models;

namespace LVDMaintenanceManager.Repositories;

public interface IEquipmentRepository
{
    // Equipment Data Operations
    IEnumerable<Equipment> GetAllEquipment();
    Equipment? GetEquipmentById(int id);
    Equipment AddEquipment(Equipment equipment);
    bool UpdateEquipment(Equipment equipment);
    bool DeleteEquipment(int id);

    // Maintenance Request Data Operations
    IEnumerable<MaintenanceRequest> GetAllMaintenanceRequests();
    MaintenanceRequest AddMaintenanceRequest(MaintenanceRequest request);
}
```

---

## 8. Repositories/EquipmentRepository.cs

```csharp
using LVDMaintenanceManager.Models;

namespace LVDMaintenanceManager.Repositories;

public class EquipmentRepository : IEquipmentRepository
{
    private readonly List<Equipment> _equipmentList = new();
    private readonly List<MaintenanceRequest> _maintenanceList = new();
    private int _nextEquipmentId = 1;
    private int _nextMaintenanceId = 1;
    private readonly object _lock = new();

    public EquipmentRepository()
    {
        SeedInitialData();
    }

    private void SeedInitialData()
    {
        // 3 sample equipment records
        AddEquipment(new Equipment
        {
            Name = "LVD Strippit VX-1225 Punch Press",
            SerialNumber = "SN-PUNCH-001",
            Location = "Factory Floor - Zone A",
            Status = "Active"
        });

        AddEquipment(new Equipment
        {
            Name = "LVD ToolCell 135/30 Press Brake",
            SerialNumber = "SN-BRAKE-002",
            Location = "Factory Floor - Zone B",
            Status = "Active"
        });

        AddEquipment(new Equipment
        {
            Name = "LVD Phoenix FL-3015 Fiber Laser",
            SerialNumber = "SN-LASER-003",
            Location = "Cutting Bay 1",
            Status = "Under Maintenance"
        });

        // 2 maintenance requests
        AddMaintenanceRequest(new MaintenanceRequest
        {
            EquipmentId = 3,
            Problem = "Laser resonator cooling temperature alarm",
            Priority = "High",
            Status = "Pending",
            CreatedDate = DateTime.UtcNow.AddDays(-2)
        });

        AddMaintenanceRequest(new MaintenanceRequest
        {
            EquipmentId = 2,
            Problem = "Hydraulic oil filter replacement due",
            Priority = "Medium",
            Status = "Pending",
            CreatedDate = DateTime.UtcNow.AddDays(-1)
        });
    }

    public IEnumerable<Equipment> GetAllEquipment()
    {
        lock (_lock)
        {
            return _equipmentList.ToList();
        }
    }

    public Equipment? GetEquipmentById(int id)
    {
        lock (_lock)
        {
            return _equipmentList.FirstOrDefault(e => e.Id == id);
        }
    }

    public Equipment AddEquipment(Equipment equipment)
    {
        lock (_lock)
        {
            equipment.Id = _nextEquipmentId++;
            _equipmentList.Add(equipment);
            return equipment;
        }
    }

    public bool UpdateEquipment(Equipment equipment)
    {
        lock (_lock)
        {
            var existing = _equipmentList.FirstOrDefault(e => e.Id == equipment.Id);
            if (existing == null) return false;

            existing.Name = equipment.Name;
            existing.SerialNumber = equipment.SerialNumber;
            existing.Location = equipment.Location;
            existing.Status = equipment.Status;
            return true;
        }
    }

    public bool DeleteEquipment(int id)
    {
        lock (_lock)
        {
            var existing = _equipmentList.FirstOrDefault(e => e.Id == id);
            if (existing == null) return false;

            _equipmentList.Remove(existing);
            return true;
        }
    }

    public IEnumerable<MaintenanceRequest> GetAllMaintenanceRequests()
    {
        lock (_lock)
        {
            return _maintenanceList.ToList();
        }
    }

    public MaintenanceRequest AddMaintenanceRequest(MaintenanceRequest request)
    {
        lock (_lock)
        {
            request.Id = _nextMaintenanceId++;
            _maintenanceList.Add(request);
            return request;
        }
    }
}
```

---

## 9. Services/IEquipmentService.cs

```csharp
using LVDMaintenanceManager.DTOs;
using LVDMaintenanceManager.Models;

namespace LVDMaintenanceManager.Services;

public interface IEquipmentService
{
    // Equipment Logic
    IEnumerable<Equipment> GetAllEquipment();
    Equipment? GetEquipmentById(int id);
    Equipment CreateEquipment(CreateEquipmentDto dto);
    bool UpdateEquipment(int id, CreateEquipmentDto dto);
    bool DeleteEquipment(int id);

    // Maintenance Logic
    IEnumerable<MaintenanceRequest> GetAllMaintenanceRequests();
    IEnumerable<MaintenanceRequest> GetHighPriorityMaintenanceRequests();
    MaintenanceRequest CreateMaintenanceRequest(CreateMaintenanceDto dto);

    // Dashboard Statistics Logic
    DashboardDto GetDashboardStats();
}
```

---

## 10. Services/EquipmentService.cs

```csharp
using LVDMaintenanceManager.DTOs;
using LVDMaintenanceManager.Models;
using LVDMaintenanceManager.Repositories;

namespace LVDMaintenanceManager.Services;

public class EquipmentService : IEquipmentService
{
    private readonly IEquipmentRepository _repository;

    public EquipmentService(IEquipmentRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Equipment> GetAllEquipment()
    {
        return _repository.GetAllEquipment();
    }

    public Equipment? GetEquipmentById(int id)
    {
        return _repository.GetEquipmentById(id);
    }

    public Equipment CreateEquipment(CreateEquipmentDto dto)
    {
        var equipment = new Equipment
        {
            Name = dto.Name,
            SerialNumber = dto.SerialNumber,
            Location = dto.Location,
            Status = dto.Status
        };
        return _repository.AddEquipment(equipment);
    }

    public bool UpdateEquipment(int id, CreateEquipmentDto dto)
    {
        var existing = _repository.GetEquipmentById(id);
        if (existing == null) return false;

        existing.Name = dto.Name;
        existing.SerialNumber = dto.SerialNumber;
        existing.Location = dto.Location;
        existing.Status = dto.Status;

        return _repository.UpdateEquipment(existing);
    }

    public bool DeleteEquipment(int id)
    {
        return _repository.DeleteEquipment(id);
    }

    public IEnumerable<MaintenanceRequest> GetAllMaintenanceRequests()
    {
        return _repository.GetAllMaintenanceRequests();
    }

    public IEnumerable<MaintenanceRequest> GetHighPriorityMaintenanceRequests()
    {
        // LINQ filter for high-priority requests
        return _repository.GetAllMaintenanceRequests()
            .Where(m => string.Equals(m.Priority, "High", StringComparison.OrdinalIgnoreCase));
    }

    public MaintenanceRequest CreateMaintenanceRequest(CreateMaintenanceDto dto)
    {
        var equipment = _repository.GetEquipmentById(dto.EquipmentId);
        if (equipment == null)
        {
            throw new KeyNotFoundException($"Equipment with ID {dto.EquipmentId} was not found.");
        }

        var request = new MaintenanceRequest
        {
            EquipmentId = dto.EquipmentId,
            Problem = dto.Problem,
            Priority = dto.Priority,
            Status = "Pending",
            CreatedDate = DateTime.UtcNow
        };

        return _repository.AddMaintenanceRequest(request);
    }

    public DashboardDto GetDashboardStats()
    {
        var allEquipment = _repository.GetAllEquipment();
        var allMaintenance = _repository.GetAllMaintenanceRequests();

        // Use LINQ for calculating dashboard statistics
        return new DashboardDto
        {
            TotalEquipment = allEquipment.Count(),
            ActiveEquipment = allEquipment.Count(e => string.Equals(e.Status, "Active", StringComparison.OrdinalIgnoreCase)),
            TotalMaintenanceRequests = allMaintenance.Count(),
            PendingMaintenance = allMaintenance.Count(m => string.Equals(m.Status, "Pending", StringComparison.OrdinalIgnoreCase)),
            HighPriorityRequests = allMaintenance.Count(m => string.Equals(m.Priority, "High", StringComparison.OrdinalIgnoreCase))
        };
    }
}
```

---

## 11. Controllers/EquipmentController.cs

```csharp
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
```

---

## 12. Controllers/MaintenanceController.cs

```csharp
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
```

---

## 13. Controllers/DashboardController.cs

```csharp
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
```
