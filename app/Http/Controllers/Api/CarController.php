<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Http\Requests\StoreCarRequest;
use App\Models\Car;
use Illuminate\Http\Response;

class CarController extends Controller
{
    public function index()
    {
        return Car::with('bookings')->get();
    }

    public function store(StoreCarRequest $request)
    {
        $car = Car::create($request->validated());
        return response($car, Response::HTTP_CREATED);
    }

    public function update(StoreCarRequest $request, Car $car)
    {
        $car->update($request->validated());
        return response($car, Response::HTTP_OK);
    }

    public function destroy(Car $car)
    {
        $car->delete();
        return response(null, Response::HTTP_NO_CONTENT);
    }
}