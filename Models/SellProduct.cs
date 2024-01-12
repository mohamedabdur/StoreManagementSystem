using System.ComponentModel.DataAnnotations;

namespace practice.Models;

public class SellProduct{
    [Key]
     public int productid{get;set;}
    // [Required]
    // [RegularExpression("^[a-zA-z]+$",ErrorMessage ="The Product Name should contain only alphabets")]
    public string? productname{get; set;} 
    // [Key]
    // public int productid{get; set;}
    // [Required]
    // [RegularExpression("^[a-zA-z]+$",ErrorMessage="The Category should contain only alphabets")]
    // public string? productcategory{get; set;}

    // [Required]
    // [RegularExpression("^[a-zA-z]+$",ErrorMessage="The Customer Name should contain only alphabets")]

    public string? customername{get; set;}

    // public Single productprice{get; set;}
    [Required]
    public string? senddate{get; set;}
    [Required]
    public int quantity{get;set;}

    public string?  status{get; set;}
}
  