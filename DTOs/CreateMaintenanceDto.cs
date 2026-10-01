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
