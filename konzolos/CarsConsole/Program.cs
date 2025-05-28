using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CarsConsole.Models;

namespace CarsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars     = LoadCars("cars.csv");
            var bookings = LoadBookings("bookings.csv", cars);

            if (!cars.Any())
            {
                Console.WriteLine("Nincs autó adat (cars.csv).");
                return;
            }
            if (!bookings.Any())
            {
                Console.WriteLine("Nincs foglalás adat (bookings.csv).");
                return;
            }

            // 1) Autók napi díj szerint csökkenő sorrendben
            Console.WriteLine("1) Autók napi díj szerint csökkenő sorrendben:");
            foreach (var c in cars.OrderByDescending(c => c.DailyPrice))
                Console.WriteLine($"   {c.Brand} {c.Model} – {c.DailyPrice:N0} Ft");

            // 2) Foglalások listája (autó + teljes díj)
            Console.WriteLine("\n2) Foglalások listája (autó + teljes díj):");
            foreach (var b in bookings)
                Console.WriteLine($"   [{b.BookingId}] {b.Brand} {b.Model} – {b.TotalPrice:N0} Ft");

            // 3) Legtöbbször foglalt autó
            var mostBookedGroup = bookings
                .GroupBy(b => b.CarId)
                .OrderByDescending(g => g.Count())
                .First();
            var mostBookedCar = cars.First(c => c.CarId == mostBookedGroup.Key);
            Console.WriteLine($"\n3) Legtöbbször foglalt autó: {mostBookedCar.Brand} {mostBookedCar.Model} ({mostBookedGroup.Count()} alkalom)");

            // 4) Legtöbb bevételt hozó autó
            var topRevenueGroup = bookings
                .GroupBy(b => b.CarId)
                .Select(g => new { CarId = g.Key, Revenue = g.Sum(b => b.TotalPrice) })
                .OrderByDescending(x => x.Revenue)
                .First();
            var topRevenueCar = cars.First(c => c.CarId == topRevenueGroup.CarId);
            Console.WriteLine($"\n4) Legtöbb bevételt hozó autó: {topRevenueCar.Brand} {topRevenueCar.Model} ({topRevenueGroup.Revenue:N0} Ft)");

            // 5) Átlagos bérleti időtartam (napban)
            double avgDays = bookings.Average(b => b.Days);
            Console.WriteLine($"5) Átlagos bérlési időtartam: {avgDays:F1} nap");

            // 6) Legutóbb foglalt autó adatai
            var lastBooking = bookings.OrderByDescending(b => b.StartDate).First();
            Console.WriteLine($"6) Legutóbb foglalt autó: {lastBooking.Brand} {lastBooking.Model}, kezdés: {lastBooking.StartDate:yyyy-MM-dd}");

            // 7) Összes bevétel
            decimal totalRevenue = bookings.Sum(b => b.TotalPrice);
            Console.WriteLine($"7) Összes bevétel: {totalRevenue:N0} Ft");

            // 8) Foglalások havi bontásban → foglalasok.csv
            var monthlyGroups = bookings
                .GroupBy(b => b.StartDate.Month)
                .OrderBy(g => g.Key);
            using (var w = new StreamWriter("foglalasok.csv"))
            {
                w.WriteLine("Month;BookingId;CarId;Brand;Model;Start;End;Days;TotalPrice");
                foreach (var g in monthlyGroups)
                    foreach (var b in g)
                        w.WriteLine($"{g.Key};{b.BookingId};{b.CarId};{b.Brand};{b.Model};" +
                                    $"{b.StartDate:yyyy-MM-dd};{b.EndDate:yyyy-MM-dd};{b.Days};{b.TotalPrice:N0}");
            }
            Console.WriteLine("8) foglalasok.csv elkészült.");

            // 9) Bevétel autónként → bevetes.csv
            var revenuePerCar = bookings
                .GroupBy(b => b.CarId)
                .Select(g => new { CarId = g.Key, Revenue = g.Sum(b => b.TotalPrice) });
            using (var w = new StreamWriter("bevetes.csv"))
            {
                w.WriteLine("CarId;Brand;Model;Revenue");
                foreach (var x in revenuePerCar)
                {
                    var c = cars.First(ca => ca.CarId == x.CarId);
                    w.WriteLine($"{c.CarId};{c.Brand};{c.Model};{x.Revenue:N0}");
                }
            }
            Console.WriteLine("9) bevetes.csv elkészült.");

            // 10) Nem használt autók → nemhasznalt.csv
            var unusedCars = cars.Where(c => bookings.All(b => b.CarId != c.CarId));
            using (var w = new StreamWriter("nemhasznalt.csv"))
            {
                w.WriteLine("CarId;Brand;Model;DailyPrice");
                foreach (var c in unusedCars)
                    w.WriteLine($"{c.CarId};{c.Brand};{c.Model};{c.DailyPrice:N0}");
            }
            Console.WriteLine("10) nemhasznalt.csv elkészült.");

            // 11) Átlag napi díj márkánként → berkat.csv
            var avgPricePerBrand = cars
                .GroupBy(c => c.Brand)
                .Select(g => new { Brand = g.Key, AvgPrice = g.Average(c => c.DailyPrice) });
            using (var w = new StreamWriter("berkat.csv"))
            {
                w.WriteLine("Brand;AvgDailyPrice");
                foreach (var x in avgPricePerBrand)
                    w.WriteLine($"{x.Brand};{x.AvgPrice:F2}");
            }
            Console.WriteLine("11) berkat.csv elkészült.");
        }

        static List<Car> LoadCars(string path)
        {
            var lines = File.ReadAllLines(path)
                            .Where(l => !string.IsNullOrWhiteSpace(l))
                            .ToArray();
            if (lines.Length < 2) return new List<Car>();

            char delim = lines[0].Contains(';') ? ';' : ',';
            return lines.Skip(1)
                        .Select(line =>
                        {
                            var p = line.Split(delim);
                            if (p.Length < 6)
                                throw new InvalidDataException($"Rossz sor (kevesebb mint 6 mező): {line}");
                            return new Car
                            {
                                CarId      = int.Parse(p[0], CultureInfo.InvariantCulture),
                                Brand      = p[1],
                                Model      = p[2],
                                DailyPrice = decimal.Parse(p[5], CultureInfo.InvariantCulture)
                            };
                        })
                        .ToList();
        }

        static List<Booking> LoadBookings(string path, List<Car> cars)
        {
            var lines = File.ReadAllLines(path)
                            .Where(l => !string.IsNullOrWhiteSpace(l))
                            .ToArray();
            if (lines.Length < 2) return new List<Booking>();

            char delim = lines[0].Contains(';') ? ';' : ',';
            return lines.Skip(1)
                        .Select(line =>
                        {
                            var p = line.Split(delim);
                            if (p.Length < 4)
                                throw new InvalidDataException($"Rossz foglalás sor (kevesebb mint 4 mező): {line}");
                            var b = new Booking
                            {
                                BookingId = int.Parse(p[0], CultureInfo.InvariantCulture),
                                CarId     = int.Parse(p[1], CultureInfo.InvariantCulture),
                                StartDate = DateTime.ParseExact(p[2], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                                EndDate   = DateTime.ParseExact(p[3], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                            };
                            var car = cars.FirstOrDefault(c => c.CarId == b.CarId);
                            if (car != null)
                            {
                                b.Brand      = car.Brand;
                                b.Model      = car.Model;
                                b.DailyPrice = car.DailyPrice;
                            }
                            return b;
                        })
                        .ToList();
        }
    }
}