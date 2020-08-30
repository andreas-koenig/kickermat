#include "Diagnostics.h"
#include "../CanOpen/CanOpen.h"

namespace Motor {
    Diagnostics::Diagnostics(NTCAN_RESULT* result) {
        *result = canOpen(
            CanOpen::net,
            CanOpen::flags,
            CanOpen::txqueuesize,
            CanOpen::rxqueuesize,
            CanOpen::txtimeout,
            CanOpen::rxqueuesize,
            &CanHandle);

        if (*result == NTCAN_SUCCESS) {
            canSetBaudrate(&CanHandle, NTCAN_BAUD_1000);

            // TODO: add id filter
            // canIdRegionAdd(...)
        }
    }

    Diagnostics::~Diagnostics() {
        canClose(&CanHandle);
    }

    void Diagnostics::ProcessMessage(CMSG* msg) {
        // Parse the COB in the CAN identifier
        uint8_t functionCode = msg->id & FC_MASK;
        uint8_t deviceAddress = (msg->id >> 4)& DEV_ADDR_MASK;

        // TODO: Process messages and update diagnostics
    }
}
