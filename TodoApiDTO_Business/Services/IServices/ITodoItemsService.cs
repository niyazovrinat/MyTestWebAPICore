using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TodoApiDTO_Models;
using TodoApiDTO_Models.DTO;

namespace TodoApi_Business.Services.IServices
{
    /// <summary>
    /// Интерфейс - Сервис по работе с задачами
    /// </summary>
    public interface ITodoItemsService
    {
        /// <summary>
        /// Получить список всех задач
        /// </summary>
        /// <returns></returns>
        Task<ResponseDTO<IEnumerable<TodoItemDTO>>> GetTodoItems();

        /// <summary>
        /// Получить задачу
        /// </summary>
        /// <param name="id">id задачи</param>
        /// <returns></returns>
        Task<ResponseDTO<TodoItemDTO>> GetTodoItem(long id);

        /// <summary>
        /// Обновить задачу
        /// </summary>
        /// <param name="id">id задачи</param>
        /// <param name="todoItemDTO">детали задачи</param>
        /// <returns></returns>
        Task<ResponseDTO<TodoItemDTO>> UpdateTodoItem(long id, TodoItemDTO todoItemDTO);

        /// <summary>
        /// Создать задачу
        /// </summary>
        /// <param name="todoItemDTO">детали задачи</param>
        /// <returns></returns>
        Task<ResponseDTO<TodoItem>> CreateTodoItem(TodoItemDTO todoItemDTO);

        /// <summary>
        /// Удалить задачу
        /// </summary>
        /// <param name="id">id задачи</param>
        /// <returns></returns>
        Task<ResponseDTO<bool>> DeleteTodoItem(long id);

        /// <summary>
        /// Проверить существование задачи
        /// </summary>
        /// <param name="id">id задачи</param>
        /// <returns></returns>
        TodoItemDTO ItemToDTO(TodoItem todoItem);
    }
}
