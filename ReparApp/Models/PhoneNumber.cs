using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReparApp.Models;

public class PhoneNumber
{
    [Key]
    public int PhoneNumberId { get; set; }

    [Required(ErrorMessage = "El id de la persona es requerida.")]
    public int PersonId { get; set; }

    [Required(ErrorMessage = "El id del tipo de numero de telefono es requerida.")]
    public int PhoneNumberTypeId { get; set; }
    [ForeignKey("PhoneNumberTypeId")]
    public PhoneNumberType? PhoneNumberType { get; set; }

    [Required(ErrorMessage = "El numero de telefono es requerido.")]
    public string Number { get; set; } = string.Empty;
}
