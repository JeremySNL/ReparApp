using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReparApp.Models;

public class RepairStatusHistory
{
    [Key]
    public int RepairStatusHistoryId { get; set; }

    [Required(ErrorMessage = "La reparación es requerida.")]
    public int RepairId { get; set; }

    [ForeignKey("RepairId")]
    public Repair Repair { get; set; } = null!;

    [Required(ErrorMessage = "El estado es requerido.")]
    public int RepairStatusId { get; set; }

    [ForeignKey("RepairStatusId")]
    public RepairStatus RepairStatus { get; set; } = null!;

    [Required(ErrorMessage = "La fecha de cambio es requerida.")]
    public DateTime ChangedAt { get; set; }

    [Required(ErrorMessage = "El técnico es requerido.")]
    public int ChangedBy { get; set; }

    [ForeignKey("ChangedBy")]
    public Technician Technician { get; set; } = null!;

    [StringLength(300, ErrorMessage = "Las notas no pueden exceder los 300 caracteres.")]
    public string? Notes { get; set; }
}