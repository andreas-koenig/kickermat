#pragma once

#ifdef DALSAVIDEOSOURCE_EXPORTS
#define DALSA_API __declspec(dllexport)
#else
#define DALSA_API __declspec(dllimport)
#endif

extern "C" {
    DALSA_API void startup(
        void __stdcall frame_callback(int index, void* address),
        void __stdcall connected_callback(char* server_name),
        void __stdcall disconnected_callback(char* server_name)
    );
    DALSA_API void shutdown();
    DALSA_API void get_available_cameras();
	DALSA_API bool start_acquisition(char* camera_name);
	DALSA_API void stop_acquisition();
	DALSA_API void release_buffer(int index);
    DALSA_API bool get_feat_value(char* camera_name, char* feature_name, double* value);
    DALSA_API bool set_feat_value(char* camera_name, char* feature_name, double value);
}
