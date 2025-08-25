using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScheduleGenerator.Data;
using ScheduleGenerator.Models;
using System.ComponentModel.DataAnnotations;

namespace ScheduleGenerator.Pages
{
    public class ScheduleModel : PageModel
    {
        private readonly AppDbContext _context;

        public ScheduleModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        public DateOnly SelectedDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [BindProperty]
        [Required(ErrorMessage = "Введите название события")]
        [RegularExpression(@"^\s*[A-Za-zА-Яа-я0-9].*", ErrorMessage = "Событие должно начинаться с буквы или цифры")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Название должно быть от 3 до 50 символов")]
        public string NewItem { get; set; } = string.Empty;

        [BindProperty]
        [DataType(DataType.Time)]
        public TimeOnly StartTime { get; set; }

        [BindProperty]
        [DataType(DataType.Time)]
        public TimeOnly EndTime { get; set; }

        public List<ScheduleItem> Schedule { get; set; } = new();

        public void OnGet()
        {
            LoadDay();
        }

        public IActionResult OnPost()
        {
            NewItem = NewItem?.Trim() ?? string.Empty;

            if (StartTime == default)
                ModelState.AddModelError(nameof(StartTime), "Введите время начала события");

            if (EndTime <= StartTime)
                ModelState.AddModelError(nameof(EndTime), "Конец события должен быть позже начала");

            var overlaps = _context.ScheduleItems.Any(x => 
                x.Date == SelectedDate && 
                x.StartTime < EndTime &&
                x.EndTime > StartTime);

            if (overlaps)
                ModelState.AddModelError(nameof(StartTime), "События пересекаются");

            if (!ModelState.IsValid)
            {
                LoadDay();
                return Page();
            }

            _context.ScheduleItems.Add(new ScheduleItem
            {
                Title = NewItem,
                Date = SelectedDate,
                StartTime = StartTime,
                EndTime = EndTime,
            });
            _context.SaveChanges();

            ModelState.Clear();
            NewItem = string.Empty;

            return RedirectToPage(new { SelectedDate = SelectedDate.ToString("yyyy-MM-dd") });
        }

        public IActionResult OnPostDeleteItem(int id)
        {
            var item = _context.ScheduleItems.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                var date = item.Date;
                _context.ScheduleItems.Remove(item);
                _context.SaveChanges();
                return RedirectToPage(new { SelectedDate = date.ToString("yyyy-MM-dd") });
            }

            return RedirectToPage(new { SelectedDate = SelectedDate.ToString("yyyy-MM-dd") });
        }

        public IActionResult OnPostClear()
        {
            var items = _context.ScheduleItems.Where(x => x.Date == SelectedDate);
            _context.ScheduleItems.RemoveRange(items);
            _context.SaveChanges();

            return RedirectToPage(new { SelectedDate = SelectedDate.ToString("yyyy-MM-dd") });
        }

        private void LoadDay()
        {
            Schedule = _context.ScheduleItems
                .Where(x => x.Date == SelectedDate)
                .OrderBy(x => x.StartTime)
                .ToList();
        }
    }
}
