#pragma once
#include <esd\ntcan.h>

const int32_t FC_MASK = 0x0F;
const int32_t DEV_ADDR_MASK = 0x7F0;

namespace Motor {
    class Diagnostics {
    public:
        Diagnostics(NTCAN_RESULT* result);
        ~Diagnostics();

    protected:
        NTCAN_HANDLE CanHandle;
        void ProcessMessage(CMSG* msg);
    };
}

