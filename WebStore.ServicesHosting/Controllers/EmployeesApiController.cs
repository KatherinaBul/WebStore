using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.ServicesHosting.Controllers
{
    [Produces("application/json")]
    [Route(WebApi.Employees)]
    [ApiController]
    public class EmployeesApiController : Controller, IEmployeesData
    {
        private readonly IEmployeesData _employeesData;

        public EmployeesApiController(IEmployeesData employeesData) => _employeesData = employeesData;

        /// <summary>
        /// Получить всех сотрудников
        /// </summary>
        /// <returns>Список сотрудников</returns>
        [HttpGet]
        public IEnumerable<Employee> Get() => _employeesData.Get();

        /// <summary>
        /// Получить сотрудника по идентификатору
        /// </summary>
        /// <param name="id">Индентификатор сотрудника</param>
        /// <returns>Информация о сотруднике</returns>
        [HttpGet("{id:int}")]
        public Employee GetById(int id) => _employeesData.GetById(id);

        /// <summary>
        /// Добавить нового сотрудника
        /// </summary>
        /// <param name="employee">Информация о сотруднике</param>
        /// <returns>Идентификатор нового сотрудника</returns>
        [HttpPost]
        public int Add(Employee employee)
        {
            var id = _employeesData.Add(employee);
            SaveChanges();
            return id;
        }

        /// <summary>
        /// Обновить информацию о сотруднике
        /// </summary>
        /// <param name="employee">Обновленная информация о сотруднике</param>
        [HttpPut]
        public void Edit(Employee employee)
        {
            _employeesData.Edit(employee);
            SaveChanges();
        }

        /// <summary>
        /// Удалить сотрудника
        /// </summary>
        /// <param name="id">Идентификатор сотрудника</param>
        /// <returns>Результат удаления</returns>
        [HttpDelete("{id:int}")]
        public bool Delete(int id)
        {
            var result = _employeesData.Delete(id);
            SaveChanges();
            return result;
        }

        [NonAction]
        public void SaveChanges() => _employeesData.SaveChanges();
    }
}