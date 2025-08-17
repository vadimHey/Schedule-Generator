using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ScheduleGenerator.Pages
{
    public class ScheduleModel : PageModel
    {
        [BindProperty]
        public string NewItem { get; set; } = string.Empty;

        public List<string> Schedules { get; set; } = new List<string>();

        public void OnGet()
        {
            LoadSchedules();
        }
        public IActionResult OnPost()
        {
            LoadSchedules();

            if(!string.IsNullOrEmpty(NewItem))
            {
                Schedules.Add(NewItem.Trim());
                SaveSchedules();
            }

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
