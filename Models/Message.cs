namespace practice.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class Message
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public string? id{get; set;}
    [Required]
    public string? customerName{get; set;}
    [Required]
    public string? message{get;set;}
    [Required]
    public string? date{get; set;}
}