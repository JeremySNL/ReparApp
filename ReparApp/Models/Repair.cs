using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReparApp.Models;

public class Repair
{
    [Key]
    public int RepairId { get; set; }

    [Required(ErrorMessage = "El técnico es requerido.")]
    public int TechnicianId { get; set; }

    [ForeignKey("TechnicianId")]
    public Technician Technician { get; set; } = null!;

    [Required(ErrorMessage = "El cliente es requerido.")]
    public int CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; } = null!;

    [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "El estado es requerido.")]
    public int RepairStatusId { get; set; }

    [ForeignKey("RepairStatusId")]
    public RepairStatus RepairStatus { get; set; } = null!;

    [Required(ErrorMessage = "La fecha de recepción es requerida.")]
    public DateTime ReceivedDate { get; set; }

    public DateTime? CompletedDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public DateTime? DueDate { get; set; }

    [StringLength(50, ErrorMessage = "La marca no puede exceder los 50 caracteres.")]
    public string? DeviceBrand { get; set; }

    [StringLength(50, ErrorMessage = "El modelo no puede exceder los 50 caracteres.")]
    public string? DeviceModel { get; set; }

    [StringLength(100, ErrorMessage = "El número de serie no puede exceder los 100 caracteres.")]
    public string? SerialNumber { get; set; }

    [Range(0, 999999.99, ErrorMessage = "El precio debe ser mayor o igual a 0.")]
    public decimal Price { get; set; }

    [Range(0, 100, ErrorMessage = "El porcentaje debe estar entre 0 y 100.")]
    public decimal TechnicianCommissionPercentage { get; set; }

    [StringLength(500, ErrorMessage = "Las notas no pueden exceder los 500 caracteres.")]
    public string? Notes { get; set; }

    public ICollection<RepairStatusHistory> StatusHistory { get; set; } = new List<RepairStatusHistory>();
}