using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ITaskService
    {
        IDataResult<List<CategoryTaskViewModel>> GetAll(Guid categoryId);
        IDataResult<CategoryTaskViewModel> Add(Guid categoryId, CategoryTaskCreateViewModel categoryTaskCreateViewModel);
        IResult Update(Guid categoryId, Guid taskId, CategoryTaskCreateViewModel categoryTaskCreateViewModel);
        IResult Delete(Guid categoryId, Guid taskId);
    }
}
