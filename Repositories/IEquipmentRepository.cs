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
