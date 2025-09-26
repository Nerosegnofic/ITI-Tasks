using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinFormsApp1.Models;

[Table("Customer")]
public class Customer
{
    [Key]
    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [MaxLength(50)]
    public string? FirstName { get; set; }

    [MaxLength(50)]
    public string? LastName { get; set; }

    [MaxLength(50)]
    public string? City { get; set; }

    [MaxLength(250)]
    public string? Address { get; set; }
}