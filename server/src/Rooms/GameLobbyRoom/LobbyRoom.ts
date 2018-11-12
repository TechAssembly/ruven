import { Client, Room } from '@techassembly/colyseus';
import { LobbyRoomState, LobbyPlayer } from './LobbyRoomState';
import { debugLobbies } from '../../loggers';
import { MatchMaker } from '@techassembly/colyseus/lib/MatchMaker';

export const MAX_PLAYERS_IN_ROOM = 16;

export abstract class LobbyRoom<
  S extends LobbyRoomState<P>, P extends LobbyPlayer = LobbyPlayer> extends Room<S> {

  public allPlayersReady(): boolean {
    return Object.values(this.state.players).every(p => p.ready);
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

  onStart({ sessionId }: Client): void {
    if (sessionId !== this.state.roomOwner) {
      debugLobbies('Player %o not owner but sent %o message', sessionId, 'start');
      return;
    }

    if (!this.allPlayersReady()) {
      debugLobbies(
        'Owner %o sent %o message but not all players are ready',
        this.state.roomOwner, 'start');
      return;
    }

    debugLobbies('Lobby %o is ready, starting game', this.roomId);
    this.emit('start_game', 'game');
  }
}

export const subscribeToGameStart = (room: Room, matchMaker: MatchMaker) => {
  debugLobbies('Room %o created, registering to start_game', room.roomId);
  room.once('start_game', (roomName: string) => {
    const gameRoomId = matchMaker.create(roomName, { create: true, maxClients: room.clients });
    debugLobbies('Received start_game for %o, created room %o', room.roomId, gameRoomId);
    room.broadcast({ action: 'join_game', roomId: gameRoomId });
  });
};
