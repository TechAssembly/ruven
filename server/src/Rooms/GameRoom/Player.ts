import { Position } from './Position';

export class Player {
  private static playerCounter: number;

  public name: string;

  constructor(
    public position: Position = new Position(),
    public rotation = 0,
  ) {
    this.name = `player${Player.playerCounter}`;
    Player.playerCounter += 1;
  }
}
