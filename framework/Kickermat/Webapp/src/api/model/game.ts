import { Entity, Named } from './base';

// Player
export interface KickermatPlayer extends Entity, Named {
  description: string;
  authors: string[];
  emoji: string;
}

// Game
export interface Game {
  state: GameState;
  player?: KickermatPlayer;
}

export enum GameState {
  NoGame = 0,
  Running = 1,
}
