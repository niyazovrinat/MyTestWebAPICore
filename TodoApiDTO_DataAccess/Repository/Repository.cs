using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TodoApiDTO_DataAccess.Data;
using TodoApiDTO_DataAccess.Repository.IRepository;

namespace TodoApiDTO_DataAccess.Repository
{
    /// <summary>
    /// Класс для работы с БД
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly TodoContext _db;
        internal DbSet<T> dbSet;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="db"></param>
        public Repository(TodoContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        /// <summary>
        /// Проверка существования записи
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public bool Any(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = dbSet;

            //Фильтр
            if (filter != null)
            {
                return query.Any(filter);
            }

            return query.Any();
        }

        /// <summary>
        /// Поиск записи
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> FindAsync(long id)
        {
            return await dbSet.FindAsync(id);
        }

        /// <summary>
        /// Вывод всех записей
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <param name="isTracking"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null,
            bool isTracking = true)
        {
            IQueryable<T> query = dbSet;

            //Фильтр
            if (filter != null)
            {
                query = query.Where(filter);
            }

            //Жадная загрузка(Eager loading)
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            //Сортировка
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            //Отслеживать запрос EF
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        /// <summary>
        /// Сохранение изменений
        /// </summary>
        /// <returns></returns>
        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
