using System.ComponentModel.DataAnnotations;

namespace ReparApp.Models;

public class RepairStatus
{
    [Key]
    public int RepairStatusId { get; set; }

    [Required(ErrorMessage = "El nombre del estado es requerido.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
    public string Name { get; set; } = string.Empty;

    public ICollection<Repair> Repairs { get; set; } = new List<Repair>();
}