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
        public List<string> Schedules { get; set; } = new List<string>();

        public void OnGet()
        {
            Schedules = LoadSchedules();
        }

        public IActionResult OnPost()
        {
            NewItem = NewItem.Trim();  

            if (!ModelState.IsValid)
            {
                Schedules = LoadSchedules();
                return Page();
            }

            Schedules = LoadSchedules();
            Schedules.Add(NewItem);
            SaveSchedules(Schedules);

            ModelState.Clear();
            NewItem = string.Empty;

            return RedirectToPage();
        }

        private List<string> LoadSchedules()
        {
            var data = HttpContext.Session.GetString("Schedule");

            return string.IsNullOrEmpty(data) 
                ? new List<string>() 
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(data) ?? new List<string>();
        }

        private void SaveSchedules(List<string> schedule)
        {
            var data = System.Text.Json.JsonSerializer.Serialize(schedule);
            HttpContext.Session.SetString("Schedule", data);
        }
    }
}
