using Core.Entities.Concrete;
using Core.Utilities.Security.Hashing;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.NUnitTests.Helpers
{
    public class DataHelper
    {
        public static User GetUser(string email)
        {
            HashingHelper.CreatePasswordHash("1", out var passwordSalt, out var passwordHash);

            return new User()
            {
                //Id = 1,
                Email = "burak@burak.com",
                FirstName = "Burak",
                LastName = "Hayırlı",
                PasswordHash = passwordSalt,
                PasswordSalt = passwordHash,
                Status = true,
                Categories = new List<Category>()
            };
        }

        public static List<User> GetUserList()
        {

            HashingHelper.CreatePasswordHash("1", out var passwordSalt, out var passwordHash);
            var list = new List<User>();

            for (var i = 1; i <= 5; i++)
            {
                var user = new User()
                {
                    //Id = 1,
                    Email = "test@test.com",
                    FirstName = "Burak",
                    LastName = "Hayırlı",
                    PasswordHash = passwordSalt,
                    PasswordSalt = passwordHash,
                    Status = true,
                    Categories = new List<Category>()
                };
                list.Add(user);
            }

            return list;
        }
    }
}
