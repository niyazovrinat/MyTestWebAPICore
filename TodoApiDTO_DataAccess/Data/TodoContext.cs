using Microsoft.EntityFrameworkCore;
using TodoApiDTO_Models;

namespace TodoApiDTO_DataAccess.Data
{
    /// <summary>
    /// Класс контекст БД
    /// </summary>
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Задачи
        /// </summary>
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}