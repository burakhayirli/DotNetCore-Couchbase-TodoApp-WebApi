using System.Collections.Generic;

namespace Core.Entities.Concrete
{
    public class User : CouchbaseEntity<User>, IEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public bool Status { get; set; }
        public ICollection<UserOperationClaim> UserOperationClaims { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();

    }
}
