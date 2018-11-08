import Position from './Position';
import Rotation from './Rotation';

export default class Player {
  private static playerCounter: number;
  name: string;
  constructor(public position: Position = new Position(),
              public rotation: Rotation = new Rotation()) {
    this.name = `player${Player.playerCounter}`;
    Player.playerCounter += 1;
  }
}
