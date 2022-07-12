using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi_Business.Services.IServices;
using TodoApiDTO_Models;
using TodoApiDTO_Models.DTO;

namespace TodoApi.Controllers
{
    /// <summary>
    /// Контроллер по работе с задачами
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        //Создать глобальные переменные
        private readonly ITodoItemsService _todoItemsService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="todoItemsService"></param>
        public TodoItemsController(ITodoItemsService todoItemsService)
        {
            _todoItemsService = todoItemsService;
        }

        /// <summary>
        /// Получить список всех задач
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ResponseDTO<IEnumerable<TodoItemDTO>>>> GetTodoItems()
        {
            //Получить список всех задач
            var getTodoItems_Resp = await _todoItemsService.GetTodoItems();

            //Проверить результат
            if (!getTodoItems_Resp.IsSuccess)
            {
                //Проверить список исключений
                if (getTodoItems_Resp.ErrorMessages != null && getTodoItems_Resp.ErrorMessages.Count > 0)
                {
                    //Вернуть результат с ошибкой
                    return BadRequest(getTodoItems_Resp);
                }
            }

            //Вернуть результат с успехом
            return Ok(getTodoItems_Resp);
        }

        /// <summary>
        /// Получить задачу
        /// </summary>
        /// <param name="id">id задачи</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDTO<TodoItemDTO>>> GetTodoItem(long id)
        {
            //Получить задачу
            var getTodoItem_Resp = await _todoItemsService.GetTodoItem(id);

            //Проверить результат
            if (!getTodoItem_Resp.IsSuccess)
            {
                //Проверить список исключений
                if (getTodoItem_Resp.ErrorMessages != null && getTodoItem_Resp.ErrorMessages.Count > 0)
                {
                    //Вернуть результат с ошибкой
                    return BadRequest(getTodoItem_Resp);
                }
            }

            //Вернуть результат с успехом
            return Ok(getTodoItem_Resp);
        }

        /// <summary>
        /// Обновить задачу
        /// </summary>
        /// <param name="id">id задачи</param>
        /// <param name="todoItemDTO">детали задачи</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDTO<TodoItemDTO>>> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            //Обновить задачу
            var updateTodoItem_Resp = await _todoItemsService.UpdateTodoItem(id, todoItemDTO);

            //Проверить результат
            if (!updateTodoItem_Resp.IsSuccess)
            {
                //Проверить список исключений
                if (updateTodoItem_Resp.ErrorMessages != null && updateTodoItem_Resp.ErrorMessages.Count > 0)
                {
                    //Вернуть результат с ошибкой
                    return BadRequest(updateTodoItem_Resp);
                }
            }

            //Вернуть результат с успехом
            return Ok(updateTodoItem_Resp);
        }

        /// <summary>
        /// Создать задачу
        /// </summary>
        /// <param name="todoItemDTO">детали задачи</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ResponseDTO<TodoItemDTO>>> CreateTodoItem(TodoItemDTO todoItemDTO)
        {
            //Создать задачу
            var createTodoItem_Resp = await _todoItemsService.CreateTodoItem(todoItemDTO);

            //Проверить результат
            if (!createTodoItem_Resp.IsSuccess)
            {
                //Проверить список исключений
                if (createTodoItem_Resp.ErrorMessages != null && createTodoItem_Resp.ErrorMessages.Count > 0)
                {
                    //Вернуть результат с ошибкой
                    return BadRequest(createTodoItem_Resp);
                }
                else
                {
                    //Вернуть результат с успехом
                    return Ok(createTodoItem_Resp);
                }
            }

            //Вернуть результат с успехом
            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = createTodoItem_Resp.Result.Id },
                new ResponseDTO<TodoItemDTO>() { Result = _todoItemsService.ItemToDTO(createTodoItem_Resp.Result) });
        }

        /// <summary>
        /// Удалить задачу
        /// </summary>
        /// <param name="id">id задачи</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            //Удалить задачу
            var deleteTodoItem_Resp = await _todoItemsService.DeleteTodoItem(id);

            //Проверить результат
            if (!deleteTodoItem_Resp.IsSuccess)
            {
                //Проверить список исключений
                if (deleteTodoItem_Resp.ErrorMessages != null && deleteTodoItem_Resp.ErrorMessages.Count > 0)
                {
                    //Вернуть результат с ошибкой
                    return BadRequest(deleteTodoItem_Resp);
                }
            }

            //Вернуть результат с успехом
            return Ok(deleteTodoItem_Resp);
        } 
    }
}
