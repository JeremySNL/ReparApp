using System.ComponentModel.DataAnnotations;

namespace ReparApp.Models;

public class Person
{
    [Key]
    public int PersonId { get; set; }

    [Required(ErrorMessage = "El nombre es requerido.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "El segundo nombre no puede exceder los 50 caracteres.")]
    public string? MiddleName { get; set; }

    [Required(ErrorMessage = "El apellido es requerido.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres.")]
    public string LastName { get; set; } = string.Empty;

    public ICollection<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
}