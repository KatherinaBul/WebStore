using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Services.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesData
    {
        public EmployeesClient(IConfiguration configuration) : base(configuration, "api/employees")
        {
        }

        public IEnumerable<Employee> Get() => Get<IEnumerable<Employee>>(ServiceAddress);

        public Employee GetById(int id) => Get<Employee>($"{ServiceAddress}/{id}");

        public int Add(Employee employee) => Post(ServiceAddress, employee).Content.ReadAsAsync<int>().Result;

        public void Edit(Employee employee) => Put(ServiceAddress, employee);

        public bool Delete(int id) => Delete($"{ServiceAddress}/{id}").IsSuccessStatusCode;

        public void SaveChanges()
        {
        }
    }
}