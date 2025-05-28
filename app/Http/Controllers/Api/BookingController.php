<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Http\Requests\StoreBookingRequest;
use App\Models\Booking;
use Illuminate\Http\Response;

class BookingController extends Controller
{
    public function index()
    {
        return Booking::with('car')->get();
    }

    public function store(StoreBookingRequest $request)
    {
        $booking = Booking::create($request->validated());
        return response($booking, Response::HTTP_CREATED);
    }

    public function destroy(Booking $booking)
    {
        $booking->delete();
        return response(null, Response::HTTP_NO_CONTENT);
    }
}