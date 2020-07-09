#include <mutex>
#include <climits>

#include "Motor.h"
#include "../CanOpen/CanOpen.h"

namespace Motor {
    BaseMotor::BaseMotor(uint8_t motorId) {
        MotorId = motorId;
        Mutex = CreateMutex(NULL, FALSE, NULL);

        auto ret = canOpen(
            CanOpen::net,
            CanOpen::flags,
            CanOpen::txqueuesize,
            CanOpen::rxqueuesize,
            CanOpen::txtimeout,
            CanOpen::rxtimeout,
            &CanHandle);

        if (ret != NTCAN_SUCCESS) {
            printf("Failed to create CanHandle for motor %d", MotorId);
        }

        canSetBaudrate(CanHandle, NTCAN_BAUD_1000);

        // Configure message filter to only accept messages addressed to this specific motor
        canIdAdd(CanHandle, CanOpen::getCobId(CanOpen::FunctionCode::T_SDO, MotorId));
    }

    void BaseMotor::RequestSdo(CMSG* request, CMSG* response) {
        int32_t numSendMessages = 1;
        int32_t numReceiveMessages;

        WaitForSingleObject(Mutex, INFINITE);

        if (NTCAN_SUCCESS != canSend(CanHandle, request, &numSendMessages)) {
            printf("fail");
        }

        int tries = 0;
        do {
            numReceiveMessages = 1;
            if (NTCAN_SUCCESS != canTake(CanHandle, response, &numReceiveMessages)) {
                printf("fail");
            }

            tries++;

            if (tries == 10000) {
                if (NTCAN_SUCCESS != canSend(CanHandle, request, &numSendMessages)) {
                    printf("fail");
                };
                tries = 0;
            }
        } while (numReceiveMessages == 0);

        ReleaseMutex(Mutex);
    }

    void BaseMotor::StartRemoteNode() {
        CMSG msg = {};
        msg.id = 0x0;
        msg.len = 2;
        msg.data[0] = 0x01;
        msg.data[1] = MotorId;
        msg.data[2] = 0x0;
        msg.data[3] = 0x0;

        int32_t len = 1;
        canSend(CanHandle, &msg, &len);
    }

    void BaseMotor::SetOperationMode(OperationMode mode) {
        CMSG msg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::ModesOfOperation),
            OBJECT_SUB_INDEX(CanOpen::ModesOfOperation),
            (uint32_t)mode, // uint8_t
            1);

        CMSG response;
        RequestSdo(&msg, &response);
    }

    OperationMode BaseMotor::GetOperationMode() {
        CMSG msg = CanOpen::CreateSdoReadRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::ModesOfOperationDisplay),
            OBJECT_SUB_INDEX(CanOpen::ModesOfOperationDisplay));

        CMSG response;
        RequestSdo(&msg, &response);

        uint32_t data;
        CanOpen::ExtractDataFromMessage(&response, &data, MotorId);

        return (OperationMode)data;
    }

    void BaseMotor::SetRpdo2State(Rpdo2State state) {
        uint32_t data = state | MotorId;

        CMSG msg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::ReceivePdo2Parameter),
            OBJECT_SUB_INDEX(CanOpen::ReceivePdo2Parameter),
            data,
            4);

        CMSG response;
        RequestSdo(&msg, &response);
    }

    Rpdo2State BaseMotor::GetRpdo2State() {
        CMSG msg = CanOpen::CreateSdoReadRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::ReceivePdo2Parameter),
            OBJECT_SUB_INDEX(CanOpen::ReceivePdo2Parameter));

        CMSG response;
        RequestSdo(&msg, &response);

        uint32_t data;
        CanOpen::ExtractDataFromMessage(&response, &data, MotorId);

        return (Rpdo2State)(data & 0xFFFFFF80);
    }

    void BaseMotor::DisableVoltage() {
        CMSG msg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::ControlWord),
            OBJECT_SUB_INDEX(CanOpen::ControlWord),
            0,
            2);

        CMSG response;
        RequestSdo(&msg, &response);
    }

    uint16_t BaseMotor::GetStatusWord() {
        CMSG msg = CanOpen::CreateSdoReadRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::StatusWord),
            OBJECT_SUB_INDEX(CanOpen::StatusWord));

        CMSG response;
        RequestSdo(&msg, &response);

        uint32_t statusWord;
        CanOpen::ExtractDataFromMessage(&response, &statusWord, MotorId);

        return (uint16_t)statusWord;
    }

    OperationState BaseMotor::GetOperationState() {
        auto statusWord = GetStatusWord();

        if ((!(statusWord & StatusWord::ReadyToSwitchOn))
            && (!(statusWord & StatusWord::SwitchedOn))
            && (!(statusWord & StatusWord::OperationEnable))
            && (!(statusWord & StatusWord::Fault))
            && (!(statusWord & StatusWord::SwitchOnDisable)))
        {
            return OperationState::NotReady;
        }

        if ((!(statusWord & StatusWord::ReadyToSwitchOn))
            && (!(statusWord & StatusWord::SwitchedOn))
            && (!(statusWord & StatusWord::OperationEnable))
            && (!(statusWord & StatusWord::Fault))
            && ((statusWord & StatusWord::SwitchOnDisable) == StatusWord::SwitchOnDisable))
        {
            return OperationState::SwitchOnDisabled;
        }

        if (((statusWord & StatusWord::ReadyToSwitchOn) == StatusWord::ReadyToSwitchOn)
            && (!(statusWord & StatusWord::SwitchedOn))
            && (!(statusWord & StatusWord::OperationEnable))
            && (!(statusWord & StatusWord::Fault))
            && ((statusWord & StatusWord::QuickStop) == StatusWord::QuickStop)
            && (!(statusWord & StatusWord::SwitchOnDisable)))
        {
            return OperationState::Ready;
        }

        if (((statusWord & StatusWord::ReadyToSwitchOn) == StatusWord::ReadyToSwitchOn)
            && ((statusWord & StatusWord::SwitchedOn) == StatusWord::SwitchedOn)
            && (!(statusWord & StatusWord::OperationEnable))
            && (!(statusWord & StatusWord::Fault))
            && ((statusWord & StatusWord::QuickStop) == StatusWord::QuickStop)
            && (!(statusWord & StatusWord::SwitchOnDisable)))
        {
            return OperationState::OperationSwitchedOn;
        }

        if (((statusWord & StatusWord::ReadyToSwitchOn) == StatusWord::ReadyToSwitchOn)
            && ((statusWord & StatusWord::SwitchedOn) == StatusWord::SwitchedOn)
            && ((statusWord & StatusWord::OperationEnable) == StatusWord::OperationEnable)
            && (!(statusWord & StatusWord::Fault))
            && ((statusWord & StatusWord::QuickStop) == StatusWord::QuickStop)
            && (!(statusWord & StatusWord::SwitchOnDisable)))
        {
            return OperationState::OperationEnabled;
        }

        if (((statusWord & StatusWord::ReadyToSwitchOn) == StatusWord::ReadyToSwitchOn)
            && ((statusWord & StatusWord::SwitchedOn) == StatusWord::SwitchedOn)
            && ((statusWord & StatusWord::OperationEnable) == StatusWord::OperationEnable)
            && (!(statusWord & StatusWord::Fault))
            && (!(statusWord & StatusWord::QuickStop))
            && (!(statusWord & StatusWord::SwitchOnDisable)))
        {
            return OperationState::QuickStopActive;
        }

        if (((statusWord & StatusWord::ReadyToSwitchOn) == StatusWord::ReadyToSwitchOn)
            && ((statusWord & StatusWord::SwitchedOn) == StatusWord::SwitchedOn)
            && ((statusWord & StatusWord::OperationEnable) == StatusWord::OperationEnable)
            && ((statusWord & StatusWord::Fault) == StatusWord::Fault)
            && (!(statusWord & StatusWord::SwitchOnDisable)))
        {
            return OperationState::OperationFault;
        }

        return OperationState::OperationFault;
    }

    void BaseMotor::EnableOperation() {
        CMSG msg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::ControlWord),
            OBJECT_SUB_INDEX(CanOpen::ControlWord),
            15,
            2);

        CMSG response;
        RequestSdo(&msg, &response);
    }

    void BaseMotor::SetTargetPosition(int32_t position) {
        CMSG msg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::TargetPositionPointToPoint),
            OBJECT_SUB_INDEX(CanOpen::TargetPositionPointToPoint),
            position,
            4);

        CMSG response;
        RequestSdo(&msg, &response);
    }

    void BaseMotor::SetPositioningMethod(PositioningMethod method) {
        // Get current control word
        CMSG msgReadCtrlWord = CanOpen::CreateSdoReadRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::ControlWord),
            OBJECT_SUB_INDEX(CanOpen::ControlWord));

        CMSG response;
        RequestSdo(&msgReadCtrlWord, &response);

        uint32_t controlWord;
        CanOpen::ExtractDataFromMessage(&response, &controlWord, MotorId);

        // Apply new set point
        controlWord &= (~DCOM_CONTROL_WORD_NEW_SETPOINT);

        CMSG msgWriteCtrlWord = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::ControlWord),
            OBJECT_SUB_INDEX(CanOpen::ControlWord),
            controlWord,
            2);

        RequestSdo(&msgWriteCtrlWord, &response);

        // Set positioning
        controlWord |= DCOM_CONTROL_WORD_NEW_SETPOINT;
        controlWord &= (~DCOM_CONTROL_WORD_CHANGE_SET_IMMEDIATLY);

        switch (method) {
        case PositioningMethod::Absolute:
            controlWord &= (~DCOM_CONTROL_WORD_POSITIONING_RELATIVE);
            break;
        case PositioningMethod::Relative:
            controlWord |= DCOM_CONTROL_WORD_POSITIONING_RELATIVE;
            break;
        }

        CMSG msgPositioning = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::ControlWord),
            OBJECT_SUB_INDEX(CanOpen::ControlWord),
            (uint16_t)controlWord,
            2);

        RequestSdo(&msgPositioning, &response);
    }

    BOOL BaseMotor::GetTargetReached() {
        uint16_t statusWord = GetStatusWord();

        return ((statusWord & StatusWord::TargetReached) == StatusWord::TargetReached);
    }

    void BaseMotor::Shutdown() {
        CMSG msg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::ControlWord),
            OBJECT_SUB_INDEX(CanOpen::ControlWord),
            6,
            2);

        CMSG response;
        RequestSdo(&msg, &response);
    }

    void BaseMotor::SwitchOn() {
        CMSG msg = CanOpen::CreateSdoWriteRequest(
            CanOpen::FunctionCode::R_SDO,
            MotorId,
            OBJECT_INDEX(CanOpen::ControlWord),
            OBJECT_SUB_INDEX(CanOpen::ControlWord),
            7,
            2);

        CMSG response;
        RequestSdo(&msg, &response);
    }
}
