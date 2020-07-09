#pragma once

#include "Motor.h"

// Defaultwert des Nenners für die Skalierung des Telemecanique Motors
#define TM_SCALE_DENOM 16384

// Time in milliseconds before reading out the motor load.
// Modify only if you know what you do, wrong values could lead to damage during calibration.
#define TELEMECANIQUE_MOTOR_LOAD_DELAY_TIME 2

// The limit for the motor load during calibration.
// Modify only if you know what you do, wrong values could lead to damage during calibration.
#define TELEMECANIQUE_LOAD_LIMIT 2

// The number of cycles a motor's load must be below the load threshold to continue calibration.
#define TELEMECANIQUE_REQUIRED_CYCLES_FOR_MOTOR_LOAD 10

// The amount of position the motor is moved during one calibration step.
// Modify only if you know what you do, wrong values could lead to damage during calibration.
#define TELEMECANIQUE_STEP_WIDTH 40

// The limit for the motor load during calibration.
// Modify only if you know what you do, wrong values could lead to damage during calibration.
#define TELEMECANIQUE_LOAD_LIMIT 2

// The distance to move the motor back after reaching minimum position during calibration.
// Used in case that start position is nearly equal to minimum position and motor load
// does not decrease if the motor is moved back only a few steps.
// The value is calculated from the available position range of the midfield motor because
// it has the shortest usable way for moving from one end to the other.
// (In uncalibrated state, the motor can move about 25000 units, this value is the half)
#define TELEMECANIQUE_GO_BACK_DISTANCE 12500

namespace Motor {
    enum PositionLimitState {
        Disabled = 0x0000,
        MaximumEnabled = 0x0001,
        MinimumEnabled = 0x0002,
        BothEnabled = 0x0003,
    };

    enum HomingMethod {
        SetDimensions = 0x23, // not exhaustive
    };

    class Telemecanique : public BaseMotor {
    public:
        Telemecanique(uint8_t motorId);

        // Set position
        void MoveBar(uint32_t position, PositioningMethod positioningMethod);

        // Write SDOs
        void SetPositionLimitState(PositionLimitState limit);
        void SetScale(uint32_t numerator, uint32_t denominator);
        void SetAcceleration(uint32_t acceleration);
        void SetDeceleration(uint32_t deceleration);
        void SetMaxVelocity(uint32_t velocity);
        void SetVelocity(uint32_t velocity);
        void SetPositionForDimensionSetting(int32_t position);
        void SetHomingMethod(HomingMethod method);
        void SetMinPosLimit(int32_t position);
        void SetMaxPosLimit(int32_t position);


        // Read SDOs
        PositionLimitState GetPositionLimitState();
        int32_t GetPositionAtPositionInterface();
        uint16_t GetLoad();
        HomingMethod GetHomingMethod();

        void Calibrate();

    protected:
        uint32_t MaxPosition;
        uint32_t MinPosition;
    };
}