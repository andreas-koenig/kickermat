#pragma once

#ifdef DALSAVIDEOSOURCE_EXPORTS
#define DALSA_API __declspec(dllexport)
#else
#define DALSA_API __declspec(dllimport)
#endif

extern "C" {
	DALSA_API void start_acquisition(void __stdcall callback(int index, void* address));
	DALSA_API void stop_acquisition();
	DALSA_API void release_buffer(int index);
}
