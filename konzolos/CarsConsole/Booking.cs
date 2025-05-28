using System;

namespace CarsConsole.Models
{
    public class Booking
    {
        public int BookingId   { get; set; }
        public int CarId       { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate   { get; set; }

        public int Days => (EndDate - StartDate).Days + 1;
        public decimal TotalPrice => Days * DailyPrice;

        // Ezt a DailyPrice-t és a Brand/Model-t a LoadBookings állítja be
        public decimal DailyPrice { get; set; }

        // Új mezők:
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
    }
}
