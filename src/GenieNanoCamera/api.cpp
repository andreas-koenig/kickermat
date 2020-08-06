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
    RoiSettings roi,
    void __stdcall frame_callback(int index, void* address),
    void __stdcall connected_callback(char* server_name),
    void __stdcall disconnected_callback(char* server_name)
) {
    auto camera = new Camera(camera_name, roi, frame_callback,
        connected_callback, disconnected_callback);

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
    if (camera == nullptr || camera->trash_buffer == nullptr) {
        std::cout << "[Dalsa VideoSource] ReleaseBuffer: Camera or buffer null" << std::endl;
        return false;
    }

    return camera->trash_buffer->SetState(buffer_index, SapBuffer::StateEmpty);
}

bool SetFeatureValue(Camera* camera, char* feature_name, double feature_value) {
    if (camera == nullptr) {
        return false;
    }
    
    if (!camera->resources_created) {
        camera->CreateObjects();
    }

    return camera->device->SetFeatureValue(feature_name, (int)feature_value);
}
