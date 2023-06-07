using System.ComponentModel.DataAnnotations;

namespace LeadManagement.Data.Dtos;

public class UpdateLeadDto
{
    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string ContactFullName { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(20, MinimumLength = 10)]
    public string ContactPhoneNumber { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(254)]
    [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Invalid Email")]
    public string ContactEmail { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string Category { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(254, MinimumLength = 10)]
    public string Description { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(100, MinimumLength = 10)]
    public string Suburb { get; set; }

    [Required]
    [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Price can't have more than 2 decimal places")]
    public decimal Price { get; set; }
}
