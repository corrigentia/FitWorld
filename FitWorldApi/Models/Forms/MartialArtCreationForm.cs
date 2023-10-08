﻿using System.ComponentModel.DataAnnotations;

namespace FitWorldApi.Models.Forms
{
    public class MartialArtCreationForm
    {
        [Required]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 42, MinimumLength = 1)]
        public string? Name { get; set; }
    }
}
