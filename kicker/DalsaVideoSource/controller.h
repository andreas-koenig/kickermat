#pragma once

#include "SapClassBasic.h"

const int TRANSFER_WAIT_TIMEOUT = 5000;

class CameraController {
public:
	SapAcqDevice* acquisitionDevice;
	SapBufferWithTrash* buffer;
	SapView* view;
	SapTransfer* transfer;
	
	void (*callback)(int, void*);

	static CameraController* getInstance();
	static CameraController* getInstance(void __stdcall callback(int index, void* address));
	static void releaseInstance();

	~CameraController();
	CameraController(CameraController const&) = delete;
	void operator=(CameraController const&) = delete;

private:
	static CameraController* INSTANCE;
	CameraController();
};

void XferCallback(SapXferCallbackInfo* info);
void ErrorCallback(SapManCallbackInfo* info);
