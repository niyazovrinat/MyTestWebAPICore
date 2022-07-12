using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApiDTO_Models
{
    /// <summary>
    /// Класс - модель задач TodoItem
    /// </summary>
    public class TodoItem
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Заполнен
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Секретное слово
        /// </summary>
        [MaxLength(500)]
        public string Secret { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}