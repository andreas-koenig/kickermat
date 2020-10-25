// User Interface
export enum UserInterface {
  Video = 0,
}

export interface VideoChannel {
  name: string;
  description: string;
}

export interface ChannelsResponse {
  channels: VideoChannel[];
  currentChannel: VideoChannel;
}
