import { Rotation } from './Rotation';
import { Position } from './Position';

export interface BaseMessage {
  action: string;
}

export interface MoveMessage extends BaseMessage {
  action: 'move';
  position: Position;
  rotation: Rotation;
}

export type GameRoomMessage =
    | MoveMessage;
