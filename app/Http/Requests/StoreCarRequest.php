<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class StoreCarRequest extends FormRequest
{
    public function rules(): array
    {
        return [
            'brand'         => 'required|string|min:1|max:50',
            'model'         => 'required|string|min:1|max:50',
            'license_plate' => ['required','string','regex:/^[A-Z]{3}-\d{3}$/','unique:cars'],
            'year'          => 'required|integer|between:2000,2025',
            'daily_price'   => 'required|integer|between:100,2000',
            'user_uid'      => 'required|string|min:1',
        ];
    }
}