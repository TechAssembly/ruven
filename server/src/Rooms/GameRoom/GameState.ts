import { EntityMap, Client } from 'colyseus';
import { MoveMessage } from './Messages';
import { Player } from './Player';
import { Direction } from './Direction';

export class GameState {
  players: EntityMap<Player> = {};

  addPlayer(client: Client) {
    this.players[client.sessionId] = new Player();
  }

  removePlayer(client: Client) {
    delete this.players[client.sessionId];
  }

  movePlayer(client: Client, { direction, rotation }: MoveMessage) {
    const player = this.players[client.sessionId];
    player.rotation = rotation;
    this.updatePlayerDirection(player, direction);
  }

  private updatePlayerDirection(player: Player, direction: Direction) {
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
      default:
        console.log('DELETE ME WHEN USE DEBUG', 'Got invalid direction', direction);
    }
  }
}
