using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReparApp.Models;

public class Technician
{
    [Key]
    [ForeignKey("Person")]
    public int PersonId { get; set; }

    public Person Person { get; set; } = null!;

    [Required(ErrorMessage = "El porcentaje de comisión es requerido.")]
    [Range(0, 100, ErrorMessage = "El porcentaje debe estar entre 0 y 100.")]
    public decimal CommissionPercentage { get; set; }

    public ICollection<Repair> Repairs { get; set; } = new List<Repair>();
}