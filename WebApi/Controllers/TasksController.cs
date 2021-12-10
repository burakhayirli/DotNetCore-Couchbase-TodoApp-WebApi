using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Controllers;

namespace WebApi.Controllers
{
    [Route("api/categories/{categoryId}/[controller]")]
    [ApiController]
    public class TasksController : BaseApiController
    {
        private readonly ITaskService _taskService;
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public IActionResult GetAll(Guid categoryId)
        {
            var result = _taskService.GetAll(categoryId);

            if (result.Success) return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpPost]
        public IActionResult Add(Guid categoryId, CategoryTaskCreateViewModel categoryTaskCreateViewModel)
        {
            var result = _taskService.Add(categoryId,categoryTaskCreateViewModel);

            if (result.Success) return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpPut("{taskId}")]
        public IActionResult Put(Guid categoryId, Guid taskId,CategoryTaskCreateViewModel categoryTaskCreateViewModel)
        {
            var result = _taskService.Update(categoryId, taskId,categoryTaskCreateViewModel);

            if (result.Success) return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpDelete("{taskId}")]
        public IActionResult Delete(Guid categoryId, Guid taskId)
        {
            var result = _taskService.Delete(categoryId, taskId);

            if (result.Success) return Ok(result);
            return BadRequest(result.Message);
        }
    }
}
