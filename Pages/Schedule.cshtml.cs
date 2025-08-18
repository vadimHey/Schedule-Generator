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
        [RegularExpression(@"^[A-Za-zА-Яа-я].*", ErrorMessage = "Событие должно начинаться с буквы")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Название должно быть от 3 до 50 символов")]
        public string NewItem { get; set; } = string.Empty;

        public List<string> Schedules { get; set; } = new List<string>();

        public void OnGet()
        {
            LoadSchedules();
        }

        public IActionResult OnPost()
        {
            LoadSchedules();

            if(!ModelState.IsValid)
            {
                return Page();
            }

            Schedules.Add(NewItem.Trim());
            SaveSchedules();

            ModelState.Clear();
            NewItem = string.Empty;

            return Page();
        }

        private void LoadSchedules()
        {
            var data = HttpContext.Session.GetString("Schedule");
            if(!string.IsNullOrEmpty(data))
            {
                Schedules = JsonSerializer.Deserialize<List<string>>(data) ?? new List<string>(); 
            }
        }

        private void SaveSchedules()
        {
            var data = JsonSerializer.Serialize(Schedules);
            HttpContext.Session.SetString("Schedule", data);
        }
    }
}
