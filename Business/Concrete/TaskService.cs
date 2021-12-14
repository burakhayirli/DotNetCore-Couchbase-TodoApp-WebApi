using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Mapping;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Messages;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class TaskService : ServiceBase, ITaskService
    {
        private readonly IUserRepository _userRepository;

        public TaskService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
                
        [SecuredOperation(Priority = 1)]
        [ValidationAspect(typeof(CategoryTaskCreateViewModelValidator), Priority = 2)]
        [CacheRemoveAspect()]
        public IDataResult<CategoryTaskViewModel> Add(Guid categoryId, CategoryTaskCreateViewModel categoryTaskCreateViewModel)
        {
            var result = _userRepository.SaveTask(GetCurrentUserId(), categoryId, ObjectMapper.Mapper.Map<CategoryTask>(categoryTaskCreateViewModel));

            if (!result.Success)
                return new ErrorDataResult<CategoryTaskViewModel>(result.Message);

            var newTask = ObjectMapper.Mapper.Map<CategoryTaskViewModel>(result.Data);
            return new SuccessDataResult<CategoryTaskViewModel>(newTask, BusinessMessages.Added);
        }

        [SecuredOperation(Priority = 1)]
        [CacheRemoveAspect()]
        public IResult Delete(Guid categoryId, Guid taskId)
        {
            var result = _userRepository.DeleteTask(GetCurrentUserId(), categoryId, taskId);

            if (!result.Success) return new ErrorResult(result.Message);
            return new SuccessResult(result.Message);
        }

        [SecuredOperation(Priority = 1)]
        [CacheAspect()]
        public IDataResult<List<CategoryTaskViewModel>> GetAll(Guid categoryId)
        {
            var user = _userRepository.GetById(GetCurrentUserId());
            if (user == null)
                return new ErrorDataResult<List<CategoryTaskViewModel>>(BusinessMessages.UserNotFound);

            var category = user.Categories.FirstOrDefault(x => x.Id.Equals(categoryId));
            if (category == null)
                return new ErrorDataResult<List<CategoryTaskViewModel>>(BusinessMessages.CategoryNotFound);

            if(category.Tasks==null || category.Tasks.Count==0)
                return new ErrorDataResult<List<CategoryTaskViewModel>>(BusinessMessages.TaskNotFoundBelongsToCategory);

            var tasks=ObjectMapper.Mapper.Map<List<CategoryTaskViewModel>>(category.Tasks);
            return new SuccessDataResult<List<CategoryTaskViewModel>>(tasks);
        }

        [SecuredOperation(Priority = 1)]
        [ValidationAspect(typeof(CategoryTaskCreateViewModelValidator), Priority = 2)]
        [CacheRemoveAspect()]
        public IResult Update(Guid categoryId, Guid taskId, CategoryTaskCreateViewModel categoryTaskCreateViewModel)
        {
            var task = ObjectMapper.Mapper.Map<CategoryTask>(categoryTaskCreateViewModel);
            task.Id = taskId;
            var result = _userRepository.UpdateTask(GetCurrentUserId(), categoryId, task);

            if (!result.Success) return new ErrorResult(result.Message);
            return new SuccessResult(result.Message);
        }
    }
}
