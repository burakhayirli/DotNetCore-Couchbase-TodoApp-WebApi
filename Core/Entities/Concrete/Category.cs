using System.Collections.Generic;

namespace Core.Entities.Concrete
{
    public class Category : CouchbaseEntity<Category>
    {
        public string Name { get; set; }
        public List<CategoryTask> Tasks { get; set; } = new List<CategoryTask>();
    }
}
