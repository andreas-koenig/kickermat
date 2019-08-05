#include "pch.h"
#include <iostream>
#include "controller.h"

CameraController* CameraController::INSTANCE;

CameraController* CameraController::getInstance()
{
    if (INSTANCE != nullptr) {
        return INSTANCE;
    }

    INSTANCE = new CameraController();
    return INSTANCE;
}

void CameraController::releaseInstance() {
    if (INSTANCE != nullptr) {
        delete INSTANCE;
    }
    INSTANCE = nullptr;
}

CameraController::CameraController() {}
CameraController::~CameraController() {}

void CameraController::start_acquisition(char* camera_name) {
    acquisitionDevice = new SapAcqDevice("Nano-C1280_1", FALSE); // TODO: Use camera_name
    buffer = new SapBufferWithTrash(BUFFER_SIZE, acquisitionDevice);
    transfer = new SapAcqDeviceToBuf(acquisitionDevice, buffer, XferCallback);
    transfer->SetAutoEmpty(false);

    acquisitionDevice->Create();
    buffer->Create();
    transfer->Create();
    transfer->Grab();

    acquisition_running = true;
}

void CameraController::stop_acquisition() {
    if (!acquisition_running) {
        return;
    }
    
    if (transfer->IsGrabbing()) {
        transfer->Freeze();
        if (!transfer->Wait(TRANSFER_WAIT_TIMEOUT)) {
            transfer->Abort();
        };
    }

    transfer->Destroy();
    buffer->Destroy();
    acquisitionDevice->Destroy();

    delete acquisitionDevice;
    delete buffer;
    delete transfer;

    acquisition_running = false;
}

void CameraController::ServerCallback(SapManCallbackInfo* info) {
    CameraController* ctrl = getInstance();
    char server_name[64];
    SapManager::GetServerName(info->GetServerIndex(), server_name, sizeof(server_name));

    switch (info->GetEventType()) {
    case SapManager::EventServerConnected:
        ctrl->cam_connected(server_name);
        ctrl->start_acquisition(server_name);
        break;
    case SapManager::EventServerDisconnected:
        ctrl->cam_disconnected(server_name);
        ctrl->stop_acquisition();
        break;
    }
}

void CameraController::XferCallback(SapXferCallbackInfo* info) {
	if (info->IsTrash()) {
		return;
	}
	
	SapBufferWithTrash* buffer = CameraController::getInstance()->buffer;
	int index = buffer->GetIndex();
	void* address;
	buffer->GetAddress(index, &address);
	
    CameraController::getInstance()->frame_arrived(buffer->GetIndex(), address);
}

void CameraController::ErrorCallback(SapManCallbackInfo* info) {
	std::cout << "[Dalsa VideoSource] Error (" << info->GetErrorValue() << "): "
		<< info->GetErrorMessage() << std::endl;
}
