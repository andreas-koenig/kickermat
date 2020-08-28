typedef enum{
	// Motor is ready to switch on
	MotorOperationStateNotReadyToSwitchOn=0x00,
	// Motor operation is enabled
	MotorOperationStateOperationEnabled=0x27,
	// Quick stop is active
	MotorOperationStateQuickStopActive=0x07,
	// Motor is ready to switch on.
	MotorOperationStateReadyToSwitchOn=0x21,
	// Switch on is disabled
	MotorOperationStateSwitchOnDisabled=0x40,
	// Motor is switched on
	MotorOperationStateSwitchedOn=0x23,

	// Motor is in fault state
	MotorOperationStateFault=0x08,

	//Motor is in an unknown operation state
	MotorOperationStateUnknown=0xFF
}MotorOperationStateType;

typedef enum{
	// Motor state is unknown
	MotorStateUnknown = 0,

	// Motor was stopped
	MotorStateNodeStopped = 4,

	// Motor in operational state
	MotorStateOperational = 5,

	// Motor in pre-operational state
	MotorStatePreOperational = 127
}MotorStateType;

typedef enum {
	DisableVoltage = 0x00,
	Shutdown = 0x06,
	SwitchOn = 0x07,
	EnableOperation = 0x0F
} value;

typedef enum
{
	MotorIDGoalieRotation	=0x01,
	MotorIDDefenseRotation	=0x02,
	MotorIDMidfieldRotation	=0x03,
	MotorIDOffenseRotation	=0x04,
	MotorIDGoalieMove		=0x0B,
	MotorIDDefenseMove		=0x0C,
	MotorIDMidfieldMove		=0x0D,
	MotorIDOffenseMove		=0x0E
}MotorIDs;

#define MotorsAreNotAlive    !(answered[0]&&answered[1]&&answered[2]&&answered[3]&&answered[4]&&answered[5]&&answered[6]&&answered[7])

