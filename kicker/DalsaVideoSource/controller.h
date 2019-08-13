#pragma once

#include "SapClassBasic.h"

const int TRANSFER_WAIT_TIMEOUT = 5000;
const int BUFFER_SIZE = 5;

class CameraController {
public:
    // Acquisition objects
    SapAcqDevice* acquisitionDevice;
    SapBufferWithTrash* buffer;
    SapTransfer* transfer;
    bool acquisition_running = false;

    // API Callbacks
    void (*frame_arrived)(int, void*);
    void (*cam_connected)(char*);
    void (*cam_disconnected)(char*);

    // Singleton methods
    static CameraController* getInstance();
    static void releaseInstance();


    // Acquisition functions
    bool start_acquisition(char* camera_name);
    void stop_acquisition();

    // Callbacks
    static void ErrorCallback(SapManCallbackInfo* info);
    static void XferCallback(SapXferCallbackInfo* info);
    static void ServerCallback(SapManCallbackInfo* info);
    
    ~CameraController();
	CameraController(CameraController const&) = delete;
	void operator=(CameraController const&) = delete;

private:
    static CameraController* INSTANCE;
    CameraController();
};
