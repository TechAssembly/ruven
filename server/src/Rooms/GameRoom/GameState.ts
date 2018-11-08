import { EntityMap, Client } from "colyseus";
import Player from "./Player";
import { Direction } from "./Direction";

export class GameState {
  players: EntityMap<Player> = {};

  addPlayer(client: Client) {
    this.players[client.sessionId] = new Player();
  }

  removePlayer(client: Client) {
    delete this.players[client.sessionId];
  }

  movePlayer(client: Client, direction: Direction) {
    const player: Player = this.players[client.sessionId];
    switch (direction) {
      case Direction.LEFT:
        player.position.x -= 1;
        break;
      case Direction.RIGHT:
        player.position.x += 1;
        break;
      case Direction.FORWARD:
        player.position.y += 1;
        break;
      case Direction.BACKWARD:
        player.position.y -= 1;
        break;
    }
  }
}
