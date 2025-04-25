using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using aspnetcore.Data;
using aspnetcore.Models;

public class LessonsController : Controller
{
    private readonly ApplicationDbContext _context;

    public LessonsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Lessons.ToListAsync());
    }

    public IActionResult Create()
    {
        return View(new Lesson { Date = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Subject,Date")] Lesson lesson)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _context.Add(lesson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Произошла ошибка при сохранении: " + ex.Message);
        }

        return View(lesson);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var lesson = await _context.Lessons.FindAsync(id);
        if (lesson == null) return NotFound();
        return View(lesson);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Subject,Date")] Lesson lesson)
    {
        if (id != lesson.Id) return NotFound();
        if (ModelState.IsValid)
        {
            _context.Update(lesson);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(lesson);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var lesson = await _context.Lessons.FirstOrDefaultAsync(m => m.Id == id);
        if (lesson == null) return NotFound();
        return View(lesson);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var lesson = await _context.Lessons.FindAsync(id);
        _context.Lessons.Remove(lesson);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}