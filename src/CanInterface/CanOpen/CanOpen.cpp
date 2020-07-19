#include "CanOpen.h"

namespace CanOpen {
    enum CommandCode {
        WriteResponse = 0x60,
        WriteRequest1 = 0x2F,
        WriteRequest2 = 0x2B,
        WriteRequest3 = 0x27,
        WriteRequest4 = 0x23,
        ReadRequest = 0x40,
        ReadResponse1 = 0x4F,
        ReadResponse2 = 0x4B,
        ReadResponse3 = 0x47,
        ReadResponse4 = 0x43,
        ErrorResponse = 0x80,
    };

    int32_t getCobId(FunctionCode functionCode, uint8_t nodeId) {
        return (((int32_t)0x0) | functionCode | nodeId);
    }

    CMSG CreateSdoWriteRequest(
        FunctionCode functionCode,
        uint8_t nodeId,
        uint16_t index,
        uint8_t subIndex,
        uint32_t data,
        uint8_t numBytes
    ) {
        CMSG message = { 0, 8 };

        message.id = getCobId(functionCode, nodeId);

        switch (numBytes)
        {
        case 1: message.data[0] = WriteRequest1;
            break;
        case 2: message.data[0] = WriteRequest2;
            break;
        case 3: message.data[0] = WriteRequest3;
            break;
        case 4: message.data[0] = WriteRequest4;
            break;
        }

        // Index + Subindex
        message.data[1] = index;
        message.data[2] = index >> 8;
        message.data[3] = subIndex;

        // Data (Little Endian)
        message.data[4] = (uint8_t)data;
        message.data[5] = (uint8_t)(data >> 8);
        message.data[6] = (uint8_t)(data >> 16);
        message.data[7] = (uint8_t)(data >> 24);

        return message;
    }

    CMSG CreateSdoReadRequest(
        FunctionCode functionCode,
        uint8_t nodeId,
        uint16_t index,
        uint8_t subIndex
    ) {
        CMSG message = { 0, 8 };

        message.id = getCobId(functionCode, nodeId);

        message.data[0] = CommandCode::ReadRequest;

        // Index + Subindex
        message.data[1] = index;
        message.data[2] = index >> 8;
        message.data[3] = subIndex;

        return message;
    }

    CMSG CreatePdoMessage(uint8_t motorId, uint32_t controlWord, uint32_t value, FunctionCode functionCode) {
        CMSG message = { 0, 8 };

        message.id = getCobId(functionCode, motorId);

        message.data[0] = (uint8_t)controlWord;

        switch (functionCode) {
        case R_PDO1:
            message.data[1] = (uint8_t)(controlWord >> 8);
            message.data[2] = 0;
            message.data[3] = 0;
            message.data[4] = 0;
            break;
        case R_PDO2:
            message.data[1] = (uint8_t)value;
            message.data[2] = (uint8_t)(value >> 8);
            message.data[3] = (uint8_t)(value >> 16);
            message.data[4] = (uint8_t)(value >> 24);
            break;
        case R_PDO3:
            message.data[1] = (uint8_t)(controlWord >> 8);
            message.data[2] = (uint8_t)value;
            message.data[3] = (uint8_t)(value >> 8);
            message.data[4] = (uint8_t)(value >> 16);
            message.data[5] = (uint8_t)(value >> 24);
            break;
        }

        return message;
    }

    uint8_t ExtractDataFromMessage(CMSG* message, uint32_t* data, uint8_t motorId) {
        FunctionCode functionCode = (FunctionCode)(message->id - motorId);

        switch (message->data[0]) {
        case 79:
            *data = (uint32_t)message->data[4];
            return 1;
        case 75:
            *data = ((uint32_t)message->data[5] << 8 | message->data[4]);
            return 2;
        case 71:
            *data = ((uint32_t)message->data[6] << 16 | (uint32_t)message->data[5] << 8 | message->data[4]);
            return 3;
        case 67:
        default:
            *data = ((uint32_t)message->data[7] << 24) | ((uint32_t)message->data[6] << 16)
                | ((uint32_t)message->data[5] << 8) | message->data[4];
            return 4;
        }
    }
}
