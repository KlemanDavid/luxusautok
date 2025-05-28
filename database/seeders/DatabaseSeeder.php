<?php

namespace Database\Seeders;

use Illuminate\Database\Seeder;

class DatabaseSeeder extends Seeder
{
    public function run(): void
    {
        $this->call([
            CarSeeder::class,
            // ha majd Booking-seedert is írsz, ide veheted fel
        ]);
    }
}