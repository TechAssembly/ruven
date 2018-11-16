import { EntityMap, Client } from '@techassembly/colyseus';
import { PlayerGameStateMessage } from './Messages';
import { Player } from './Player';

export abstract class GameState<P extends Player = Player>{
  players: EntityMap<P> = {};

  abstract createNewPlayer(client: Client, options: object): P;

  addPlayer(client: Client, options: any) {
    this.players[client.sessionId] = this.createNewPlayer(client, options);
  }

  removePlayer(client: Client) {
    delete this.players[client.sessionId];
  }

  changePlayerGameState(client: Client, { playerGameState }: PlayerGameStateMessage) {
    const player = this.players[client.sessionId];
    player.playerGameState = playerGameState;
  }
}
