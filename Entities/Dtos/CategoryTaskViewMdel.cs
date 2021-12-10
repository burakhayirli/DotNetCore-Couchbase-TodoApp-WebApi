using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Dtos
{
    public class CategoryTaskCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
        [Required]
        public bool IsImportant { get; set; }
        public DateTime? Deadline { get; set; }
    }

    public class CategoryTaskViewModel : CategoryTaskCreateViewModel
    {
        public Guid Id { get; set; }
    }
}
