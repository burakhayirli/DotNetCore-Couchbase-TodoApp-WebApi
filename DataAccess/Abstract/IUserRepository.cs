using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IUserRepository
    {
        IDataResult<User> Save(User newUser);
        User GetByMail(string mailAddress);
        User GetById(Guid id);

        IDataResult<Category> SaveCategory(Guid userId, Category newCategory);
        IResult UpdateCategory(Guid userId, Category category);
        IResult DeleteCategory(Guid userId, Guid categoryId);

        IDataResult<CategoryTask> SaveTask(Guid userId, Guid categoryId, CategoryTask newTask);
        IResult UpdateTask(Guid userId, Guid categoryId, CategoryTask task);
        IResult DeleteTask(Guid userId, Guid categoryId, Guid taskId);
    }
}
