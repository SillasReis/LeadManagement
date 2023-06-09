using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeadManagement.Models;


public enum LeadStatus
{
    INVITED,
    ACCEPTED,
    DECLINED
}

public class Lead
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(50)]
    public string ContactFullName { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(20)]
    public string ContactPhoneNumber { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(254)]
    public string ContactEmail { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(50)]
    public string Category { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(254)]
    public string Description { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(100)]
    public string Suburb { get; set; }

    [Required]
    public LeadStatus Status { get; set; } = LeadStatus.INVITED;

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Discount { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
