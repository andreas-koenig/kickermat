#include "Telemecanique.h"
#include "../CanOpen/CanOpen.h"

namespace Motor {
    Telemecanique::Telemecanique(uint8_t motorId) : BaseMotor(motorId) {}

    void Telemecanique::MoveBar(uint32_t position, PositioningMethod positioningMethod) {
        /*
        if (position > MaxPosition) {
            position = MaxPosition;
        }
        else if (position < MinPosition) {
            position = MinPosition;
        }
        */

        uint16_t controlWord = DCOM_CONTROL_WORD_SWITCH_ON
            | DCOM_CONTROL_WORD_QUICK_STOP
            | DCOM_CONTROL_WORD_ENABLE_VOLTAGE
            | DCOM_CONTROL_WORD_ENABLE_OPERATION;

        switch (positioningMethod)
        {
        case PositioningMethod::Absolute:
            controlWord &= ~(DCOM_CONTROL_WORD_POSITIONING_RELATIVE);
            break;
        case PositioningMethod::Relative:
            controlWord |= DCOM_CONTROL_WORD_POSITIONING_RELATIVE;
            break;
        }

        int32_t numMessages = 1;

        auto msg = CanOpen::CreatePdoMessage(MotorId, controlWord, 0, CanOpen::FunctionCode::R_PDO1);
        controlWord |= DCOM_CONTROL_WORD_NEW_SETPOINT | DCOM_CONTROL_WORD_CHANGE_SET_IMMEDIATLY;
        auto msg2 = CanOpen::CreatePdoMessage(MotorId, controlWord, position, CanOpen::FunctionCode::R_PDO2);

        canSend(CanHandle, &msg, &numMessages);
        canSend(CanHandle, &msg2, &numMessages);
    }

    void Telemecanique::SetPositionLimitState(PositionLimitState limit) {
        CMSG msg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::PositionLimitState),
            OBJECT_SUB_INDEX(CanOpen::PositionLimitState),
            limit,
            2);

        CMSG response;
        RequestSdo(&msg, &response);
    }

    PositionLimitState Telemecanique::GetPositionLimitState() {
        CMSG msg = CanOpen::CreateSdoReadRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::PositionLimitState),
            OBJECT_SUB_INDEX(CanOpen::PositionLimitState));

        CMSG response;
        RequestSdo(&msg, &response);

        uint32_t data;
        CanOpen::ExtractDataFromMessage(&response, &data, MotorId);

        return (PositionLimitState)data;
    }

    void Telemecanique::SetScale(uint32_t numerator, uint32_t denominator) {
        CMSG denominatorMsg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::TelemecaniquePositionScaleDenominator),
            OBJECT_SUB_INDEX(CanOpen::TelemecaniquePositionScaleDenominator),
            denominator,
            4);

        CMSG numeratorMsg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::TelemecaniquePositionScaleNumerator),
            OBJECT_SUB_INDEX(CanOpen::TelemecaniquePositionScaleNumerator),
            numerator,
            4);

        CMSG denominatorResponse, numeratorResponse;
        RequestSdo(&denominatorMsg, &denominatorResponse);
        RequestSdo(&numeratorMsg, &numeratorResponse);
    }

    void Telemecanique::SetAcceleration(uint32_t acceleration) {
        CMSG msg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::ProfileAcceleration),
            OBJECT_SUB_INDEX(CanOpen::ProfileAcceleration),
            acceleration,
            4);

        CMSG response;
        RequestSdo(&msg, &response);
    }

    void Telemecanique::SetDeceleration(uint32_t deceleration) {
        CMSG msg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::ProfileDeceleration),
            OBJECT_SUB_INDEX(CanOpen::ProfileDeceleration),
            deceleration,
            4);

        CMSG response;
        RequestSdo(&msg, &response);
    }

    void Telemecanique::SetMaxVelocity(uint32_t velocity) {
        CMSG msg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::MaxProfileVelocity),
            OBJECT_SUB_INDEX(CanOpen::MaxProfileVelocity),
            velocity,
            4);

        CMSG response;
        RequestSdo(&msg, &response);
    }

    void Telemecanique::SetVelocity(uint32_t velocity) {
        CMSG msg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::ProfileVelocity),
            OBJECT_SUB_INDEX(CanOpen::ProfileVelocity),
            velocity,
            4);

        CMSG response;
        RequestSdo(&msg, &response);
    }

    int32_t Telemecanique::GetPositionAtPositionInterface() {
        CMSG msg = CanOpen::CreateSdoReadRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::PositionAtPositionInterface),
            OBJECT_SUB_INDEX(CanOpen::PositionAtPositionInterface));

        CMSG response;
        RequestSdo(&msg, &response);

        uint32_t data;
        CanOpen::ExtractDataFromMessage(&response, &data, MotorId);

        return (int32_t)data;
    }

    uint16_t Telemecanique::GetLoad() {
        CMSG msg = CanOpen::CreateSdoReadRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::TelemecaniqueMotorLoad),
            OBJECT_SUB_INDEX(CanOpen::TelemecaniqueMotorLoad));

        CMSG response;
        RequestSdo(&msg, &response);

        uint32_t data;
        CanOpen::ExtractDataFromMessage(&response, &data, MotorId);

        return (uint16_t)data;
    }

    void Telemecanique::SetPositionForDimensionSetting(int32_t position) {
        CMSG msg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::TelemecaniquePositionForDimensionSetting),
            OBJECT_SUB_INDEX(CanOpen::TelemecaniquePositionForDimensionSetting),
            position,
            4);

        CMSG response;
        RequestSdo(&msg, &response);
    }

    void Telemecanique::SetHomingMethod(HomingMethod method) {
        auto data = (uint32_t)method;

        CMSG msg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::TelemecaniqueHomingMethod),
            OBJECT_SUB_INDEX(CanOpen::TelemecaniqueHomingMethod),
            data,
            1);

        CMSG response;
        RequestSdo(&msg, &response);
    }

    HomingMethod Telemecanique::GetHomingMethod() {
        CMSG msg = CanOpen::CreateSdoReadRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::TelemecaniqueHomingMethod),
            OBJECT_SUB_INDEX(CanOpen::TelemecaniqueHomingMethod));

        CMSG response;
        RequestSdo(&msg, &response);

        uint32_t homingMethod;
        CanOpen::ExtractDataFromMessage(&response, &homingMethod, MotorId);

        return (HomingMethod)homingMethod;
    }

    void Telemecanique::SetMinPosLimit(int32_t position) {
        CMSG msg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::MinPositionLimit),
            OBJECT_SUB_INDEX(CanOpen::MinPositionLimit),
            position,
            4);

        CMSG response;
        RequestSdo(&msg, &response);
    }

    void Telemecanique::SetMaxPosLimit(int32_t position) {
        CMSG msg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::MaxPositionLimit),
            OBJECT_SUB_INDEX(CanOpen::MaxPositionLimit),
            position,
            4);

        CMSG response;
        RequestSdo(&msg, &response);
    }

    void Telemecanique::Calibrate() {
        // Betriebsmodus Referenzierung aktiveren, nur in diesem Modus ist Maßsetzen möglich
        SetOperationMode(OperationMode::Referencing);
        while (GetOperationMode() != OperationMode::Referencing);

        SetRpdo2State(Rpdo2State::Rpdo2Enabled);
        while (GetRpdo2State() != Rpdo2State::Rpdo2Enabled);

        // Spannung abschalten, damit sich der Motor in einer entspannten Position befindet
        DisableVoltage();
        while (GetOperationState() != OperationState::Ready);

        // Disable minimum and maximum position limit because limits from previous calibration could lead to
        // position limit errors during calibration
        SetPositionLimitState(PositionLimitState::Disabled);
        while (GetPositionLimitState() != PositionLimitState::Disabled);

        // Skalierung wieder auf Defaultwert zurücksetzen
        SetScale(1, TM_SCALE_DENOM);

        // Set speed depended parameters
        // Todo: Check why only 10000, maximum is 3000000
        SetAcceleration(10000);
        SetDeceleration(10000);
        SetMaxVelocity(13200);
        SetVelocity(8000);

        // Wenn Motor entspannt ist, Spannung wieder einschalten
        EnableOperation();
        while (GetOperationState() != OperationState::OperationEnabled);

        // Startposition sichern, zu dieser Position wird nach dem Kalibrieren zurückgekehrt
        auto calibrationStartPosition = GetPositionAtPositionInterface();

        // In den Betriebsmodus Punkt zu Punkt wechseln, erforderlich damit sich die Motoren bewegen
        SetOperationMode(OperationMode::PointToPoint);
        while (GetOperationMode() != OperationMode::PointToPoint);

        // Wait before reading out motor load, because the load value changes very slow

        int loadCounter = 0;
        do {
            Sleep(TELEMECANIQUE_MOTOR_LOAD_DELAY_TIME);
            if (GetLoad() < TELEMECANIQUE_LOAD_LIMIT)
            {
                loadCounter++;
            }

        } while (loadCounter <= TELEMECANIQUE_REQUIRED_CYCLES_FOR_MOTOR_LOAD);
        loadCounter = 0;

        // Suche des Minimalen Positionslimits: Soweit zurück gehen, bis die Motorbelastung ansteigt
        SetTargetPosition(-TELEMECANIQUE_STEP_WIDTH);

        // Wartezeit dazwischen, weil Motorbelastung sich nur sehr langsam ändert
        do {
            Sleep(TELEMECANIQUE_MOTOR_LOAD_DELAY_TIME);
            SetPositioningMethod(PositioningMethod::Relative);
        } while (GetLoad() < TELEMECANIQUE_LOAD_LIMIT);

        while (!GetTargetReached());

        // Minimale Position speichern, wird später zum Setzen des Nullpunktes und der Softlimits benötigt
        auto calibrationMinPosition = GetPositionAtPositionInterface();

        // Move the bar at least the half of the moving range of the midfield bar to reduce motor load
        // Do not go to start position because start position could be the maximum position and motor
        // load won't reduce for next steps
        SetTargetPosition(calibrationMinPosition + TELEMECANIQUE_GO_BACK_DISTANCE);
        SetPositioningMethod(PositioningMethod::Absolute);

        // Warten bis Motorbelastung zurückgegangen ist
        //OSTimeDlyHMSM (0, 0, 0, MOTOR_CONTROL_TELEMECANIQUE_MOTOR_LOAD_DELAY_TIME);  edited
        do {
            Sleep(TELEMECANIQUE_MOTOR_LOAD_DELAY_TIME);
            if (GetLoad() < TELEMECANIQUE_LOAD_LIMIT) {
                loadCounter++;
            }

        } while (loadCounter <= TELEMECANIQUE_REQUIRED_CYCLES_FOR_MOTOR_LOAD);
        loadCounter = 0;

        // Suche des Endes des Spielfeldes, soweit vorwärts gehen, bis die Motorbelastung ansteigt
        SetTargetPosition(TELEMECANIQUE_STEP_WIDTH);

        do {
            Sleep(TELEMECANIQUE_MOTOR_LOAD_DELAY_TIME);
            SetPositioningMethod(PositioningMethod::Relative);
        } while (GetLoad() < TELEMECANIQUE_LOAD_LIMIT);

        while (!GetTargetReached());

        // Maximale Position speichern, wird später zum Setzen des Nullpunktes und der Softlimits benötigt
        auto calibrationMaxPosition = GetPositionAtPositionInterface();

        // Zurück zur Startposition
        SetTargetPosition(calibrationMinPosition + 1850); // TODO: Choose right DefaultMinPositionBuffer
        SetPositioningMethod(PositioningMethod::Absolute);
        while (!GetTargetReached());

        // Betriebsmodus Referenzierung aktiveren, nur in diesem Modus ist Maßsetzen möglich
        SetOperationMode(OperationMode::Referencing);
        while (GetOperationMode() != OperationMode::Referencing);

        // .. und schreiben es in den Motor
        SetPositionForDimensionSetting(0);

        // Maßsetzen aktiviren, um Nullpunkt festlegen zu können
        SetHomingMethod(HomingMethod::SetDimensions);
        while (GetHomingMethod() != HomingMethod::SetDimensions);

        //MotorDiagnostic_ApplyNewSetpoint(motorNumber);
        SetPositioningMethod(PositioningMethod::Absolute);

        // Calculate new maximum allowed position
        calibrationMaxPosition -= calibrationMinPosition;
        calibrationMaxPosition -= 1850; // TODO: Choose appropriate DefaultMinPositionBuffer
        calibrationMaxPosition -= 1000; // TODO: Choose appropriate DefaultMaxPositionBuffer

        // Set new minimum allowed position to 0 after setting new allowed maximum position
        calibrationMinPosition = 0;

        // Spannung abschalten, sonst können keine Positionslimits gesetzt werden
        DisableVoltage();
        while (GetOperationState() != OperationState::Ready);

        // Untere und obere Positionsgrenze setzten, immer Toleranz mit einbeziehen
        SetMinPosLimit(calibrationMinPosition - 1850); // TODO: DefaultMinPosBuffer
        SetMaxPosLimit(calibrationMaxPosition + 1000); // TODO: DefaultMaxPosBuffer


        // Wenn Positionslimits gesetzt sind, kann Spannung wieder eingeschalten werden
        EnableOperation();
        while (GetOperationState() != OperationState::OperationEnabled);

        // Minimales Positionslimit aktivieren
        SetPositionLimitState(PositionLimitState::BothEnabled);
        while (GetPositionLimitState() != PositionLimitState::BothEnabled);

        // Abschließend wieder Punkt zu Punkt aktivieren
        SetOperationMode(OperationMode::PointToPoint);
        while (GetOperationMode() != OperationMode::PointToPoint);

        SetTargetPosition(calibrationMaxPosition / 2);
        SetPositioningMethod(PositioningMethod::Absolute);
    }
}