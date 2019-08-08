const API_BASE = "http://localhost:5001";
const SIGNALR_BASE = API_BASE + "/signalr";

export const HUB_CAMERA = SIGNALR_BASE + '/camera';
export const HUB_CAMERA_VIDEO = 'video';
export const HUB_CAMERA_DISCONNECTED = 'cameraDisconnected';
export const HUB_CAMERA_CONNECTED = 'cameraConnected';

export const HUB_CALIBRATION = SIGNALR_BASE + '/calibration';
export const HUB_CALIBRATION_START = 'startCalibration';
