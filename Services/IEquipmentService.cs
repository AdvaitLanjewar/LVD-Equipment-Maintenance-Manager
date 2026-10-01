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
