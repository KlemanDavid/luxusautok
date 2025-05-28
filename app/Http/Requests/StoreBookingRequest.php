<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\Rule;

class StoreBookingRequest extends FormRequest
{
    public function rules(): array
    {
        return [
            'car_id'     => ['required','integer',Rule::exists('cars','id')],
            'start_date' => 'required|date|after_or_equal:today',
            'end_date'   => 'required|date|after:start_date',
        ];
    }

    public function withValidator($validator)
    {
        // extra logika: ne ültessen rá ütköző foglalásra
        $validator->after(function ($validator) {
            $data = $this->validated();
            $overlap = \App\Models\Booking::where('car_id', $data['car_id'])
                ->where(function($q) use($data) {
                    $q->whereBetween('start_date', [$data['start_date'], $data['end_date']])
                      ->orWhereBetween('end_date',   [$data['start_date'], $data['end_date']]);
                })
                ->exists();

            if ($overlap) {
                $validator->errors()->add('overlap', 'Ez az autó a választott időszakra már foglalt.');
            }
        });
    }
}