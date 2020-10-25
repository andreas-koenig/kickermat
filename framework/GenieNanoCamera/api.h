#pragma once

#ifdef GENIENANOCAMERA_EXPORTS
#define DALSA_API __declspec(dllexport)
#else
#define DALSA_API __declspec(dllimport)
#endif

#include "camera.h"

extern "C" {
    DALSA_API void* CreateCamera(char* camera_name,
        RoiSettings roi,
        void __stdcall frame_callback(int index, void* address),
        void __stdcall connected_callback(char* server_name),
        void __stdcall disconnected_callback(char* server_name)
    );
    DALSA_API void DestroyCamera(Camera* camera);
    DALSA_API bool StartAcquisition(Camera* camera);
    DALSA_API bool StopAcquisition(Camera* camera);
    DALSA_API bool ReleaseBuffer(Camera* camera, int buffer_index);
    DALSA_API bool SetFeatureValue(Camera* camera, char* feature_name, double feature_value);
}
