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

CameraController* CameraController::getInstance(void __stdcall callback(int index, void* address)) {
	CameraController* ctrl = CameraController::getInstance();
	ctrl->callback = callback;
	return ctrl;
}

void CameraController::releaseInstance() {
	if (INSTANCE != nullptr) {
		delete INSTANCE;
	}
	INSTANCE = nullptr;
}

CameraController::CameraController() {
	SapManager::SetDisplayStatusMode(SapManager::StatusCallback, ErrorCallback);

	callback = nullptr;
	acquisitionDevice = new SapAcqDevice("Nano-C1280_1", FALSE);
	buffer = new SapBufferWithTrash(5, acquisitionDevice);
	transfer = new SapAcqDeviceToBuf(acquisitionDevice, buffer, XferCallback);
	transfer->SetAutoEmpty(false);

	acquisitionDevice->Create();
	buffer->Create();
	transfer->Create();
	transfer->Grab();
}

CameraController::~CameraController() {
	SapManager::UnregisterServerCallback();

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
}

void XferCallback(SapXferCallbackInfo* info) {
	if (info->IsTrash()) {
		return;
	}
	
	CameraController* ctrl = CameraController::getInstance();
	SapBufferWithTrash* buffer = ctrl->buffer;
	int index = buffer->GetIndex();
	void* address;
	buffer->GetAddress(index, &address);
	
    ctrl->callback(buffer->GetIndex(), address);
}

void ErrorCallback(SapManCallbackInfo* info) {
	std::cout << "[Dalsa VideoSource] Error (" << info->GetErrorValue() << "): "
		<< info->GetErrorMessage() << std::endl;
}
