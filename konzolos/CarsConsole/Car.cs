namespace CarsConsole.Models
{
    public class Car
    {
        public int CarId        { get; set; }
        public string Brand     { get; set; } = string.Empty;
        public string Model     { get; set; } = string.Empty;
        public decimal DailyPrice { get; set; }
    }
}