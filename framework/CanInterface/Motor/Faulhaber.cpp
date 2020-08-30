#include "../CanOpen/CanOpen.h"
#include "Faulhaber.h"

namespace Motor {
    enum Pdo2Commands {
        SetAbsolutePosition = 0xB4,
        SetRelativePosition = 0xB6,
        InitiateMotion = 0x3C
    };

    Faulhaber::Faulhaber(uint8_t motorId) : BaseMotor(motorId) {}

    void Faulhaber::RotateBar(uint32_t angle, PositioningMethod positioningMethod) {
        const int32_t position = angle * (FH_NUMBER_OF_STEPS / 360);

        uint8_t command;
        switch (positioningMethod)
        {
        case PositioningMethod::Absolute:
            command = Pdo2Commands::SetAbsolutePosition;
            break;
        case PositioningMethod::Relative:
            command = Pdo2Commands::SetRelativePosition;
            break;
        default:
            command = Pdo2Commands::SetAbsolutePosition;
            break;
        }

        auto msg = CanOpen::CreatePdoMessage(MotorId, command, position, CanOpen::FunctionCode::R_PDO2);
        auto msg2 = CanOpen::CreatePdoMessage(MotorId, Pdo2Commands::InitiateMotion, 0x0, CanOpen::FunctionCode::R_PDO2);

        int32_t numMessages = 1;
        canSend(CanHandle, &msg, &numMessages);
        canSend(CanHandle, &msg2, &numMessages);
    }

    void Faulhaber::SetHomingOffset(uint16_t angle) {
        CMSG msg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::FaulhaberHomingOffset),
            OBJECT_SUB_INDEX(CanOpen::FaulhaberHomingOffset),
            (uint32_t)angle,
            4);

        CMSG response;
        RequestSdo(&msg, &response);
    }

    uint32_t Faulhaber::GetScaleNumerator() {
        CMSG msg = CanOpen::CreateSdoReadRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::TargetPositionNumerator),
            OBJECT_SUB_INDEX(CanOpen::TargetPositionNumerator));

        CMSG response;
        RequestSdo(&msg, &response);

        uint32_t numerator;
        CanOpen::ExtractDataFromMessage(&response, &numerator, MotorId);

        return numerator;
    }

    void Faulhaber::SetScaleNumerator(uint16_t numerator) {
        CMSG msg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::TargetPositionNumerator),
            OBJECT_SUB_INDEX(CanOpen::TargetPositionNumerator),
            numerator,
            4);

        CMSG response;
        RequestSdo(&msg, &response);
    }

    uint32_t Faulhaber::GetScaleFeedConstant() {
        CMSG msg = CanOpen::CreateSdoReadRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::TargetPositionFeedConstant),
            OBJECT_SUB_INDEX(CanOpen::TargetPositionFeedConstant));

        CMSG response;
        RequestSdo(&msg, &response);

        uint32_t constant;
        CanOpen::ExtractDataFromMessage(&response, &constant, MotorId);

        return constant;
    }

    void Faulhaber::SetScaleFeedConstant(uint16_t constant) {
        CMSG msg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::TargetPositionFeedConstant),
            OBJECT_SUB_INDEX(CanOpen::TargetPositionFeedConstant),
            constant,
            4);

        CMSG response;
        RequestSdo(&msg, &response);
    }

    void Faulhaber::Calibrate() {
        StartRemoteNode();

        DisableVoltage();
        while (GetOperationState() != OperationState::SwitchOnDisabled);

        Sleep(500);

        Shutdown();
        while (GetOperationState() != OperationState::Ready);

        SwitchOn();
        while (GetOperationState() != OperationState::OperationSwitchedOn);

        EnableOperation();
        while (GetOperationState() != OperationState::OperationEnabled);

        SetOperationMode(OperationMode::PointToPoint);
        while (GetOperationMode() != OperationMode::PointToPoint);

        //Setze Positions Faktor so, dass der Motor direkt mit Winkelangaben positioniert werden kann
        SetScaleNumerator(360);
        while (GetScaleNumerator() != 360);

        SetScaleFeedConstant(FH_NUMBER_OF_STEPS);
        while (GetScaleFeedConstant() != FH_NUMBER_OF_STEPS);

        SetTargetPosition(360);
        SetPositioningMethod(PositioningMethod::Relative);
        while (!GetTargetReached()) {
            Sleep(50);
        }

        // RoRe: Am Schluss wieder zum Nullpunkt zurück, damit die Kalibrierung das nicht machen muss
        SetTargetPosition(0);
        SetPositioningMethod(PositioningMethod::Absolute);
        while (!GetTargetReached());

        SetOperationMode(OperationMode::FaulhaberMode);
        while (GetOperationMode() != OperationMode::FaulhaberMode);
    }
}
