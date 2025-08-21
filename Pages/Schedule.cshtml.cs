using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScheduleGenerator.Data;
using ScheduleGenerator.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ScheduleGenerator.Pages
{
    public class ScheduleModel : PageModel
    {
        private readonly AppDbContext _context;

        public ScheduleModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        [Required(ErrorMessage = "Введите название события")]
        [RegularExpression(@"^\s*[A-Za-zА-Яа-я0-9].*", ErrorMessage = "Событие должно начинаться с буквы или цифры")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Название должно быть от 3 до 50 символов")]
        public string NewItem { get; set; } = string.Empty;
        public List<ScheduleItem> Schedule { get; set; } = new();

        public void OnGet()
        {
            Schedule = _context.ScheduleItems.ToList();
        }

        public IActionResult OnPost()
        {
            NewItem = NewItem?.Trim() ?? string.Empty;  

            if (!ModelState.IsValid)
            {
                Schedule = _context.ScheduleItems.ToList();
                return Page();
            }

            var item = new ScheduleItem { Title = NewItem };
            _context.ScheduleItems.Add(item);
            _context.SaveChanges();

            ModelState.Clear();
            NewItem = string.Empty;

            return RedirectToPage();
        }

        public IActionResult OnPostDeleteItem(int id)
        {
            var item = _context.ScheduleItems.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            _context.ScheduleItems.Remove(item);
            _context.SaveChanges();

            return RedirectToPage();
        }

        public IActionResult OnPostClear()
        {
            _context.ScheduleItems.RemoveRange(_context.ScheduleItems);
            _context.SaveChanges();

            return RedirectToPage();
        }

        private List<string> LoadSchedule()
        {
            var data = HttpContext.Session.GetString("Schedule");

            return string.IsNullOrEmpty(data) 
                ? new List<string>() 
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(data) ?? new List<string>();
        }

        private void SaveSchedule(List<string> schedule)
        {
            var data = System.Text.Json.JsonSerializer.Serialize(schedule);
            HttpContext.Session.SetString("Schedule", data);
        }
    }
}
