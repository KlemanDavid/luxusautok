<?php

use App\Http\Controllers\Api\CarController;
use App\Http\Controllers\Api\BookingController;
use Illuminate\Support\Facades\Route;

Route::apiResource('cars', CarController::class)
     ->only(['index','store','update','destroy']);

Route::apiResource('bookings', BookingController::class)
     ->only(['index','store','destroy']);