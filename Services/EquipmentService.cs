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
