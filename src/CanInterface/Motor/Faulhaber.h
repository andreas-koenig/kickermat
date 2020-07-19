#pragma once

#include "Motor.h"

// Umrechnungsfaktor für die Skalierung der Faulhaber Motoren in Winkel
#define FH_NUMBER_OF_STEPS 41375

namespace Motor {
    class Faulhaber : public Motor::BaseMotor {
    public:
        Faulhaber(uint8_t motorId);

        // Write PDOs
        void RotateBar(int16_t angle, PositioningMethod positioningMethod);

        // Write SDOs
        void SetScaleNumerator(uint16_t numerator);
        void SetScaleFeedConstant(uint16_t constant);
        void SetHomingOffset(uint16_t angle);

        // Read SDOs
        uint32_t GetScaleNumerator();
        uint32_t GetScaleFeedConstant();

        void Calibrate();
    };
}
