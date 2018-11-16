import { Position } from './Position';

export class Player {
  private static playerCounter: number = 0;

  public name: string;

  constructor(
    public position: Position = new Position(),
    public rotation = 0,
  ) {
    this.name = `ruven#${Player.playerCounter}`;
    Player.playerCounter += 1;
  }
}
