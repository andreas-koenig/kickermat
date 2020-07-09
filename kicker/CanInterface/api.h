#pragma once

#include <mutex>
#include <esd/ntcan.h>

#include "Motor/Motor.h"
#include "Motor/Faulhaber.h"
#include "Motor/Telemecanique.h"

#define CAN_API __declspec(dllexport)

using namespace Motor;

enum class CalibrationState {
    NotCalibrated = 0,
    Running = 1,
    Finished = 2,
};

struct ApiHandle {
    std::mutex* Mutex;

    Faulhaber* FaulhaberKeeper;
    Faulhaber* FaulhaberDefense;
    Faulhaber* FaulhaberMidfield;
    Faulhaber* FaulhaberStriker;
    Telemecanique* TelemecaniqueKeeper;
    Telemecanique* TelemecaniqueDefense;
    Telemecanique* TelemecaniqueMidfield;
    Telemecanique* TelemecaniqueStriker;

    CalibrationState CalibrationState;
};

extern "C" {
    CAN_API void* init();
    CAN_API void destroy(void* api);
    
    CAN_API void start_calibration(void* api, void __stdcall done_callback(void));
    CAN_API CalibrationState get_calibration_state(void* api);
    
    CAN_API void move_bar(void* api, uint8_t bar, uint8_t position, char angle, char rot_direction);
    CAN_API void shift_bar(void* api, uint8_t bar, uint8_t position);
    CAN_API void rotate_bar(void* api, uint8_t bar, char angle, char rot_direction);
}
