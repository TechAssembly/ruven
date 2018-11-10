import { EntityMap, Client } from '@techassembly/colyseus';
import { MoveMessage } from './Messages';
import { Player } from './Player';

export class GameState {
  players: EntityMap<Player> = {};

  addPlayer(client: Client) {
    this.players[client.sessionId] = new Player();
  }

  removePlayer(client: Client) {
    delete this.players[client.sessionId];
  }

  movePlayer(client: Client, { position, rotation }: MoveMessage) {
    const player = this.players[client.sessionId];
    player.rotation = rotation;
    player.position = position;
  }
}
