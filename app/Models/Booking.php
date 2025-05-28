<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Carbon\Carbon;

class Booking extends Model
{
    protected $fillable = [
        'car_id',
        'start_date',
        'end_date',
    ];

    // automatikusan csatoljuk a számított mezőt
    protected $appends = ['total_price'];

    public function car()
    {
        return $this->belongsTo(Car::class);
    }

    // napi díj * napok száma
    public function getTotalPriceAttribute()
    {
        $start = Carbon::parse($this->start_date);
        $end   = Carbon::parse($this->end_date);
        $days  = $start->diffInDays($end);
        return $days * $this->car->daily_price;
    }
}