#include "pch.h"
#include <iostream>
#include "camera.h"

Camera::Camera(
    char* camera_name,
    RoiSettings roi,
    void __stdcall frame_callback(int index, void* address),
    void __stdcall connected_callback(char* server_name),
    void __stdcall disconnected_callback(char* server_name)
) {
    name = camera_name;
    this->roi = roi;
    frame_arrived = frame_callback;
    cam_connected = connected_callback;
    cam_disconnected = disconnected_callback;

    SapManager::Open();
    SapManager::SetDisplayStatusMode(SapManager::StatusCallback, Camera::ErrorCallback);
    SapManager::RegisterServerCallback(
        SapManager::EventServerDisconnected | SapManager::EventServerConnected,
        Camera::ServerCallback, this);
    
    device = new SapAcqDevice(name, FALSE);
    trash_buffer = new SapBufferWithTrash(BUFFER_SIZE, device);
    trash_buffer->SetWidth(roi.width);
    trash_buffer->SetHeight(roi.height);
    transfer = new SapAcqDeviceToBuf(device, trash_buffer, XferCallback, this);

    CreateObjects();
}
Camera::~Camera() {
    StopAcquisition();
    DestroyObjects();

    delete name;
    delete device;
    delete trash_buffer;
    delete transfer;

    SapManager::UnregisterServerCallback();
    SapManager::Close();
}

bool Camera::CreateObjects() {
    if (!device->Create()) {
        DestroyObjects();
        return false;
    }
    
    // Must be called after device was created but before transfer
    ConfigureCamera();

    if (!trash_buffer->Create()) {
        DestroyObjects();
        return false;
    }

    if (!transfer->Create()) {
        DestroyObjects();
        return false;
    }
    transfer->SetAutoEmpty(false);

    resources_created = true;
    return true;
}

void Camera::DestroyObjects() {
    if (transfer != nullptr)
        transfer->Destroy();

    if (trash_buffer != nullptr)
        trash_buffer->Destroy();

    if (device != nullptr)
        device->Destroy();

    resources_created = false;
}

bool Camera::StartAcquisition() {
    if (acquisition_running) {
        return false;
    }

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
    if (transfer != nullptr && transfer->IsGrabbing()) {
        success = transfer->Freeze();
        if (!transfer->Wait(TRANSFER_WAIT_TIMEOUT)) {
            success = transfer->Abort();
        };
    }

    acquisition_running = false;
    
    return success;
}

void Camera::ConfigureCamera() {
    device->SetFeatureValue("PixelFormat", "BayerRG8");
    device->SetFeatureValue("autoBrightnessMode", true);
    device->SetFeatureValue("BalanceWhiteAuto", "Periodic");
    device->SetFeatureValue("Width", roi.width);
    device->SetFeatureValue("Height", roi.height);
    device->SetFeatureValue("OffsetX", roi.x_min);
    device->SetFeatureValue("OffsetY", roi.y_min);
    device->SetFeatureValue("acquisitionFrameRateControlMode", "MaximumSpeed");
    device->SetFeatureValue("turboTransferEnable", true);
}

void Camera::ServerCallback(SapManCallbackInfo* info) {
    auto camera = (Camera*) info->GetContext();

    char server_name[64];
    SapManager::GetServerName(info->GetServerIndex(), server_name, sizeof(server_name));

    switch (info->GetEventType()) {
    case SapManager::EventServerConnected:
        camera->cam_connected(server_name);
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
		return;
	}

	int index = camera->trash_buffer->GetIndex();
	void* address;
	bool success = camera->trash_buffer->GetAddress(index, &address);
    
    if (!success) {
        std::cout << "[Dalsa VideoSource] Failed to get buffer address" << std::endl;
        return;
    }
	
    try {
        camera->frame_arrived(camera->trash_buffer->GetIndex(), address);
    }
    catch (std::exception & ex) {
        std::cout << ex.what() << std::endl;
    }
}

void Camera::ErrorCallback(SapManCallbackInfo* info) {
	std::cout << "[Dalsa VideoSource] Error (" << info->GetErrorValue() << "): "
		<< info->GetErrorMessage() << std::endl;
}
