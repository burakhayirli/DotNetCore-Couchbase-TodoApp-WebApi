using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<CategoryAndTasksViewModel> Categories { get; set; } = new List<CategoryAndTasksViewModel>();
    }
}
