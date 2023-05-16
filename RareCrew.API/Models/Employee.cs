namespace RareCrew.API.Models
{
    public class Employee
    {
        public required string Id { get; set; }
        public required string EmployeeName { get; set; }
        public DateTime StarTimeUTC { get; set; }
        public DateTime EndTimeUTC { get; set; }
        public required string EntryNotes { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
