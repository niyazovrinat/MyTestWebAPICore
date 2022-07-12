using System;
using System.Collections.Generic;
using System.Text;
using TodoApiDTO_Models;

namespace TodoApiDTO_DataAccess.Repository.IRepository
{
    /// <summary>
    /// Интерфейс для работы с БД
    /// </summary>
    public interface ITodoItemsRepository : IRepository<TodoItem>
    {
    }
}
