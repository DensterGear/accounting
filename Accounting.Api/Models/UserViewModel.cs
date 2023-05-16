using System.ComponentModel.DataAnnotations;

namespace Accounting.Api.Models;

public enum Gender
{
    None = 0,
    Male = 1,
    Female = 2
}

public record UserViewModel
{
    public string? Id { get; init; }
    
    [Required(ErrorMessage = "The Name field is required.")]
    [StringLength(50, ErrorMessage = "Name is too long")]
    public string? Name { get; init; }
    public string? LastName { get; init; }
    
    [Required(ErrorMessage = "E-mail is required")]
    [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid e-mail address")]
    [StringLength(100, ErrorMessage = "E-mail is too long")]
    public string? Email { get; init; }
    
    [Range(0, 150, ErrorMessage = "The Age field must be between 0 and 150.")]
    public int Age { get; init; }
    
    [Required]
    public Gender Gender { get; init; }
    
    [Required]
    public string? City { get; init; }
}