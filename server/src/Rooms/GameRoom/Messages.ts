import { PlayerGameState } from './Player';

export interface BaseMessage {
  action: string;
}

export interface PlayerGameStateMessage extends BaseMessage {
  action: 'gameStateChange';
  data: PlayerGameState;
}

export type GameRoomMessage =
    | PlayerGameStateMessage;
