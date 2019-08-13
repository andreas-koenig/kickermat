import { VideoSource } from './api.model';

const API_BASE = "http://localhost:5001";
const SIGNALR_BASE = API_BASE + "/signalr";

export const HUB_CALIBRATION = SIGNALR_BASE + '/calibration';
export const HUB_CALIBRATION_START = 'startCalibration';

export interface VideoSourceEndpoint {
  hub: string;
  video: string;
  connected: string;
  disconnected: string;
}
export const VIDEOSOURCE_ENDPOINTS = new Map<VideoSource, VideoSourceEndpoint>();
VIDEOSOURCE_ENDPOINTS.set(VideoSource.CAMERA, {
  hub: SIGNALR_BASE + '/camera',
  video: 'video',
  connected: 'cameraConnected',
  disconnected: 'cameraDisconnected'
});
