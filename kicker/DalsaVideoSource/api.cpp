#include "pch.h"
#include <iostream>

#include "api.h"
#include "controller.h"

void server_connected(SapManCallbackInfo* info) {
    std::cout << "CONNECTED [Server Index: " << info->GetServerIndex()
        << ", Resource Index: " << info->GetResourceIndex() << "]" << std::endl;
}

void server_disconnected(SapManCallbackInfo* info) {
    std::cout << "DISCONNECTED [Server Index: " << info->GetServerIndex()
        << ", Resource Index: " << info->GetResourceIndex() << "]" << std::endl;
}

void startup(
    void __stdcall frame_callback(int index, void* address),
    void __stdcall connected_callback(char* server_name),
    void __stdcall disconnected_callback(char* server_name)
) {
    SapManager::Open();

    CameraController* ctrl = CameraController::getInstance();

    ctrl->frame_arrived = frame_callback;
    ctrl->cam_connected = connected_callback;
    ctrl->cam_disconnected = disconnected_callback;

    SapManager::SetDisplayStatusMode(SapManager::StatusCallback, CameraController::ErrorCallback);
    SapManager::RegisterServerCallback(
        SapManager::EventServerDisconnected | SapManager::EventServerConnected,
        CameraController::ServerCallback);
}

void shutdown() {
    CameraController::releaseInstance();
    SapManager::UnregisterServerCallback();
    SapManager::Close();
}

void get_available_cameras() {
    int server_count = SapManager::GetServerCount();
    char** cameras = new char* [server_count]();

    for (int i = 0; i < server_count; i++) {
        char server_name[64];
        SapManager::GetServerName(i, server_name, sizeof(server_name));
        cameras[i] = server_name;
    }

    // TODO: return cameras to C# caller
}

bool start_acquisition(char* camera_name) {
    bool success = CameraController::getInstance()->start_acquisition(camera_name);
    if (success) {
        std::cout << "[Dalsa VideoSource] Acquisition started" << std::endl;
    }
    else {
        std::cout << "[Dalsa VideoSource] Acquisition start failed" << std::endl;
    }

    return success;
}

void stop_acquisition() {
	CameraController::getInstance()->stop_acquisition();
	std::cout << "[Dalsa VideoSource] Acquisition stopped" << std::endl;
}

void release_buffer(int index) {
    CameraController::getInstance()->buffer->SetState(index, SapBuffer::StateEmpty);
}
