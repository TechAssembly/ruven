import { Client, Room } from 'colyseus';
import { LobbyRoomState, LobbyPlayer, PlayerState } from './LobbyRoomState';

export const MAX_PLAYERS_IN_ROOM = 16;

export abstract class LobbyRoom<
  S extends LobbyRoomState<P>, P extends LobbyPlayer = LobbyPlayer> extends Room<S> {

  public get ready(): boolean {
    return Object.values(this.state.players).every(p => p.state === PlayerState.Ready);
  }

  onInit(options: any): void {
    this.setState(this.initialPlayerState());
    this.maxClients = options.maxClients || MAX_PLAYERS_IN_ROOM;
  }

  protected abstract initialPlayerState(data?: any): S;

  onJoin(client: Client): void {
    this.state.addPlayer(client);
  }

  onLeave(client: Client): void {
    this.state.removePlayer(client);
  }

  onReady(client: Client): void {
    this.state.readyPlayer(client);
  }
}
