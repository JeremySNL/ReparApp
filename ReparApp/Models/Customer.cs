using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReparApp.Models;

public class Customer
{
    [Key]
    [ForeignKey("Person")]
    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;

    [StringLength(20, ErrorMessage = "El documento no puede exceder los 20 caracteres.")]
    public string? DocumentId { get; set; }

    [StringLength(150, ErrorMessage = "La dirección no puede exceder los 150 caracteres.")]
    public string? Address { get; set; }

    [StringLength(500, ErrorMessage = "Las notas no pueden exceder los 500 caracteres.")]
    public string? Notes { get; set; }

    public ICollection<Repair> Repairs { get; set; } = new List<Repair>();
}