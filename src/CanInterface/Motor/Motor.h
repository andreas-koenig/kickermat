#pragma once

#include <esd/ntcan.h>

#define ID_FH_GOALKEEPER 0x01
#define ID_FH_DEFENSE 0x02
#define ID_FH_MIDFIELD 0x03
#define ID_FH_STRIKER 0x04

#define ID_TM_GOALKEEPER 0x0B
#define ID_TM_DEFENSE 0x0C
#define ID_TM_MIDFIELD 0x0D
#define ID_TM_STRIKER 0x0E

namespace Motor {

#pragma region enums
    enum OperationMode {
        PointToPoint = 0x01,
        VelocityProfile = 0x03,
        Referencing = 0x06,
        FaulhaberMode = 0xFF,
    };

    enum OperationState
    {
        NotReady,
        OperationEnabled,
        QuickStopActive,
        Ready,
        SwitchOnDisabled,
        OperationSwitchedOn,
        OperationFault
    };

    enum MotorState
    {
        Unknown = 0,
        Stopped = 4,
        Operational = 5,
        PreOperational = 127
    };

    enum Rpdo2State
    {
        Rpdo2Enabled = 0x00000300,
        Rpdo2Disabled = 0x80000300
    };

    enum PositioningMethod {
        Absolute,
        Relative,
    };

    // Bits of the control word of a CANopen node.
    typedef enum
    {
        DCOM_CONTROL_WORD_SWITCH_ON = (0x0001 << 0),				// Bit 0: Switch On
        DCOM_CONTROL_WORD_ENABLE_VOLTAGE = (0x0001 << 1),			// Bit 1: Enable voltage.
        DCOM_CONTROL_WORD_QUICK_STOP = (0x0001 << 2),				// Bit 2: Quick stop.
        DCOM_CONTROL_WORD_ENABLE_OPERATION = (0x0001 << 3),			// Bit 3: Enable operation.
        DCOM_CONTROL_WORD_NEW_SETPOINT = (0x0001 << 4),				// Bit 4: Apply new position.
        DCOM_CONTROL_WORD_CHANGE_SET_IMMEDIATLY = (0x0001 << 5),	// Bit 5: Change position immediately.
        DCOM_CONTROL_WORD_POSITIONING_RELATIVE = (0x0001 << 6),		// Bit 6: Relative positioning.
        DCOM_CONTROL_WORD_FAULT_RESET = (0x0001 << 7),				// Bit 7: Fault reset.
        DCOM_CONTROL_WORD_HALT = (0x0001 << 8)						// Bit 8: Halt.
    } DcomControlWordBits;

    // Bits of a status word of a CANopen node.
    enum StatusWord
    {
        ReadyToSwitchOn = (0x0001 << 0),	// Bit 0: Ready to switch on.
        SwitchedOn = (0x0001 << 1),			// Bit 1: Switched on.
        OperationEnable = (0x0001 << 2),	// Bit 2: Operation enable.
        Fault = (0x0001 << 3),				// Bit 3: Fault.
        VoltageEnabled = (0x0001 << 4),		// Bit 4: Voltage enabled.
        QuickStop = (0x0001 << 5),			// Bit 5: Quick stop.
        SwitchOnDisable = (0x0001 << 6),	// Bit 6: Switch on disable.
        TargetReached = (0x0001 << 10),		// Bit 10: Target reached (used for motors).
        XEnd = (0x0001 << 14),				// Bit 14: X_END
    };

    // Bit masks for TPDO2 enabled/disabled state. The 7 less significant bits
    // contain the node ID and must be added/removed manually during use.
    typedef enum
    {
        TPDO2Enabled = 0x00000280,			// TPDO2 is enabled.
        TPDO2Disabled = 0x80000280			// TPDO2 is disabled.
    } TPDO2StateType;

    // Enumeration of all known homing methods of the motors.
    typedef enum
    {
        UNKNOWN_METHOD,						// Homing method is unknown
        SET_DIMENSIONS = 0x23				// Homing via set dimensions
    } MotorHomingMethod;

    typedef enum
    {
        ABSOLUTE_POSITIONING,				// Apply position absolute to zero position.
        RELATIVE_POSITIONING				// Apply position relative to current position.
    } MotorPositioningMethod;

    // Rotation directions of a motor.
    typedef enum
    {
        CLOCKWISE_ROTATION = 0x00,			// Rotate motor clockwise.
        ANTICLOCKWISE_ROTATION = 0x01,		// Rotate motor anti-clockwise.
        UNKNOWN_ROTATION = 0xFF				// Unknown rotation direction.
    } MotorRotationDirection;

#pragma endregion enums

    class BaseMotor {
    public:
        BaseMotor(uint8_t motorId);

        // Calibration
        virtual void Calibrate() = 0;

        // Send SDO
        void RequestSdo(CMSG* request, CMSG* response);

        // Network Management
        void StartRemoteNode();

        // Write SDOs
        void SetOperationMode(OperationMode mode);
        void EnableOperation();
        void SetRpdo2State(Rpdo2State state);
        void SetTargetPosition(int32_t position);
        void SetPositioningMethod(PositioningMethod method);
        void DisableVoltage();
        void Shutdown();
        void SwitchOn();

        // Read SDOs
        OperationMode GetOperationMode();
        OperationState GetOperationState();
        Rpdo2State GetRpdo2State();
        BOOL GetTargetReached();
        uint16_t GetStatusWord();

    protected:
        NTCAN_HANDLE CanHandle;
        uint8_t MotorId;

    private:
        HANDLE Mutex;
    };
}
