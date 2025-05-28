<?php

namespace Database\Seeders;

use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\File;
use App\Models\Car;

class CarSeeder extends Seeder
{
    /**
     * Run the database seeds.
     */
    public function run(): void
    {
        // Beolvassuk a database/seeders/cars.json fájlt
        $json = File::get(database_path('seeders/cars.json'));
        $cars = json_decode($json, true);

        foreach ($cars as $carData) {
            Car::create([
                'brand'         => $carData['brand'],
                'model'         => $carData['model'],
                'license_plate' => $carData['licensePlate'],
                'year'          => $carData['year'],
                'daily_price'   => $carData['dailyPrice'],
                'user_uid'      => $carData['userUID'] ?? '',  // ha nincs userUID a JSON-ben, üres string
            ]);
        }
    }
}
