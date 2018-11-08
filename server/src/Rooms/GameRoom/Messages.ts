import { Direction } from './Direction';
import { Rotation } from './Rotation';

export interface BaseMessage {
  action: string;
}

export interface MoveMessage extends BaseMessage {
  action: 'move';
  direction: Direction;
  rotation: Rotation;
}

export type GameRoomMessage =
    | MoveMessage;
