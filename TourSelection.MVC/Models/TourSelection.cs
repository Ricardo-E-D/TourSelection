﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TourSelection.MVC.Models
{
    public class TourSelection
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Tour { get; set; }
        [Required]
        public string TourRequest { get; set; }
    }
}
