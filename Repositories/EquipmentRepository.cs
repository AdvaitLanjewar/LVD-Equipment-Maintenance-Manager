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
