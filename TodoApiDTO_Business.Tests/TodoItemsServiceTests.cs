using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi_Business.Services;
using TodoApi_Business.Services.IServices;
using TodoApiDTO_Business.Mapping;
using TodoApiDTO_DataAccess.Repository.IRepository;
using TodoApiDTO_Models;
using TodoApiDTO_Models.DTO;

namespace TodoApiDTO_Business.Tests
{
    /// <summary>
    /// Класс для тестирования TodoItemsService
    /// </summary>
    [TestFixture]
    public class TodoItemsServiceTests
    {
        //Создать глобальные переменные
        private ITodoItemsRepository _todoItemsRepositoryMock;
        private ITodoItemsService _todoItemsService;
        private List<TodoItemDTO> _getTodoItems_EXPECTED;
        private List<TodoItemDTO> _getTodoItems_EXPECTED_UPDATED;
        private List<TodoItem> _getTodoItems_MOCK;
        private IMapper _mapper;

        #region SetupAndTeardown
        /// <summary>
        /// Метод вызываемый перед запуском каждого теста
        /// </summary>
        [SetUp]
        public void Setup()
        {
            //Создать объект Mapper
            _mapper = MappingConfig.RegisterMaps().CreateMapper();

            //Настроить подставку
            _todoItemsRepositoryMock = Substitute.For<ITodoItemsRepository>();

            //Создать объект сервиса
            _todoItemsService = new TodoItemsService(_todoItemsRepositoryMock, _mapper);

            #region Подготовить ожидаемые данные
            _getTodoItems_EXPECTED = new List<TodoItemDTO>()
            {
                new TodoItemDTO() { Id = 1, Name = "Test1", IsComplete = true },
                new TodoItemDTO() { Id = 2, Name = "Test2", IsComplete = false },
                new TodoItemDTO() { Id = 3, Name = "Test3", IsComplete = true }
            };

            _getTodoItems_EXPECTED_UPDATED = new List<TodoItemDTO>();
            for (int i = 1; i <= 3; i++)
            {
                _getTodoItems_EXPECTED_UPDATED.Add(new TodoItemDTO()
                {
                    Id = i,
                    Name = "Test" + i + "_Test",
                    IsComplete = (i == 2 ? false : true)
                });
            }
            #endregion

            #region Подготовить данные подставки
            _getTodoItems_MOCK = new List<TodoItem>() 
            { 
                new TodoItem() { Id = 1, Name = "Test1", IsComplete = true, Secret = "SecretTest1" },
                new TodoItem() { Id = 2, Name = "Test2", IsComplete = false, Secret = "SecretTest2" },
                new TodoItem() { Id = 3, Name = "Test3", IsComplete = true, Secret = "SecretTest3" }
            };

            _getTodoItems_EXPECTED_UPDATED = new List<TodoItemDTO>();
            for (int i = 1; i <= 3; i++)
            {
                _getTodoItems_EXPECTED_UPDATED.Add(new TodoItemDTO()
                {
                    Id = i,
                    Name = "Test" + i + "_Test",
                    IsComplete = (i == 2 ? false : true)
                });
            }
            #endregion
        }

        /// <summary>
        /// Метод вызываемый после запуска каждого теста
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            _todoItemsRepositoryMock = null;
            _todoItemsService = null;
            _getTodoItems_EXPECTED = null;
            _getTodoItems_EXPECTED_UPDATED = null;
        }
        #endregion

        #region GetTodoItems_NoParams_ReturnsListItems
        /// <summary>
        /// Получить список всех задач
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetTodoItems_NoParams_ReturnsListItems()
        {
            //Подготовить методы подставки
            _todoItemsRepositoryMock
                .GetAllAsync()
                .ReturnsForAnyArgs(_getTodoItems_MOCK);

            //Получить список всех задач (Фактический)
            var getTodoItems_ACTUAL = await _todoItemsService.GetTodoItems();
            var getTodoItems_ACTUAL_LIST = getTodoItems_ACTUAL.Result.ToList();

            //Проверить утверждение, что результат IsSuccess
            Assert.That(getTodoItems_ACTUAL.IsSuccess, Is.True);

            //Проверить утверждение, что фактический список равен ожидаемому списку
            Assert.That(getTodoItems_ACTUAL_LIST.Count, Is.EqualTo(_getTodoItems_EXPECTED.Count));
            for (var i = 0; i < _getTodoItems_EXPECTED.Count; i++)
            {
                //Проверить каждое поле
                Assert.That(getTodoItems_ACTUAL_LIST[i].Id, Is.EqualTo(_getTodoItems_EXPECTED[i].Id));
                Assert.That(getTodoItems_ACTUAL_LIST[i].Name, Is.EqualTo(_getTodoItems_EXPECTED[i].Name));
                Assert.That(getTodoItems_ACTUAL_LIST[i].IsComplete, Is.EqualTo(_getTodoItems_EXPECTED[i].IsComplete));
            }
        }
        #endregion

        #region GetTodoItem_123_ReturnsOneItem
        /// <summary>
        /// Получить задачу
        /// </summary>
        /// <param name="id">id задачи</param>
        /// <returns></returns>
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task GetTodoItem_123_ReturnsOneItem(long id)
        {
            //Подготовить методы подставки
            _todoItemsRepositoryMock
                .FindAsync(id)
                .ReturnsForAnyArgs(_getTodoItems_MOCK.FirstOrDefault(v => v.Id == id));

            //Получить задачу (Фактический)
            var getTodoItemOne_ACTUAL = await _todoItemsService.GetTodoItem(id);
            var getTodoItemOne_ACTUAL_ITEM = getTodoItemOne_ACTUAL.Result;

            //Получить задачу (Ожидаемый)
            var getTodoItemOne_EXPECTED = _getTodoItems_EXPECTED.FirstOrDefault(v => v.Id == id);

            //Проверить утверждение, что результат IsSuccess
            Assert.That(getTodoItemOne_ACTUAL.IsSuccess, Is.True);

            //Проверить утверждение, что задача (Фактический) не NULL
            Assert.That(getTodoItemOne_ACTUAL_ITEM, Is.Not.Null);

            //Проверить утверждение, что фактический объект равен ожидаемому объекту
            Assert.That(getTodoItemOne_ACTUAL_ITEM.Id, Is.EqualTo(getTodoItemOne_EXPECTED.Id));
            Assert.That(getTodoItemOne_ACTUAL_ITEM.Name, Is.EqualTo(getTodoItemOne_EXPECTED.Name));
            Assert.That(getTodoItemOne_ACTUAL_ITEM.IsComplete, Is.EqualTo(getTodoItemOne_EXPECTED.IsComplete));
        }
        #endregion

        #region UpdateTodoItem_UpdateObject_ReturnsOneItem
        /// <summary>
        /// Обновить задачу
        /// </summary>
        /// <param name="id">id задачи</param>
        /// <param name="todoItemDTO">детали задачи</param>
        /// <returns></returns>
        [TestCaseSource(typeof(TestCaseSource_UpdateTodoItem_UpdateObject_ReturnsOneItem))]
        public async Task UpdateTodoItem_UpdateObject_ReturnsOneItem(long id, TodoItemDTO todoItemDTO)
        {
            //Подготовить методы подставки
            _todoItemsRepositoryMock
                .FindAsync(id)
                .ReturnsForAnyArgs(_getTodoItems_MOCK.FirstOrDefault(v => v.Id == id));
            _todoItemsRepositoryMock
                .SaveChangesAsync()
                .ReturnsForAnyArgs(Task.CompletedTask);

            //Обновить и получить задачу (Фактический)
            var getTodoItemOne_ACTUAL = await _todoItemsService.UpdateTodoItem(id, todoItemDTO);
            var getTodoItemOne_ACTUAL_ITEM = getTodoItemOne_ACTUAL.Result;

            //Получить задачу (Ожидаемый)
            var getTodoItemOne_EXPECTED = _getTodoItems_EXPECTED_UPDATED.FirstOrDefault(v => v.Id == id);

            //Проверить утверждение, что результат IsSuccess
            Assert.That(getTodoItemOne_ACTUAL.IsSuccess, Is.True);

            //Проверить утверждение, что задача (Фактический) не NULL
            Assert.That(getTodoItemOne_ACTUAL_ITEM, Is.Not.Null);

            //Проверить утверждение, что фактический объект равен ожидаемому объекту
            Assert.That(getTodoItemOne_ACTUAL_ITEM.Id, Is.EqualTo(getTodoItemOne_EXPECTED.Id));
            Assert.That(getTodoItemOne_ACTUAL_ITEM.Name, Is.EqualTo(getTodoItemOne_EXPECTED.Name));
            Assert.That(getTodoItemOne_ACTUAL_ITEM.IsComplete, Is.EqualTo(getTodoItemOne_EXPECTED.IsComplete));
        }

        /// <summary>
        /// Класс для передачи параметров в тест
        /// </summary>
        public class TestCaseSource_UpdateTodoItem_UpdateObject_ReturnsOneItem : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                for (int i = 1; i <= 3; i++)
                {
                    yield return new object[] {
                        i,
                        new TodoItemDTO()
                        {
                            Id = i,
                            Name = "Test" + i + "_Test",
                            IsComplete = (i == 2 ? false : true)
                        }
                    };
                }
            }
        }
        #endregion

        #region CreateTodoItem_CreateObject_ReturnsOneItem
        /// <summary>
        /// Создать задачу
        /// </summary>
        /// <param name="todoItemDTO">детали задачи</param>
        /// <returns></returns>
        [Test]
        public async Task CreateTodoItem_CreateObject_ReturnsOneItem()
        {
            //Подготовить объект (Ожидаемый)
            TodoItem getTodoItemOne_EXPECTED = new TodoItem()
            {
                Id = 10,
                Name = "NewName",
                IsComplete = true,
                Secret = "TestSecret"
            };

            //Подготовить объект для создания (Фактический)
            TodoItemDTO todoItemDTO = new TodoItemDTO()
            {
                Name = "NewName",
                IsComplete = true
            };

            //Подготовить методы подставки
            _todoItemsRepositoryMock
                .Add(getTodoItemOne_EXPECTED);
            _todoItemsRepositoryMock
                .SaveChangesAsync()
                .ReturnsForAnyArgs(Task.CompletedTask);

            //Создать и получить задачу (Фактический)
            var getTodoItemOne_ACTUAL = await _todoItemsService.CreateTodoItem(todoItemDTO);
            var getTodoItemOne_ACTUAL_ITEM = getTodoItemOne_ACTUAL.Result;

            //Проверить утверждение, что результат IsSuccess
            Assert.That(getTodoItemOne_ACTUAL.IsSuccess, Is.True);

            //Проверить утверждение, что задача (Фактический) не NULL
            Assert.That(getTodoItemOne_ACTUAL_ITEM, Is.Not.Null);

            //Проверить утверждение, что фактический объект равен ожидаемому объекту
            Assert.That(getTodoItemOne_ACTUAL_ITEM.Name, Is.EqualTo(getTodoItemOne_EXPECTED.Name));
            Assert.That(getTodoItemOne_ACTUAL_ITEM.IsComplete, Is.EqualTo(getTodoItemOne_EXPECTED.IsComplete));
            Assert.That(getTodoItemOne_ACTUAL_ITEM.Secret, Is.EqualTo(getTodoItemOne_EXPECTED.Secret));
        }
        #endregion

        #region DeleteTodoItem_DeleteObject1_ReturnsTrue
        /// <summary>
        /// Удалить задачу
        /// </summary>
        /// <param name="id">id задачи</param>
        /// <returns></returns>
        [TestCase(1)]
        public async Task DeleteTodoItem_DeleteObject1_ReturnsTrue(long id)
        {
            //Подготовить методы подставки
            _todoItemsRepositoryMock
                .FindAsync(id)
                .ReturnsForAnyArgs(_getTodoItems_MOCK.FirstOrDefault(v => v.Id == id));
            _todoItemsRepositoryMock
                .Remove(Arg.Any<TodoItem>());
            _todoItemsRepositoryMock
                .SaveChangesAsync()
                .ReturnsForAnyArgs(Task.CompletedTask);

            //Создать и получить задачу (Фактический)
            var getTodoItemOne_ACTUAL = await _todoItemsService.DeleteTodoItem(id);

            //Проверить утверждение, что результат IsSuccess
            Assert.That(getTodoItemOne_ACTUAL.IsSuccess, Is.True);

            //Проверить утверждение, что результат (Фактический) равен True
            Assert.That(getTodoItemOne_ACTUAL.Result, Is.True);
        }
        #endregion
    }
}
