using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ScheduleGenerator.Pages
{
    public class ScheduleModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Введите название события")]
        [RegularExpression(@"^\s*[A-Za-zА-Яа-я0-9].*", ErrorMessage = "Событие должно начинаться с буквы или цифры")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Название должно быть от 3 до 50 символов")]
        public string NewItem { get; set; } = string.Empty;
        public List<string> Schedule { get; set; } = new List<string>();

        public void OnGet()
        {
            Schedule = LoadSchedule();
        }

        public IActionResult OnPost()
        {
            NewItem = NewItem?.Trim() ?? string.Empty;  

            if (!ModelState.IsValid)
            {
                Schedule = LoadSchedule();
                return Page();
            }

            Schedule = LoadSchedule();
            Schedule.Add(NewItem);
            SaveSchedule(Schedule);

            ModelState.Clear();
            NewItem = string.Empty;

            return RedirectToPage();
        }

        public IActionResult OnPostDeleteItem(int index)
        {
            Schedule = LoadSchedule();
            if (index >= 0 && index < Schedule.Count)
            {
                Schedule.RemoveAt(index);
                SaveSchedule(Schedule);
            }

            return RedirectToPage();
        }

        public IActionResult OnPostClear()
        {
            Schedule.Clear();
            HttpContext.Session.Remove("Schedule");
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
