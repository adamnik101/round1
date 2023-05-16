

using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Diagnostics;

namespace PieChartApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string url = "http://localhost:5078/api/Employee";

            try
            {
                var employees = await FetchData(url);
                double totalWorkHours = employees.Sum(x => x.TotalHoursOriginal);
                var chart = new Chart();
                chart.Series.Add(new Series());
                chart.ChartAreas.Add(new ChartArea());
                chart.Legends.Add(new Legend());
                chart.Legends[0].Font = new Font(chart.Legends[0].Font.FontFamily, 15);
                chart.Titles.Add(new Title("Employee Work Time Percentage"));
                chart.Titles[0].Font = new Font(chart.Titles[0].Font.FontFamily, 15);
                chart.Width = 1920;
                chart.Height = 1080;
                
                foreach (var employee in employees)
                {
                    double percentage = (employee.TotalHoursOriginal / totalWorkHours) * 100;
                    chart.Series[0].Points.AddXY(employee.EmployeeName,percentage);
                }
                chart.Series[0].ChartType = SeriesChartType.Pie;
                chart.Series[0].IsValueShownAsLabel = true;
                chart.Series[0].LabelFormat = "{0}%";
                foreach (DataPoint dataPoint in chart.Series[0].Points)
                {
                    dataPoint.Font = new Font(dataPoint.Font.FontFamily, 15);
                }
                string currentDir = Directory.GetCurrentDirectory();
                string imagePath = Path.Combine(currentDir, "pie-chart.png");
                chart.SaveImage(imagePath, ChartImageFormat.Png);
                Process.Start(imagePath);
                Console.WriteLine($"Pie chart saved at {imagePath}");
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured.");
            }
        }

        static async Task<ICollection<Employee>> FetchData(string url)
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(url);
            HttpResponseMessage response = await client.GetAsync(url);
            ICollection<Employee> responseJSON = new List<Employee>();
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                responseJSON = JsonConvert.DeserializeObject<List<Employee>>(content);

            }
            return responseJSON;
        }
        public class Employee
        {
            public string EmployeeName { get; set; }
            public double TotalHoursOriginal { get; set; }
            public string TotalHours { get; set; }
        }
    }
}