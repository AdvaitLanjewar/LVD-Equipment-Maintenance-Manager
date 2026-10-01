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
