﻿using System.ComponentModel.DataAnnotations;

namespace Entities.Models;

public class Book
{
    public int Id { get; set; }
    
    [Display(Name = "Book Title")]
    public string Title { get; set; }
    
    public string genre { get; set; }
    
    [DataType(DataType.Currency)]
    [Range(1, 100)]
    public decimal Price { get; set; }
    
    [Display(Name = "Publish Date")]
    [DataType(DataType.Date)]
    public DateTime PublishDate { get; set; }
}