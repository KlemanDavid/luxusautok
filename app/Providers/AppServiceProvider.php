<?php

namespace App\Providers;

use Illuminate\Support\ServiceProvider;
use Illuminate\Support\Facades\Route;

class AppServiceProvider extends ServiceProvider
{
    public function register(): void
    {
        //
    }

    public function boot(): void
    {
        // API-route-ok betöltése az /api prefix alatt
        Route::prefix('api')
             ->middleware('api')
             ->group(base_path('routes/api.php'));

        // (Ha kell) web-route-ok betöltése
        Route::middleware('web')
             ->group(base_path('routes/web.php'));
    }
}