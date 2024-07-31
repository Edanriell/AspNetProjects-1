using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AdvancedEfCore.Models;

// Note that the data annotation attributes are overridden by Fluent API configuration in the DbContext.
[Table("Invoices")]
public class Invoice
{
    [Column("Id")] [Key] public Guid Id { get; set; }

    [Column("InvoiceNumber", TypeName = "varchar(32)")]
    [Required]
    public string InvoiceNumber { get; set; } = string.Empty;

    [Column("ContactName")]
    [Required]
    [MaxLength(32)]
    public string ContactName { get; set; } = string.Empty;

    [Column("Description")] public string? Description { get; set; }

    [Column("Amount")]
    [Required]
    [Precision(18, 2)]
    [Range(0, 9999999999999999.99)]
    public decimal Amount { get; set; }

    [Column("InvoiceDate", TypeName = "datetimeoffset")]
    [Required]
    public DateTimeOffset InvoiceDate { get; set; }

    [Column("DueDate", TypeName = "datetimeoffset")]
    [Required]
    public DateTimeOffset DueDate { get; set; }

    [Column("Status", TypeName = "varchar(16)")]
    public InvoiceStatus Status { get; set; }
}