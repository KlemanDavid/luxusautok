<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

class CreateCarsTable extends Migration
{
    /**
     * Run the migrations.
     */
    public function up()
    {
        Schema::create('cars', function (Blueprint $table) {
            $table->id();

            // Autó jellemzői
            $table->string('brand', 50);
            $table->string('model', 50);
            $table->string('license_plate')->unique();
            $table->integer('year');
            $table->unsignedInteger('daily_price');

            // Tulajdonos / kezelő azonosítója
            $table->string('user_uid');

            $table->timestamps();
        });
    }

    /**
     * Reverse the migrations.
     */
    public function down()
    {
        Schema::dropIfExists('cars');
    }
}
