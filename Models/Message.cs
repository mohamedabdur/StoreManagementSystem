namespace practice.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class Message
{
    [Key]
    public string? id{get; set;}
    public string? customerName{get; set;}
    public string? message{get;set;}
    public string? date{get; set;}
}