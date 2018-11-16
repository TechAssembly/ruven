import { Room, Client, EntityMap } from '@techassembly/colyseus';
import { GameState } from './GameState';
import { GameRoomMessage } from './Messages';
import { LobbyPlayer } from '../GameLobbyRoom/LobbyRoomState';
import { Player } from './Player';

export abstract class GameRoom<S extends GameState<P>, P extends Player = Player> extends Room<S> {
  constructor() {
    super();
  }

  abstract initialGameState(): S;

  initialPlayerData: EntityMap<LobbyPlayer> = {};

  onInit ({ initialPlayerData, maxClients }: any) {
    this.setState(this.initialGameState());
    this.maxClients = maxClients;
    this.initialPlayerData = initialPlayerData.players;
  }

  onJoin(client: Client) {
    this.state.addPlayer(client, this.initialPlayerData[client.sessionId]);
    delete this.initialPlayerData[client.sessionId];
    this.checkIfAllPlayersJoined();
  }

  private checkIfAllPlayersJoined() {
    if (Object.keys(this.initialPlayerData).length === 0) {
      this.broadcast('all_players_in_game');
    }
  }

  onLeave(client: Client) {
    this.state.removePlayer(client);
  }

  onMessage(client: Client, message: GameRoomMessage) {
    switch (message.action) {
      case 'gameStateChange':
        this.state.changePlayerGameState(client, message);
    }
  }
}
