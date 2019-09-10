#pragma once

#include <mutex>
#include "SapClassBasic.h"

const int TRANSFER_WAIT_TIMEOUT = 5000;
const int BUFFER_SIZE = 5;

class Camera {
public:
    char* name;

    // Acquisition objects
    SapAcqDevice* device;
    SapBufferWithTrash* buffer;
    SapTransfer* transfer;
    bool acquisition_running = false;

    // API Callbacks
    void (*frame_arrived)(int, void*);
    void (*cam_connected)(char*);
    void (*cam_disconnected)(char*);

    // Acquisition functions
    bool StartAcquisition();
    bool StopAcquisition();

    // Callbacks
    static void ErrorCallback(SapManCallbackInfo* info);
    static void XferCallback(SapXferCallbackInfo* info);
    static void ServerCallback(SapManCallbackInfo* info);
    
    // Constructors & Destructors
    Camera(
        char* camera_name,
        void __stdcall frame_callback(int index, void* address),
        void __stdcall connected_callback(char* server_name),
        void __stdcall disconnected_callback(char* server_name)
    );
    ~Camera();
	Camera(Camera const&) = delete;
	void operator=(Camera const&) = delete;

    bool CreateObjects();
    void DestroyObjects();

private:
    void ConfigureCamera();
};
