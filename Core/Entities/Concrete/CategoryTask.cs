using System;

namespace Core.Entities.Concrete
{
    public class CategoryTask : CouchbaseEntity<CategoryTask>
    {
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsImportant { get; set; }
        public DateTime? Deadline { get; set; }

    }
}
