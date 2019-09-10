#include "pch.h"
#include <iostream>
#include "camera.h"

Camera::Camera(
    char* camera_name,
    void __stdcall frame_callback(int index, void* address),
    void __stdcall connected_callback(char* server_name),
    void __stdcall disconnected_callback(char* server_name)
) {
    name = camera_name;
    frame_arrived = frame_callback;
    cam_connected = connected_callback;
    cam_disconnected = disconnected_callback;

    SapManager::Open();
    SapManager::SetDisplayStatusMode(SapManager::StatusCallback, Camera::ErrorCallback);
    SapManager::RegisterServerCallback(
        SapManager::EventServerDisconnected | SapManager::EventServerConnected,
        Camera::ServerCallback, this);
    
    device = new SapAcqDevice(name, FALSE);
    buffer = new SapBufferWithTrash(BUFFER_SIZE, device);
    transfer = new SapAcqDeviceToBuf(device, buffer, XferCallback, this);
}
Camera::~Camera() {
    StopAcquisition();
    DestroyObjects();

    delete name;
    delete device;
    delete buffer;
    delete transfer;

    SapManager::UnregisterServerCallback();
    SapManager::Close();
}

bool Camera::CreateObjects() {
    if (!device->Create()) {
        DestroyObjects();
        return false;
    }

    if (!buffer->Create()) {
        DestroyObjects();
        return false;
    }

    if (!transfer->Create()) {
        DestroyObjects();
        return false;
    }
    transfer->SetAutoEmpty(false);
}

void Camera::DestroyObjects() {
    if (transfer != nullptr)
        transfer->Destroy();

    if (buffer != nullptr)
        buffer->Destroy();

    if (device != nullptr)
        device->Destroy();
}

bool Camera::StartAcquisition() {
    if (!CreateObjects()) {
        return false;
    }

    ConfigureCamera();

    if (transfer->Grab()) {
        acquisition_running = true;
        return true;
    }

    return false;
}

bool Camera::StopAcquisition() {
    if (!acquisition_running) {
        return false;
    }
    
    bool success = false;
    if (transfer->IsGrabbing()) {
        success = transfer->Freeze();
        if (!transfer->Wait(TRANSFER_WAIT_TIMEOUT)) {
            success = transfer->Abort();
        };
    }

    buffer->Clear();
    DestroyObjects();
    acquisition_running = false;
    
    return success;
}

void Camera::ConfigureCamera() {
    device->SetFeatureValue("autoBrightnessMode", true);
    device->SetFeatureValue("BalanceWhiteAuto", "Periodic");
}

void Camera::ServerCallback(SapManCallbackInfo* info) {
    auto camera = (Camera*) info->GetContext();

    char server_name[64];
    SapManager::GetServerName(info->GetServerIndex(), server_name, sizeof(server_name));

    switch (info->GetEventType()) {
    case SapManager::EventServerConnected:
        camera->cam_connected(server_name);
        camera->StartAcquisition();
        break;
    case SapManager::EventServerDisconnected:
        camera->cam_disconnected(server_name);
        camera->StopAcquisition();
        break;
    }
}

void Camera::XferCallback(SapXferCallbackInfo* info) {
    auto camera = (Camera*) info->GetContext();

    if (info->IsTrash()) {
        std::cout << "Camera " << camera->name << ": Frame ended up in trash buffer" << std::endl;
		return;
	}

	int index = camera->buffer->GetIndex();
	void* address;
	bool success = camera->buffer->GetAddress(index, &address);
    
    if (!success) {
        std::cout << "[Dalsa VideoSource] Failed to get buffer address" << std::endl;
        return;
    }
	
    camera->frame_arrived(camera->buffer->GetIndex(), address);
}

void Camera::ErrorCallback(SapManCallbackInfo* info) {
	std::cout << "[Dalsa VideoSource] Error (" << info->GetErrorValue() << "): "
		<< info->GetErrorMessage() << std::endl;
}
