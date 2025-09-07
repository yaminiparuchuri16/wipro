using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class Vehicles
    {
        [Key]
        public int VehiclesId { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public decimal DailyRate { get; set; }
        public string? Status { get; set; }
        public int PassengerCapacity { get; set; }
        public string? EngineCapacity { get; set; }
    }
}