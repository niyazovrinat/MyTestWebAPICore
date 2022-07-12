using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace TodoApiDTO_Models.DTO
{
    /// <summary>
    /// Класс - модель вывода задач TodoItemDTO
    /// </summary>
    public class TodoItemDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Заполнен
        /// </summary>
        public bool IsComplete { get; set; }
    }

    /// <summary>
    /// Валидация входных параметров
    /// </summary>
    public class TodoItemDTOValidator : AbstractValidator<TodoItemDTO>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public TodoItemDTOValidator()
        {
            RuleFor(c => c.Name)
                .NotNull()
                .NotEmpty();

            RuleFor(c => c.Name)
                .Length(2, 100);
        }
    }
}
