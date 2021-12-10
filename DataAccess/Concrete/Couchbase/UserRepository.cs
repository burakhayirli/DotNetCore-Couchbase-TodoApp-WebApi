using Core.Entities.Concrete;
using Core.Utilities.Messages;
using Core.Utilities.Results;
using Couchbase.N1QL;
using DataAccess.Abstract;
using System;
using System.Linq;

namespace DataAccess.Concrete.Couchbase
{
    public class UserRepository : CouchbaseRepository<User>, IUserRepository
    {
        public UserRepository(ITodoBucketProvider bucketProvider)
            : base(bucketProvider)
        {
        }

        public User GetById(Guid id)
        {
            return Get(id);
        }

        public User GetByMail(string mailAddress)
        {
            var query = new QueryRequest(
                $@"SELECT t.* 
                FROM {_bucket.Name} as t 
                WHERE type = '{Type}' AND email = '{mailAddress}'
                LIMIT 1;");

            var result = _bucket.Query<User>(query);
            if (result == null || !result.Success)
                return null;

            return result.Rows.ToList().FirstOrDefault();
        }

        public IDataResult<User> Save(User newUser)
        {
            if (GetByMail(newUser.Email) != null)
                return new ErrorDataResult<User>(newUser, BusinessMessages.UserAlreadyExists);

            Create(newUser);
            return new SuccessDataResult<User>(newUser);
        }

        public IDataResult<Category> SaveCategory(Guid userId, Category newCategory)
        {
            var user = Get(userId);
            user.Categories.Add(newCategory);
            Update(user);
            return new SuccessDataResult<Category>(newCategory);
        }

        public IResult UpdateCategory(Guid userId, Category category)
        {
            var user = Get(userId);
            var index = user.Categories.FindIndex(x => x.Id.Equals(category.Id));
            if (index == -1)
                return new ErrorResult(BusinessMessages.CategoryNotFound);

            user.Categories[index] = category;
            Update(user);
            return new SuccessResult();
        }

        public IResult DeleteCategory(Guid userId, Guid categoryId)
        {
            var user = Get(userId);
            var index = user.Categories.FindIndex(x => x.Id.Equals(categoryId));
            if (index == -1)
                return new ErrorResult(BusinessMessages.CategoryNotFound);

            user.Categories.RemoveAt(index);
            Update(user);
            return new SuccessResult();
        }

        public IDataResult<CategoryTask> SaveTask(Guid userId, Guid categoryId, CategoryTask newTask)
        {
            var user = Get(userId);
            var category = user.Categories.FirstOrDefault(x => x.Id.Equals(categoryId));
            if (category == null)
                return new ErrorDataResult<CategoryTask>(BusinessMessages.CategoryNotFound);
            category.Tasks.Add(newTask);
            Update(user);
            return new SuccessDataResult<CategoryTask>(newTask);
        }

        public IResult UpdateTask(Guid userId, Guid categoryId, CategoryTask task)
        {
            var user = Get(userId);
            var category = user.Categories.FirstOrDefault(x => x.Id.Equals(categoryId));
            if (category == null)
                return new ErrorResult(BusinessMessages.CategoryNotFound);

            var index = category.Tasks.FindIndex(x => x.Id.Equals(task.Id));
            if (index == -1)
                return new ErrorResult(BusinessMessages.TaskNotFound);

            category.Tasks[index] = task;
            Update(user);
            return new SuccessResult();
        }

        public IResult DeleteTask(Guid userId, Guid categoryId, Guid taskId)
        {
            var user = Get(userId);
            var category = user.Categories.FirstOrDefault(x => x.Id.Equals(categoryId));
            if (category == null)
                return new ErrorResult(BusinessMessages.CategoryNotFound);

            var index = category.Tasks.FindIndex(x => x.Id.Equals(taskId));
            if (index == -1)
                return new ErrorResult(BusinessMessages.TaskNotFound);

            category.Tasks.RemoveAt(index);
            Update(user);
            return new SuccessResult();
        }
    }
}
