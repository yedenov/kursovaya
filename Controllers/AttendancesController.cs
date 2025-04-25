using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aspnetcore.Data;
using aspnetcore.Models;
using Microsoft.Extensions.Logging;

public class AttendancesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AttendancesController> _logger;

    public AttendancesController(ApplicationDbContext context, ILogger<AttendancesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var attendances = _context.Attendances
            .Include(a => a.Student)
            .Include(a => a.Lesson);
        return View(await attendances.ToListAsync());
    }

    public async Task<IActionResult> Create()
    {
        try
        {
            var students = await _context.Students.ToListAsync();
            var lessons = await _context.Lessons.ToListAsync();

            if (!students.Any())
            {
                _logger.LogWarning("Нет доступных студентов");
                TempData["Error"] = "Нет доступных студентов. Сначала создайте студента.";
                return RedirectToAction(nameof(Index));
            }

            if (!lessons.Any())
            {
                _logger.LogWarning("Нет доступных занятий");
                TempData["Error"] = "Нет доступных занятий. Сначала создайте занятие.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.StudentId = new SelectList(students, "Id", "Name");
            ViewBag.LessonId = new SelectList(lessons, "Id", "Subject");

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при открытии формы создания посещаемости");
            TempData["Error"] = "Произошла ошибка при открытии формы";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int StudentId, int LessonId, bool IsPresent = false)
    {
        _logger.LogInformation("Попытка создания записи посещаемости: StudentId={StudentId}, LessonId={LessonId}, IsPresent={IsPresent}",
            StudentId, LessonId, IsPresent);

        try
        {
            // Проверяем существование студента и занятия
            var student = await _context.Students.FindAsync(StudentId);
            var lesson = await _context.Lessons.FindAsync(LessonId);

            if (student == null)
            {
                _logger.LogWarning("Студент не найден: StudentId={StudentId}", StudentId);
                TempData["Error"] = "Выбранный студент не существует";
                return RedirectToAction(nameof(Create));
            }

            if (lesson == null)
            {
                _logger.LogWarning("Занятие не найдено: LessonId={LessonId}", LessonId);
                TempData["Error"] = "Выбранное занятие не существует";
                return RedirectToAction(nameof(Create));
            }

            var attendance = new Attendance
            {
                StudentId = StudentId,
                LessonId = LessonId,
                IsPresent = IsPresent,
                Student = student,
                Lesson = lesson
            };

            _context.Add(attendance);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Запись посещаемости успешно создана");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании записи посещаемости");
            TempData["Error"] = "Произошла ошибка при сохранении данных";
            return RedirectToAction(nameof(Create));
        }
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var attendance = await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Lesson)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attendance == null)
            {
                return NotFound();
            }

            var students = await _context.Students.ToListAsync();
            var lessons = await _context.Lessons.ToListAsync();

            ViewBag.StudentId = new SelectList(students, "Id", "Name");
            ViewBag.LessonId = new SelectList(lessons, "Id", "Subject");

            return View(attendance);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при открытии формы редактирования посещаемости: Id={Id}", id);
            TempData["Error"] = "Произошла ошибка при открытии формы редактирования";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, int StudentId, int LessonId, bool IsPresent = false)
    {
        _logger.LogInformation("Попытка редактирования записи посещаемости: Id={Id}, StudentId={StudentId}, LessonId={LessonId}, IsPresent={IsPresent}", 
            id, StudentId, LessonId, IsPresent);

        try
        {
            var attendance = await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Lesson)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attendance == null)
            {
                return NotFound();
            }

            // Проверяем существование студента и занятия
            var student = await _context.Students.FindAsync(StudentId);
            var lesson = await _context.Lessons.FindAsync(LessonId);

            if (student == null)
            {
                _logger.LogWarning("Студент не найден: StudentId={StudentId}", StudentId);
                TempData["Error"] = "Выбранный студент не существует";
                return RedirectToAction(nameof(Edit), new { id });
            }

            if (lesson == null)
            {
                _logger.LogWarning("Занятие не найдено: LessonId={LessonId}", LessonId);
                TempData["Error"] = "Выбранное занятие не существует";
                return RedirectToAction(nameof(Edit), new { id });
            }

            attendance.StudentId = StudentId;
            attendance.LessonId = LessonId;
            attendance.IsPresent = IsPresent;
            attendance.Student = student;
            attendance.Lesson = lesson;

            _context.Update(attendance);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Запись посещаемости успешно обновлена: Id={Id}", id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при редактировании записи посещаемости: Id={Id}", id);
            TempData["Error"] = "Произошла ошибка при сохранении данных";
            return RedirectToAction(nameof(Edit), new { id });
        }
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var attendance = await _context.Attendances
            .Include(a => a.Student)
            .Include(a => a.Lesson)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (attendance == null) return NotFound();

        return View(attendance);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var attendance = await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Lesson)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Запись посещаемости успешно удалена: Id={Id}", id);
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении записи посещаемости: Id={Id}", id);
            TempData["Error"] = "Произошла ошибка при удалении записи";
            return RedirectToAction(nameof(Index));
        }
    }
}