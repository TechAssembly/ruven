import { Position } from './Position';
import { Rotation } from './Rotation';

export class Player {
  private static playerCounter: number;

  public name: string;

  constructor(
    public position: Position = new Position(),
    public rotation: Rotation = new Rotation(),
  ) {
    this.name = `player${Player.playerCounter}`;
    Player.playerCounter += 1;
  }
}
