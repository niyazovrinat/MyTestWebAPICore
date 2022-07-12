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
    /// ���������� �� ������ � ��������
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        //������� ���������� ����������
        private readonly ITodoItemsService _todoItemsService;

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="todoItemsService"></param>
        public TodoItemsController(ITodoItemsService todoItemsService)
        {
            _todoItemsService = todoItemsService;
        }

        /// <summary>
        /// �������� ������ ���� �����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ResponseDTO<IEnumerable<TodoItemDTO>>>> GetTodoItems()
        {
            //�������� ������ ���� �����
            var getTodoItems_Resp = await _todoItemsService.GetTodoItems();

            //��������� ���������
            if (!getTodoItems_Resp.IsSuccess)
            {
                //��������� ������ ����������
                if (getTodoItems_Resp.ErrorMessages != null && getTodoItems_Resp.ErrorMessages.Count > 0)
                {
                    //������� ��������� � �������
                    return BadRequest(getTodoItems_Resp);
                }
            }

            //������� ��������� � �������
            return Ok(getTodoItems_Resp);
        }

        /// <summary>
        /// �������� ������
        /// </summary>
        /// <param name="id">id ������</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDTO<TodoItemDTO>>> GetTodoItem(long id)
        {
            //�������� ������
            var getTodoItem_Resp = await _todoItemsService.GetTodoItem(id);

            //��������� ���������
            if (!getTodoItem_Resp.IsSuccess)
            {
                //��������� ������ ����������
                if (getTodoItem_Resp.ErrorMessages != null && getTodoItem_Resp.ErrorMessages.Count > 0)
                {
                    //������� ��������� � �������
                    return BadRequest(getTodoItem_Resp);
                }
            }

            //������� ��������� � �������
            return Ok(getTodoItem_Resp);
        }

        /// <summary>
        /// �������� ������
        /// </summary>
        /// <param name="id">id ������</param>
        /// <param name="todoItemDTO">������ ������</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDTO<TodoItemDTO>>> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            //�������� ������
            var updateTodoItem_Resp = await _todoItemsService.UpdateTodoItem(id, todoItemDTO);

            //��������� ���������
            if (!updateTodoItem_Resp.IsSuccess)
            {
                //��������� ������ ����������
                if (updateTodoItem_Resp.ErrorMessages != null && updateTodoItem_Resp.ErrorMessages.Count > 0)
                {
                    //������� ��������� � �������
                    return BadRequest(updateTodoItem_Resp);
                }
            }

            //������� ��������� � �������
            return Ok(updateTodoItem_Resp);
        }

        /// <summary>
        /// ������� ������
        /// </summary>
        /// <param name="todoItemDTO">������ ������</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ResponseDTO<TodoItemDTO>>> CreateTodoItem(TodoItemDTO todoItemDTO)
        {
            //������� ������
            var createTodoItem_Resp = await _todoItemsService.CreateTodoItem(todoItemDTO);

            //��������� ���������
            if (!createTodoItem_Resp.IsSuccess)
            {
                //��������� ������ ����������
                if (createTodoItem_Resp.ErrorMessages != null && createTodoItem_Resp.ErrorMessages.Count > 0)
                {
                    //������� ��������� � �������
                    return BadRequest(createTodoItem_Resp);
                }
                else
                {
                    //������� ��������� � �������
                    return Ok(createTodoItem_Resp);
                }
            }

            //������� ��������� � �������
            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = createTodoItem_Resp.Result.Id },
                new ResponseDTO<TodoItemDTO>() { Result = _todoItemsService.ItemToDTO(createTodoItem_Resp.Result) });
        }

        /// <summary>
        /// ������� ������
        /// </summary>
        /// <param name="id">id ������</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            //������� ������
            var deleteTodoItem_Resp = await _todoItemsService.DeleteTodoItem(id);

            //��������� ���������
            if (!deleteTodoItem_Resp.IsSuccess)
            {
                //��������� ������ ����������
                if (deleteTodoItem_Resp.ErrorMessages != null && deleteTodoItem_Resp.ErrorMessages.Count > 0)
                {
                    //������� ��������� � �������
                    return BadRequest(deleteTodoItem_Resp);
                }
            }

            //������� ��������� � �������
            return Ok(deleteTodoItem_Resp);
        } 
    }
}
