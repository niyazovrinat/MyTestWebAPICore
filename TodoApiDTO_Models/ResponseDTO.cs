using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApiDTO_Models
{
    /// <summary>
    /// Класс - модель вывода результата
    /// </summary>
    public class ResponseDTO<T>
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        public bool IsSuccess { get; set; } = true;

        /// <summary>
        /// Данные
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// Сообщение для вывода
        /// </summary>
        public string DisplayMessage { get; set; }

        /// <summary>
        /// Список ошибок
        /// </summary>
        public List<string> ErrorMessages { get; set; }
    }
}
