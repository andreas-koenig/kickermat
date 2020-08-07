#include "pch.h"
#include <thread>

#include "api.h"

#define KEEPER (uint8_t)0
#define DEFENSE (uint8_t)1
#define MIDFIELD (uint8_t)2
#define STRIKER (uint8_t)3

CAN_API void* init() {
    auto api = new ApiHandle{
        new std::mutex(),

        new Faulhaber(ID_FH_GOALKEEPER),
        new Faulhaber(ID_FH_DEFENSE),
        new Faulhaber(ID_FH_MIDFIELD),
        new Faulhaber(ID_FH_STRIKER),
        new Telemecanique(ID_TM_GOALKEEPER),
        new Telemecanique(ID_TM_DEFENSE),
        new Telemecanique(ID_TM_MIDFIELD),
        new Telemecanique(ID_TM_STRIKER),

        CalibrationState::NotCalibrated,
    };

    api->TelemecaniqueKeeper->EnableOperation();
    api->TelemecaniqueDefense->EnableOperation();
    api->TelemecaniqueMidfield->EnableOperation();
    api->TelemecaniqueStriker->EnableOperation();

    return (void*)api;
}

CAN_API void destroy(void* apiHandle) {
    auto api = (ApiHandle*)apiHandle;
    if (api == nullptr) {
        return;
    }

    delete api->Mutex;
    
    delete api->FaulhaberKeeper;
    delete api->FaulhaberDefense;
    delete api->FaulhaberMidfield;
    delete api->FaulhaberStriker;
    delete api->TelemecaniqueKeeper;
    delete api->TelemecaniqueDefense;
    delete api->TelemecaniqueMidfield;
    delete api->TelemecaniqueStriker;

    delete api;
}

CAN_API void start_calibration(void* apiHandle, void __stdcall done_callback(void)) {
    auto api = (ApiHandle*)apiHandle;
    if (api == nullptr) {
        return;
    }

    api->Mutex->lock();
    api->CalibrationState = CalibrationState::Running;
    api->Mutex->unlock();

    auto calibrate = [](BaseMotor* motor) { motor->Calibrate(); };

    std::thread threadTmKeeper(calibrate, api->TelemecaniqueKeeper);
    std::thread threadTmDefense(calibrate, api->TelemecaniqueDefense);
    std::thread threadTmMidfield(calibrate, api->TelemecaniqueMidfield);
    std::thread threadTmStriker(calibrate, api->TelemecaniqueStriker);

    threadTmKeeper.join();
    threadTmDefense.join();
    threadTmMidfield.join();
    threadTmStriker.join();

    std::thread threadFhKeeper(calibrate, api->FaulhaberKeeper);
    std::thread threadFhDefense(calibrate, api->FaulhaberDefense);
    std::thread threadFhMidfield(calibrate, api->FaulhaberMidfield);
    std::thread threadFhStriker(calibrate, api->FaulhaberStriker);

    threadFhKeeper.join();
    threadFhDefense.join();
    threadFhMidfield.join();
    threadFhStriker.join();

    api->Mutex->lock();
    api->CalibrationState = CalibrationState::Finished;
    api->Mutex->unlock();

    done_callback();
}

CAN_API CalibrationState get_calibration_state(void* apiHandle) {
    auto api = (ApiHandle*)apiHandle;
    if (api == nullptr) {
        return CalibrationState::NotCalibrated;
    }

    CalibrationState state;
    api->Mutex->lock();
    state = api->CalibrationState;
    api->Mutex->unlock();

    return state;
}

CAN_API void move_bar(void* apiHandle, uint8_t bar, uint8_t position, char angle, char rot_direction) {
    auto api = (ApiHandle*)apiHandle;
    if (api == nullptr) {
        return;
    }

    Faulhaber* faulhaber = nullptr;
    Telemecanique* telemecanique = nullptr;

    switch (bar) {
    case KEEPER:
        faulhaber = api->FaulhaberKeeper;
        telemecanique = api->TelemecaniqueKeeper;
        break;
    case DEFENSE:
        faulhaber = api->FaulhaberKeeper;
        telemecanique = api->TelemecaniqueKeeper;
        break;
    case MIDFIELD:
        faulhaber = api->FaulhaberKeeper;
        telemecanique = api->TelemecaniqueKeeper;
        break;
    case STRIKER:
        faulhaber = api->FaulhaberKeeper;
        telemecanique = api->TelemecaniqueKeeper;
        break;
    }

    if (telemecanique != nullptr && position >= 0) {
        telemecanique->MoveBar((uint32_t)position, PositioningMethod::Absolute);
    }

    if (faulhaber != nullptr && angle >= 0) {
        // TODO: Include direction
        faulhaber->RotateBar((int16_t)angle, PositioningMethod::Absolute);
    }
}

CAN_API void shift_bar(void* api, uint8_t bar, uint8_t position) {
    move_bar(api, bar, position, -1, 0);
}

CAN_API void rotate_bar(void* api, uint8_t bar, char angle, char rot_direction) {
    move_bar(api, bar, -1, angle, rot_direction);
}
