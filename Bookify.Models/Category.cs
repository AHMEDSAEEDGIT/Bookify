﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bookify.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        [Range(1,100,ErrorMessage ="Display Order must be between 1-100")]
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
    }
}

