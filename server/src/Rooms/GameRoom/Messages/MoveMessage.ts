import GameMessage from './GameMessage';
import { Direction } from '../Direction';

export default interface MoveMessage extends GameMessage{
  direction: Direction;
}
