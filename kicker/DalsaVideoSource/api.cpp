#include "pch.h"
#include <iostream>
#include <mutex>

#include "api.h"
#include "camera.h"

void GetAvailableCameras() {
    int server_count = SapManager::GetServerCount();
    char** cameras = new char* [server_count]();

    for (int i = 0; i < server_count; i++) {
        char server_name[64];
        SapManager::GetServerName(i, server_name, sizeof(server_name));
        cameras[i] = server_name;
    }

    // TODO: return cameras to C# caller
}

void* CreateCamera(char* camera_name,
    void __stdcall frame_callback(int index, void* address),
    void __stdcall connected_callback(char* server_name),
    void __stdcall disconnected_callback(char* server_name)
) {
    auto camera = new Camera(camera_name,
        frame_callback, connected_callback, disconnected_callback);

    return camera;
}

void DestroyCamera(Camera* camera) {
    delete camera;
}

bool StartAcquisition(Camera* camera) {
    if (camera == nullptr) {
        return false;
    }
    
    return camera->StartAcquisition();
}

bool StopAcquisition(Camera* camera) {
    if (camera == nullptr) {
        return false;
    }

    return camera->StopAcquisition();
}

bool ReleaseBuffer(Camera* camera, int buffer_index) {
    if (camera == nullptr || camera->buffer == nullptr) {
        std::cout << "camera or buffer null" << std::endl;
        return false;
    }

    return camera->buffer->SetState(buffer_index, SapBuffer::StateEmpty);
}

bool SetFeatureValue(Camera* camera, char* feature_name, double feature_value) {
    if (camera == nullptr) {
        return false;
    }
    
    if (camera->device == nullptr) {
        camera->CreateObjects();
    }

    return camera->device->SetFeatureValue(feature_name, feature_value);
}
