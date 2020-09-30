using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Services.Data
{
    public class WebStoreDBInitializer
    {
        private readonly WebStoreDB _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<WebStoreDBInitializer> _logger;

        public WebStoreDBInitializer(WebStoreDB db, UserManager<User> userManager, RoleManager<Role> roleManager,
            ILogger<WebStoreDBInitializer> logger)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public void Initialize()
        {
            _logger.LogInformation("Инициализация БД...");

            var db = _db.Database;

            try
            {
                _logger.LogInformation("Проведение миграций БД");
                db.Migrate();

                _logger.LogInformation("Инициализация каталога товаров");
                InitializeProducts();

                _logger.LogInformation("Инициализация каталога сотрудников");
                InitializeEmployees();

                _logger.LogInformation("Инициализация данных системы Identity");
                InitializeIdentityAsync().Wait();
            }
            catch (Exception error)
            {
                _logger.LogCritical(new EventId(0), error, "Ошибка процесса инициализации базы данных");
                throw;
            }

            _logger.LogInformation("Инициализация БД выполнена успешно");
        }

        private void InitializeProducts()
        {
            if (_db.Products.Any())
            {
                _logger.LogInformation("Каталог товаров уже инициализирован");
                return;
            }

            var db = _db.Database;
            using (db.BeginTransaction())
            {
                _db.Sections.AddRange(TestData.Sections);

                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductSections] ON");
                _db.SaveChanges();
                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductSections] OFF");

                db.CommitTransaction();
            }

            using (db.BeginTransaction())
            {
                _db.Brands.AddRange(TestData.Brands);

                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductBrands] ON");
                _db.SaveChanges();
                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductBrands] OFF");

                db.CommitTransaction();
            }

            using (db.BeginTransaction())
            {
                _db.Products.AddRange(TestData.Products);

                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] ON");
                _db.SaveChanges();
                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] OFF");

                db.CommitTransaction();
            }

            //var products = TestData.Products;
            //var sections = TestData.Sections;
            //var brands = TestData.Brands;

            //var product_section = products.Join(
            //    sections, 
            //    p => p.SectionId, 
            //    s => s.Id, 
            //    (product, section) => (product, section));

            //foreach (var (product, section) in product_section)
            //{
            //    product.Section = section;
            //    product.SectionId = 0;
            //}

            //var product_brand = products.Join(
            //    brands,
            //    p => p.BrandId,
            //    b => b.Id,
            //    (product, brand) => (product, brand));

            //foreach (var (product, brand) in product_brand)
            //{
            //    product.Brand = brand;
            //    product.BrandId = null;
            //}

            //foreach (var product in products)
            //    product.Id = 0;

            //var child_sections = sections.Join(
            //    sections,
            //    child => child.ParentId,
            //    parent => parent.Id,
            //    (child, parent) => (child, parent));

            //foreach (var (child, parent) in child_sections)
            //{
            //    child.ParentSection = parent;
            //    child.ParentId = null;
            //}

            //foreach (var section in sections)
            //    section.Id = 0;

            //foreach (var brand in brands)
            //    brand.Id = 0;


            //using (db.BeginTransaction())
            //{
            //    _db.Sections.AddRange(sections);
            //    _db.Brands.AddRange(brands);
            //    _db.Products.AddRange(products);
            //    _db.SaveChanges();
            //    db.CommitTransaction();
            //}
        }

        private void InitializeEmployees()
        {
            if (_db.Employees.Any())
            {
                _logger.LogInformation("Раздел сотрудников уже инициализирован");
                return;
            }

            using (_db.Database.BeginTransaction())
            {
                TestData.Employees.ForEach(employee => employee.Id = 0);

                _db.Employees.AddRange(TestData.Employees);

                _db.SaveChanges();

                _db.Database.CommitTransaction();
            }
        }

        private async Task InitializeIdentityAsync()
        {
            async Task CheckRoleExist(string roleName)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    _logger.LogInformation("Добавление роли пользователей {0}", roleName);
                    await _roleManager.CreateAsync(new Role {Name = roleName});
                }
            }

            await CheckRoleExist(Role.Administrator);
            await CheckRoleExist(Role.User);

            if (await _userManager.FindByNameAsync(User.Administrator) is null)
            {
                var admin = new User {UserName = User.Administrator};
                var creationResult = await _userManager.CreateAsync(admin, User.DefaultAdminPassword);
                if (creationResult.Succeeded)
                {
                    _logger.LogInformation("Пользователь {0} добавлен", User.Administrator);
                    await _userManager.AddToRoleAsync(admin, Role.Administrator);
                    _logger.LogInformation("Пользователю {0} добавлена роль {1}", User.Administrator,
                        Role.Administrator);
                }
                else
                {
                    var errors = creationResult.Errors.Select(e => e.Description);
                    throw new InvalidOperationException(
                        $"Ошибка при создании пользователя Администратор: {string.Join(", ", errors)}");
                }
            }
        }
    }
}