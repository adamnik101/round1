using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RareCrew.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RareCrew.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/<EmployeeController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            HttpClient client = new HttpClient();

            string apiUrl = _configuration["apiUrl"];
            client.BaseAddress = new Uri(apiUrl);

            HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
            if (response.IsSuccessStatusCode)
            {
                // Parse the response content and return it
                string responseContent = await response.Content.ReadAsStringAsync();

                ICollection<Models.Employee> responseJSON = new List<Models.Employee>();
                ICollection<EmployeeUI> employeesTotal = new List<EmployeeUI>();

                try
                {
                    // Deserialize 
                    responseJSON = JsonConvert.DeserializeObject<ICollection<Models.Employee>>(responseContent);
                }
                catch (JsonReaderException ex)
                {
                    return StatusCode(500);
                }

                // 
                try 
                {
                    var employees = responseJSON.Where(y => y.EmployeeName != null && y.DeletedOn == null).GroupBy(x => x.EmployeeName);

                    // Calculate total time worked and send it to client in a format {name: string, totalHours: number}
                    foreach (var employee in employees)
                    {
                        double durationInHours = employee.Sum(x => (x.EndTimeUTC - x.StarTimeUTC).TotalHours);
                        TimeSpan duration = TimeSpan.FromHours(durationInHours);
                        employeesTotal.Add(new EmployeeUI
                        {
                            EmployeeName = employee.Select(x => x.EmployeeName).First(),
                            TotalHours = string.Format("{0:0} hours {1:0} minutes", Math.Floor(duration.TotalHours), duration.Minutes),
                            TotalHoursOriginal = durationInHours
                        });
                    }
                    employeesTotal = employeesTotal.OrderByDescending(x => x.TotalHoursOriginal).ToList();
                }
                catch (OverflowException ex)
                {
                    return StatusCode(500);
                }
                catch (ArgumentNullException ex) 
                {
                    return StatusCode(500);
                }
                return Ok(employeesTotal);
            }
            else
            {
                // Handle the error and return an appropriate response
                return BadRequest("API call failed.");
            }
        }
    }
}
