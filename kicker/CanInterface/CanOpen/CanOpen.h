#pragma once

#include <esd/ntcan.h>

#define OBJECT_INDEX(entry) CanOpen::ObjectDirectory[entry].Index
#define OBJECT_SUB_INDEX(entry) CanOpen::ObjectDirectory[entry].SubIndex

namespace CanOpen {
    const int net = 0x015;
    const uint32_t flags = 0;
    const int32_t txqueuesize = 8;
    const int32_t rxqueuesize = 8;
    const int32_t txtimeout = 100;
    const int32_t rxtimeout = 0;

    enum FunctionCode {
        NMT = 0x000,
        SYNC = 0x100,
        TIME = 0x200,
        T_PDO1 = 0x180,
        R_PDO1 = 0x200,
        T_PDO2 = 0x280,
        R_PDO2 = 0x300,
        T_PDO3 = 0x380,
        R_PDO3 = 0x400,
        T_PDO4 = 0x480,
        R_PDO4 = 0x500,
        T_SDO = 0x580,
        R_SDO = 0x600,
        HEARTBEAT = 0x700,
        INVALID = -1,
    };

    int32_t getCobId(FunctionCode functionCode, uint8_t nodeId);

    CMSG CreateSdoWriteRequest(
        FunctionCode functionCode,
        uint8_t nodeId,
        uint16_t index,
        uint8_t subIndex,
        uint32_t data,
        uint8_t numBytes
    );

    CMSG CreateSdoReadRequest(
        FunctionCode functionCode,
        uint8_t nodeId,
        uint16_t index,
        uint8_t subIndex
    );

    uint8_t ExtractDataFromMessage(CMSG* message, uint32_t* data, uint8_t motorId);

    CMSG CreatePdoMessage(uint8_t motorId, uint32_t controlWord, uint32_t value, FunctionCode functionCode);

    typedef enum
    {
        ControlWord = 0,
        StatusWord = 1,
        FaulhaberHomingOffset = 2,
        TelemecaniqueMotorLoad = 3,
        MaxProfileVelocity = 4,
        ModesOfOperation = 5,
        ModesOfOperationDisplay = 6,
        MaxPositionLimit = 7,
        MinPositionLimit = 8,
        ProfileVelocity = 9,
        ProfileAcceleration = 10,
        ProfileDeceleration = 11,
        ReceivePdo2Parameter = 12,
        TransmitPdo2Parameter = 13,
        TargetPositionPointToPoint = 14,
        TelemecaniqueHomingMethod = 15,
        TelemecaniquePositionForDimensionSetting = 16,
        TargetPositionNumerator = 17,
        TargetPositionFeedConstant = 18,
        RotationDirection = 19,
        PositionLimitState = 20,
        PositionAtPositionInterface = 21,
        TelemecaniquePositionScaleDenominator = 22,
        TelemecaniquePositionScaleNumerator = 23
    } MotorAbstractionObject;

    typedef struct
    {
        MotorAbstractionObject Entry;
        uint16_t Index;
        uint8_t SubIndex;
    } ObjectDirectoryItem;

    static const ObjectDirectoryItem ObjectDirectory[] =
    {
        {ControlWord, 0x6040, 0x00},
        {StatusWord, 0x6041, 0x00},
        {FaulhaberHomingOffset, 0x607C, 0x00},
        {TelemecaniqueMotorLoad, 0x301C, 0x1A},
        {MaxProfileVelocity, 0x607F, 0x00},
        {ModesOfOperation, 0x6060, 0x00},
        {ModesOfOperationDisplay, 0x6061, 0x00},
        {MaxPositionLimit, 0x607D, 0x02},
        {MinPositionLimit, 0x607D, 0x01},
        {ProfileVelocity, 0x6081, 0x00},
        {ProfileAcceleration, 0x6083, 0x00},
        {ProfileDeceleration, 0x6084, 0x00},
        {ReceivePdo2Parameter, 0x1401, 0x01},
        {TransmitPdo2Parameter, 0x1801, 0x01},
        {TargetPositionPointToPoint, 0x607A, 0x00},
        {TelemecaniqueHomingMethod, 0x6098, 0x00},
        {TelemecaniquePositionForDimensionSetting, 0x301B, 0x16},
        {TargetPositionFeedConstant, 0x6093, 0x02},
        {TargetPositionNumerator, 0x6093, 0x01},
        {RotationDirection, 0x3006, 0x0C},
        {PositionLimitState, 0x3006, 0x03},
        {PositionAtPositionInterface, 0x6064, 0x00},
        {TelemecaniquePositionScaleDenominator, 0x3006, 0x07},
        {TelemecaniquePositionScaleNumerator, 0x3006, 0x08}
    };
}
