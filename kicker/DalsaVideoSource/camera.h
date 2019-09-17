#pragma once

#include <mutex>
#include "SapClassBasic.h"

const int TRANSFER_WAIT_TIMEOUT = 5000;
const int BUFFER_SIZE = 5;

struct RoiSettings {
    int x_min;
    int y_min;
    int width;
    int height;
};

class Camera {
public:
    char* name;

    // Acquisition objects
    SapAcqDevice* device;
    SapBufferWithTrash* trash_buffer;
    SapTransfer* transfer;

    // Status & Setting
    bool acquisition_running = false;
    RoiSettings roi;

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
        RoiSettings roi,
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
