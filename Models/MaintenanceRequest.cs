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
