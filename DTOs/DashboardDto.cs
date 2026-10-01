namespace LVDMaintenanceManager.DTOs;

public class DashboardDto
{
    public int TotalEquipment { get; set; }
    public int ActiveEquipment { get; set; }
    public int TotalMaintenanceRequests { get; set; }
    public int PendingMaintenance { get; set; }
    public int HighPriorityRequests { get; set; }
}
