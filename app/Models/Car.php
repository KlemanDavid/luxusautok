<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class Car extends Model
{
    protected $fillable = [
        'brand',
        'model',
        'license_plate',
        'year',
        'daily_price',
        'user_uid',
    ];

    // egy autóhoz sok foglalás tartozhat
    public function bookings()
    {
        return $this->hasMany(Booking::class);
    }
}
