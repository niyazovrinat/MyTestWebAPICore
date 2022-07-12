using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi_Business.Services.IServices;
using TodoApiDTO_DataAccess.Repository.IRepository;
using TodoApiDTO_Models;
using TodoApiDTO_Models.DTO;

namespace TodoApi_Business.Services
{
    /// <summary>
    /// Класс - Сервис по работе с задачами
    /// </summary>
    public class TodoItemsService : ITodoItemsService
    {
        //Создать глобальные переменные
        private readonly ITodoItemsRepository _todoItemsRepo;
        private IMapper _mapper;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="todoItemsRepo"></param>
        public TodoItemsService(ITodoItemsRepository todoItemsRepo, IMapper mapper)
        {
            _todoItemsRepo = todoItemsRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить список всех задач
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDTO<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            ResponseDTO<IEnumerable<TodoItemDTO>> result = new ResponseDTO<IEnumerable<TodoItemDTO>>();

            try
            {
                //Получить список всех задач
                var items = await _todoItemsRepo.GetAllAsync();

                //Проверить на null и кол-во
                if (items == null || items.Count() == 0)
                {
                    return result;
                }

                //Сконвертировать в TodoItemDTO и присвоить в результат
                result.Result = items.Select(x => ItemToDTO(x)).ToList();
            }
            catch (AggregateException ex)
            {
                //Задать детали исключения
                result = new ResponseDTO<IEnumerable<TodoItemDTO>>();
                result.IsSuccess = false;
                result.ErrorMessages = new List<string>();
                for (int i = 0; i < ex.InnerExceptions.Count; i++)
                {
                    result.ErrorMessages.Add($"EXCEPTION {i + 1}: " + ex.InnerExceptions[i].ToString() + "; ");
                }
                //Вместо вывода деталей EXCEPTION пользователю, тут должно быть логирование, а пользователю просто вывод "Ошибка! Свяжитесь с администратором!" 
            }

            return result;
        }

        /// <summary>
        /// Получить задачу
        /// </summary>
        /// <param name="id">id задачи</param>
        /// <returns></returns>
        public async Task<ResponseDTO<TodoItemDTO>> GetTodoItem(long id)
        {
            ResponseDTO<TodoItemDTO> result = new ResponseDTO<TodoItemDTO>();

            try
            {
                //Получить задачу
                var todoItem = await _todoItemsRepo.FindAsync(id);

                //Проверить на null
                if (todoItem == null)
                {
                    return result;
                }

                //Сконвертировать в TodoItemDTO и присвоить в результат
                result.Result = ItemToDTO(todoItem);
            }
            catch (AggregateException ex)
            {
                //Задать детали исключения
                result = new ResponseDTO<TodoItemDTO>();
                result.IsSuccess = false;
                result.ErrorMessages = new List<string>();
                for (int i = 0; i < ex.InnerExceptions.Count; i++)
                {
                    result.ErrorMessages.Add($"EXCEPTION {i + 1}: " + ex.InnerExceptions[i].ToString() + "; ");
                }
                //Вместо вывода деталей EXCEPTION пользователю, тут должно быть логирование, а пользователю просто вывод "Ошибка! Свяжитесь с администратором!" 
            }

            return result;
        }

        /// <summary>
        /// Обновить задачу
        /// </summary>
        /// <param name="id">id задачи</param>
        /// <param name="todoItemDTO">детали задачи</param>
        /// <returns></returns>
        public async Task<ResponseDTO<TodoItemDTO>> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            ResponseDTO<TodoItemDTO> result = new ResponseDTO<TodoItemDTO>();

            try
            {
                //Проверить id
                if (id != todoItemDTO.Id)
                {
                    result.IsSuccess = false;
                    result.DisplayMessage = "Id не совпадают!";
                    return result;
                }

                //Получить задачу
                var todoItem = await _todoItemsRepo.FindAsync(id);

                //Проверить на null
                if (todoItem == null)
                {
                    result.IsSuccess = false;
                    result.DisplayMessage = "Id не найден!";
                    return result;
                }

                //Обновить значения
                todoItem.Name = todoItemDTO.Name;
                todoItem.IsComplete = todoItemDTO.IsComplete;

                try
                {
                    //Сохранить изменения
                    await _todoItemsRepo.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) when (!TodoItemExists(id).Result)
                {
                    result.IsSuccess = false;
                    result.DisplayMessage = "Id не найден!";
                    return result;
                }

                result.Result = todoItemDTO;
            }
            catch(AggregateException ex)
            {
                //Задать детали исключения
                result = new ResponseDTO<TodoItemDTO>();
                result.IsSuccess = false;
                result.ErrorMessages = new List<string>();
                for (int i = 0; i < ex.InnerExceptions.Count; i++)
                {
                    result.ErrorMessages.Add($"EXCEPTION {i + 1}: " + ex.InnerExceptions[i].ToString() + "; ");
                }
                //Вместо вывода деталей EXCEPTION пользователю, тут должно быть логирование, а пользователю просто вывод "Ошибка! Свяжитесь с администратором!" 
            }

            return result;
        }

        /// <summary>
        /// Создать задачу
        /// </summary>
        /// <param name="todoItemDTO">детали задачи</param>
        /// <returns></returns>
        public async Task<ResponseDTO<TodoItem>> CreateTodoItem(TodoItemDTO todoItemDTO)
        {
            ResponseDTO<TodoItem> result = new ResponseDTO<TodoItem>();

            try
            {
                //Подготовить объект для добавления
                TodoItem todoItem = _mapper.Map<TodoItemDTO, TodoItem>(todoItemDTO);
                todoItem.Secret = "TestSecret";

                //Добавить новую задачу
                _todoItemsRepo.Add(todoItem);

                //Сохранить изменения
                await _todoItemsRepo.SaveChangesAsync();

                //Присвоить в результат
                result.Result = todoItem;
            }
            catch (AggregateException ex)
            {
                //Задать детали исключения
                result = new ResponseDTO<TodoItem>();
                result.IsSuccess = false;
                result.ErrorMessages = new List<string>();
                for (int i = 0; i < ex.InnerExceptions.Count; i++)
                {
                    result.ErrorMessages.Add($"EXCEPTION {i + 1}: " + ex.InnerExceptions[i].ToString() + "; ");
                }
                //Вместо вывода деталей EXCEPTION пользователю, тут должно быть логирование, а пользователю просто вывод "Ошибка! Свяжитесь с администратором!" 
            }

            return result;
        }

        /// <summary>
        /// Удалить задачу
        /// </summary>
        /// <param name="id">id задачи</param>
        /// <returns></returns>
        public async Task<ResponseDTO<bool>> DeleteTodoItem(long id)
        {
            ResponseDTO<bool> result = new ResponseDTO<bool>();

            try
            {
                //Получить задачу
                var todoItem = await _todoItemsRepo.FindAsync(id);

                //Проверить на null
                if (todoItem == null)
                {
                    result.IsSuccess = false;
                    result.DisplayMessage = "Id не найден!";
                    return result;
                }

                //Удалить задачу
                _todoItemsRepo.Remove(todoItem);

                //Сохранить изменения
                await _todoItemsRepo.SaveChangesAsync();

                //Присвоить результат
                result.Result = true;
            }
            catch (AggregateException ex)
            {
                //Задать детали исключения
                result = new ResponseDTO<bool>();
                result.IsSuccess = false;
                result.ErrorMessages = new List<string>();
                for (int i = 0; i < ex.InnerExceptions.Count; i++)
                {
                    result.ErrorMessages.Add($"EXCEPTION {i + 1}: " + ex.InnerExceptions[i].ToString() + "; ");
                }
                //Вместо вывода деталей EXCEPTION пользователю, тут должно быть логирование, а пользователю просто вывод "Ошибка! Свяжитесь с администратором!" 
            }

            return result;
        }

        /// <summary>
        /// Проверить существование задачи
        /// </summary>
        /// <param name="id">id задачи</param>
        /// <returns></returns>
        private ResponseDTO<bool> TodoItemExists(long id)
        {
            ResponseDTO<bool> result = new ResponseDTO<bool>();

            try
            {
                //Проверить существование задачи
                result.Result = _todoItemsRepo.Any(e => e.Id == id);
            }
            catch (Exception ex)
            {
                //Задать детали исключения
                result = new ResponseDTO<bool>();
                result.IsSuccess = false;
                result.ErrorMessages = new List<string>() { $"EXCEPTION: " + ex.ToString() + "; " };
                //Вместо вывода деталей EXCEPTION пользователю, тут должно быть логирование, а пользователю просто вывод "Ошибка! Свяжитесь с администратором!" 
            }

            return result;
        }

        /// <summary>
        /// Создать обект TodoItemDTO на основе TodoItem
        /// </summary>
        /// <param name="todoItem">детали задачи</param>
        /// <returns></returns>
        public TodoItemDTO ItemToDTO(TodoItem todoItem) => _mapper.Map<TodoItem, TodoItemDTO>(todoItem);
    }
}
