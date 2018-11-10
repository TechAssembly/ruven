import { Client, Room } from "@techassembly/colyseus";
import { LobbyRoomState, LobbyPlayer, PlayerState } from './LobbyRoomState';
import { debugLobbies } from '../../loggers';

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
    debugLobbies('Player %o joined lobby %o', client.sessionId, this.roomId);
    this.state.addPlayer(client);
  }

  onLeave(client: Client): void {
    debugLobbies('Player %o left lobby %o', client.sessionId, this.roomId);
    this.state.removePlayer(client);
  }

  onReady(client: Client): void {
    debugLobbies('Player %o marked as ready in lobby %o', client.sessionId, this.roomId);
    this.state.readyPlayer(client);
  }
}
