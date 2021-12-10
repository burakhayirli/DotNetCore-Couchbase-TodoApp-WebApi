using Core.Utilities.Results;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        IDataResult<CategoryAndTasksViewModel> Get(Guid categoryId);
        IDataResult<List<CategoryViewModel>> GetAll(string searchTerm="");
        IDataResult<CategoryViewModel> Add(CategoryCreateViewModel category);
        IResult Update(Guid categoryId,CategoryCreateViewModel category);
        IResult Delete(Guid categoryId);
    }
}
