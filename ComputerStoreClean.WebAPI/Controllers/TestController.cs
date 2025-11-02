using ComputerStoreClean.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerStoreClean.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("database")]
        public async Task<IActionResult> TestDatabase()
        {
            try
            {
                // Проверяем подключение к базе данных
                var canConnect = await _context.Database.CanConnectAsync();

                // Получаем количество категорий
                var categoriesCount = await _context.Categories.CountAsync();

                return Ok(new
                {
                    message = "Database connection successful",
                    canConnect = canConnect,
                    categoriesCount = categoriesCount,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Database connection failed",
                    error = ex.Message
                });
            }
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }
    }
}
