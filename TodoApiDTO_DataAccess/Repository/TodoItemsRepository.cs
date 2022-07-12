using System;
using System.Collections.Generic;
using System.Text;
using TodoApiDTO_DataAccess.Data;
using TodoApiDTO_DataAccess.Repository.IRepository;
using TodoApiDTO_Models;

namespace TodoApiDTO_DataAccess.Repository
{
    /// <summary>
    /// Класс для работы с БД
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TodoItemsRepository : Repository<TodoItem>, ITodoItemsRepository
    {
        private readonly TodoContext _db;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="db"></param>
        public TodoItemsRepository(TodoContext db) : base(db)
        {
            _db = db;
        }
    }
}
