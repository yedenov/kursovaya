using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using aspnetcore.Data;
using aspnetcore.Models;
using Microsoft.Extensions.Logging;

public class StudentsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(ApplicationDbContext context, ILogger<StudentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Students
    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Открытие списка студентов");
        return View(await _context.Students.ToListAsync());
    }

    // GET: Students/Create
    public IActionResult Create()
    {
        _logger.LogInformation("Открытие формы создания студента");
        return View();
    }

    // POST: Students/Create
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] Student student)
    {
        _logger.LogInformation("Попытка создания студента: {Name}", student.Name);

        try
        {
            if (string.IsNullOrEmpty(student.Name))
            {
                _logger.LogWarning("Попытка создания студента с пустым именем");
                ModelState.AddModelError("Name", "Имя студента обязательно");
                return View(student);
            }

            _context.Add(student);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Студент успешно создан: {Name}", student.Name);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании студента");
            ModelState.AddModelError("", "Произошла ошибка при сохранении данных");
            return View(student);
        }
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var student = await _context.Students
                .Include(s => s.Attendances)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            // Проверяем, есть ли связанные записи посещаемости
            if (student.Attendances.Any())
            {
                TempData["Error"] = "Нельзя удалить студента, у которого есть записи посещаемости. Сначала удалите все записи посещаемости этого студента.";
                return RedirectToAction(nameof(Index));
            }

            return View(student);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при открытии формы удаления студента: Id={Id}", id);
            TempData["Error"] = "Произошла ошибка при открытии формы удаления";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var student = await _context.Students
                .Include(s => s.Attendances)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            // Проверяем, есть ли связанные записи посещаемости
            if (student.Attendances.Any())
            {
                TempData["Error"] = "Нельзя удалить студента, у которого есть записи посещаемости. Сначала удалите все записи посещаемости этого студента.";
                return RedirectToAction(nameof(Index));
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Студент успешно удален: Id={Id}, Name={Name}", id, student.Name);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении студента: Id={Id}", id);
            TempData["Error"] = "Произошла ошибка при удалении студента";
            return RedirectToAction(nameof(Index));
        }
    }

    // ... остальные методы контроллера ...
}