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
        public ScheduleItem Item { get; set; } = new();

        [BindProperty]
        public ScheduleItem EditItem { get; set; } = new();

        public List<ScheduleItem> Schedule { get; set; } = new();

        public void OnGet()
        {
            LoadDay();
        }

        public IActionResult OnPost()
        {
            Item.Date = SelectedDate;

            if (!ValidateEvent(Item, null, "Item"))
            {
                LoadDay();
                return Page();
            }

            _context.ScheduleItems.Add(Item);
            _context.SaveChanges();

            ModelState.Clear();
            Item.Title = string.Empty;

            return RedirectToPage(new { SelectedDate = SelectedDate.ToString("yyyy-MM-dd") });
        }

        public IActionResult OnPostEdit()
        {
            var newItem = _context.ScheduleItems.FirstOrDefault(x => x.Id == EditItem.Id);
            if (newItem == null)
                return RedirectToPage(new { SelectedDate = SelectedDate.ToString("yyyy-MM-dd") });

            if (!ValidateEvent(EditItem, EditItem.Id, "EditItem"))
            {
                LoadDay();
                ViewData["ShowEditModal"] = true;
                return Page();
            }

            newItem.Title = EditItem.Title;
            newItem.StartTime = EditItem.StartTime;
            newItem.EndTime = EditItem.EndTime;
            _context.SaveChanges();

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

        /// <summary>
        /// Общая функция валидации событий
        /// </summary>
        private bool ValidateEvent(ScheduleItem item, int? id, string prefix) 
        {
            if (string.IsNullOrWhiteSpace(item.Title))
            {
                ModelState.AddModelError($"{prefix}.Title", "Введите название события");
                return false;
            }

            item.Title = item.Title.Trim();

            if (!System.Text.RegularExpressions.Regex.IsMatch(item.Title, @"^[A-Za-zА-Яа-я0-9].*"))
            {
                ModelState.AddModelError($"{prefix}.Title", "Событие должно начинаться с буквы или цифры");
                return false;
            }

            if (item.Title.Length < 3 || item.Title.Length > 50)
            {
                ModelState.AddModelError($"{prefix}.Title", "Название должно быть от 3 до 50 символов");
                return false;
            }

            if (item.StartTime == default)
            {
                ModelState.AddModelError($"{prefix}.StartTime", "Введите время начала события");
                return false;
            }

            if (item.EndTime <= item.StartTime)
            {
                ModelState.AddModelError($"{prefix}.EndTime", "Конец события должен быть позже начала");
                return false;
            }

            var overlaps = _context.ScheduleItems.Any(x =>
                x.Id != id &&
                x.Date == item.Date &&
                x.StartTime < item.EndTime &&
                x.EndTime > item.StartTime);

            if (overlaps)
            {
                ModelState.AddModelError($"{prefix}.StartTime", "События пересекаются");
                return false;
            }

            return true;
        }
    }
}
