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

        [HttpGet]
        public IEnumerable<Employee> Get() => _employeesData.Get();

        [HttpGet("{id:int}")]
        public Employee GetById(int id) => _employeesData.GetById(id);

        [HttpPost]
        public int Add(Employee employee)
        {
            var id = _employeesData.Add(employee);
            SaveChanges();
            return id;
        }

        [HttpPut]
        public void Edit(Employee employee)
        {
            _employeesData.Edit(employee);
            SaveChanges();
        }

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