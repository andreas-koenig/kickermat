#pragma once

#include "Motor/Motor.h"
#include "Motor/Faulhaber.h"
#include "Motor/Telemecanique.h"

#define CAN_API __declspec(dllexport)

using namespace Motor;

struct ApiHandle {
    Faulhaber* FaulhaberKeeper;
    Faulhaber* FaulhaberDefense;
    Faulhaber* FaulhaberMidfield;
    Faulhaber* FaulhaberStriker;
    Telemecanique* TelemecaniqueKeeper;
    Telemecanique* TelemecaniqueDefense;
    Telemecanique* TelemecaniqueMidfield;
    Telemecanique* TelemecaniqueStriker;
};

extern "C" {
    CAN_API void* init();
    CAN_API void destroy(void* api);
    
    CAN_API void start_calibration(void* api, void __stdcall done_callback(void));
    
    CAN_API void move_bar(void* api, char bar, char position, char angle, char rot_direction);
    CAN_API void shift_bar(void* api, char bar, char position);
    CAN_API void rotate_bar(void* api, char bar, char angle, char rot_direction);
}
