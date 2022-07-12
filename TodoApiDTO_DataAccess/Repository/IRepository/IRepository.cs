using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TodoApiDTO_DataAccess.Repository.IRepository
{
    /// <summary>
    /// Интерфейс для работы с БД
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Поиск записи
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> FindAsync(long id);

        /// <summary>
        /// Вывод всех записей
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <param name="isTracking"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null,
            bool isTracking = true //Если вы будете только считывать данные(но не будете изменять), то при вызове указывайте false, что бы .NET EF Core не отслеживал запросы
            );

        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="entity"></param>
        void Remove(T entity);

        /// <summary>
        /// Проверка существования записи
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        bool Any(Expression<Func<T, bool>> filter = null);

        /// <summary>
        /// Сохранение изменений
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();
    }
}
