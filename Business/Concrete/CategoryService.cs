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

namespace Business.Concrete
{
    public class CategoryService : ServiceBase, ICategoryService
    {
        private readonly IUserRepository _userRepository;

        public CategoryService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [SecuredOperation(Priority = 1)]
        [ValidationAspect(typeof(CategoryCreateViewModelValidator), Priority = 2)]
        [CacheRemoveAspect()]
        public IDataResult<CategoryViewModel> Add(CategoryCreateViewModel category)
        {
            var result = _userRepository.SaveCategory(GetCurrentUserId(), ObjectMapper.Mapper.Map<Category>(category));

            if (!result.Success)
                return new ErrorDataResult<CategoryViewModel>(result.Message);

            var newCategory = ObjectMapper.Mapper.Map<CategoryViewModel>(result.Data);
            return new SuccessDataResult<CategoryViewModel>(newCategory, BusinessMessages.Added);
        }

        [SecuredOperation(Priority = 1)]
        [CacheRemoveAspect()]
        public IResult Delete(Guid categoryId)
        {
            var result = _userRepository.DeleteCategory(GetCurrentUserId(), categoryId);

            if (!result.Success) return new ErrorResult(result.Message);
            return new SuccessResult(BusinessMessages.Deleted);
        }

        [SecuredOperation(Priority = 1)]
        [CacheAspect()]
        public IDataResult<CategoryAndTasksViewModel> Get(Guid categoryId)
        {
            var user = _userRepository.GetById(GetCurrentUserId());
            if (user == null)
                return new ErrorDataResult<CategoryAndTasksViewModel>(BusinessMessages.UserNotFound);

            var category = user.Categories.FirstOrDefault(x => x.Id == categoryId);
            if (category == null)
                return new ErrorDataResult<CategoryAndTasksViewModel>(BusinessMessages.CategoryNotFound);

            var result = ObjectMapper.Mapper.Map<CategoryAndTasksViewModel>(category);
            return new SuccessDataResult<CategoryAndTasksViewModel>(result);
        }

        [SecuredOperation(Priority = 1)]
        [CacheAspect()]
        public IDataResult<List<CategoryViewModel>> GetAll(string searchTerm = "")
        {
            var user = _userRepository.GetById(GetCurrentUserId());
            if (user == null)
                return new ErrorDataResult<List<CategoryViewModel>>(BusinessMessages.UserNotFound);

            searchTerm = searchTerm.Trim();
            var categorys = user.Categories;
            if (!string.IsNullOrEmpty(searchTerm))
                categorys = categorys.Where(x => x.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            var response = ObjectMapper.Mapper.Map<List<CategoryViewModel>>(categorys);
            return new SuccessDataResult<List<CategoryViewModel>>(response);
        }

        [SecuredOperation(Priority = 1)]
        [CacheRemoveAspect()]
        public IResult Update(Guid categoryId, CategoryCreateViewModel category)
        {
            var categoryEntity = ObjectMapper.Mapper.Map<Category>(category);
            categoryEntity.Id = categoryId;
            var result = _userRepository.UpdateCategory(GetCurrentUserId(), categoryEntity);

            if (!result.Success) return new ErrorResult(result.Message);
            return new SuccessResult(BusinessMessages.Updated);
        }
    }
}
