import { Room, Client, EntityMap } from '@techassembly/colyseus';
import { GameState } from './GameState';
import { GameRoomMessage } from './Messages';
import { LobbyPlayer } from '../GameLobbyRoom/LobbyRoomState';
import { Player } from './Player';
import { debugLobbies } from '../../loggers';

export abstract class GameRoom<S extends GameState<P>, P extends Player = Player> extends Room<S> {
  abstract initialGameState(): S;

  initialPlayerData: EntityMap<LobbyPlayer> | null = {};

  onInit ({ initialPlayerData, maxClients }: any) {
    this.setState(this.initialGameState());
    this.maxClients = maxClients;
    this.initialPlayerData = initialPlayerData.players;
    debugLobbies('Creating lobby with initial data: %O', this.initialPlayerData);
  }

  onJoin(client: Client) {
    if (this.initialPlayerData === null) {
      debugLobbies(
        'Client %o trying to join the lobby although all clients have already connected',
        client.id);
      return;
    }

    debugLobbies(
      'initial player data for player %o is: %O', client.id, this.initialPlayerData[client.id]);
    this.state.addPlayer(client, this.initialPlayerData[client.id]);
    delete this.initialPlayerData[client.id];
    this.checkIfAllPlayersJoined();
  }

  private checkIfAllPlayersJoined() {
    if (this.initialPlayerData === null) {
      return;
    }

    if (Object.keys(this.initialPlayerData).length === 0) {
      this.broadcast('all_players_in_game');
      this.initialPlayerData = null;
    }
  }

  onLeave(client: Client) {
    this.state.removePlayer(client);
  }

  onMessage(client: Client, message: GameRoomMessage) {
    switch (message.action) {
      case 'gameStateChange':
        this.state.changePlayerGameState(client, message);
        break;
    }
  }
}
