using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScheduleGenerator.Data;
using ScheduleGenerator.Models;
using ScheduleGenerator.Service;
using System.ComponentModel.DataAnnotations;

namespace ScheduleGenerator.Pages
{
    public class ScheduleModel : PageModel
    {
        private readonly AppDbContext _context;

        private readonly ScheduleService _scheduleService;

        [BindProperty]
        public ScheduleItem Item { get; set; } = new();

        [BindProperty]
        public ScheduleItem EditItem { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        public DateOnly SelectedDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        public List<ScheduleItem> DayItems { get; set; } = new();

        public ScheduleModel(AppDbContext context, ScheduleService scheduleService)
        {
            _context = context;
            _scheduleService = scheduleService;
        }

        public void OnGet()
        {
            DayItems = _scheduleService.GetEventsByDate(SelectedDate);
        }

        public IActionResult OnPost()
        {
            Item.Date = SelectedDate;

            if (!_scheduleService.ValidateEvent(ModelState, Item, null, "Item"))
            {
                DayItems = _scheduleService.GetEventsByDate(SelectedDate);
                return Page();
            }

            _scheduleService.AddEvent(Item);
            return RedirectToPage(new { SelectedDate = SelectedDate.ToString("yyyy-MM-dd") });
        }

        public IActionResult OnPostEdit()
        {
            EditItem.Date = SelectedDate;

            if (!_scheduleService.ValidateEvent(ModelState, EditItem, EditItem.Id, "EditItem"))
            {
                DayItems = _scheduleService.GetEventsByDate(SelectedDate);
                ViewData["ShowEditModalId"] = EditItem.Id;
                return Page();
            }

            if (!_scheduleService.EditEvent(EditItem))
                return NotFound();

            return RedirectToPage(new { SelectedDate = SelectedDate.ToString("yyyy-MM-dd") });
        }

        public IActionResult OnPostDeleteItem(int id)
        {
            if (!_scheduleService.DeleteEvent(id))
                return NotFound();

            return RedirectToPage(new { SelectedDate = SelectedDate.ToString("yyyy-MM-dd") });
        }
    }
}
