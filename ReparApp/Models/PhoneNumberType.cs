using System.ComponentModel.DataAnnotations;

namespace ReparApp.Models;

public class PhoneNumberType
{
    [Key]
    public int PhoneNumberTypeId { get; set; }

    [Required(ErrorMessage = "El nombre es requerido.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
    public string Name { get; set; } = string.Empty;

    public ICollection<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
}
