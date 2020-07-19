const API_BASE = 'http://localhost:5001';
export const REST_BASE = API_BASE + '/api';
export const SIGNALR_BASE = API_BASE + '/signalr';
export const CAMERA = REST_BASE + '/camera';
export const VIDEO_URL = REST_BASE + '/ui/video';

// SignalR Hubs
export const HUB_CALIBRATION = SIGNALR_BASE + '/calibration';
export const HUB_CALIBRATION_START = 'startCalibration';
