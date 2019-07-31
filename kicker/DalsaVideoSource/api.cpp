#include "pch.h"
#include <iostream>

#include "api.h"
#include "controller.h"

void start_acquisition(void __stdcall callback(int index, void* address)) {
	CameraController::getInstance(callback);
	std::cout << "[Dalsa VideoSource] Acquisition started" << std::endl;
}

void stop_acquisition() {
	CameraController::releaseInstance();
	std::cout << "[Dalsa VideoSource] Acquisition stopped" << std::endl;
}

void release_buffer(int index) {
	SapBuffer* buffer = CameraController::getInstance()->buffer;
	buffer->SetState(index, SapBuffer::StateEmpty);
}
