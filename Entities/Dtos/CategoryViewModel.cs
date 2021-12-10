using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Dtos
{    public class CategoryCreateViewModel
    {
        [Required]
        public string Name { get; set; }
    }

    public class CategoryViewModel : CategoryCreateViewModel
    {
        public Guid Id { get; set; }
    }

    public class CategoryAndTasksViewModel : CategoryViewModel
    {
        public List<CategoryTaskCreateViewModel> Tasks { get; set; } = new List<CategoryTaskCreateViewModel>();
    }
}
