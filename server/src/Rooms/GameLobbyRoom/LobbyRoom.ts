import { Client, Room, EntityMap } from '@techassembly/colyseus';
import { LobbyRoomState, LobbyPlayer } from './LobbyRoomState';
import { debugLobbies } from '../../loggers';
import { MatchMaker } from '@techassembly/colyseus/lib/MatchMaker';

export const MAX_PLAYERS_IN_ROOM = 16;

export abstract class LobbyRoom<
  S extends LobbyRoomState<P>, P extends LobbyPlayer = LobbyPlayer> extends Room<S> {

  gameRoomName: string = 'game';
  incomingClientOptions: EntityMap<any> = {};

  onInit(options: any): void {
    this.setState(this.initialPlayerState());
    this.maxClients = options.maxClients || MAX_PLAYERS_IN_ROOM;
    this.gameRoomName = options.gameRoomName || this.gameRoomName;
  }

  protected abstract initialPlayerState(data?: any): S;

  requestJoin(options: any, isNew?: boolean): number | boolean {
    if (options.sessionId && options.name) {
      this.incomingClientOptions[options.sessionId] = options;
    }
    return true;
  }

  public allPlayersReady(): boolean {
    return Object.values(this.state.players).every(p => p.ready);
  }

  onJoin(client: Client): void {
    debugLobbies('Player %o joined lobby %o', client.sessionId, this.roomId);
    client.options = { ...client.options, ...this.incomingClientOptions[client.sessionId] };
    delete this.incomingClientOptions[client.sessionId];
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
    this.emit('start_game');
  }

  public createGameRoom(matchMaker: MatchMaker) {
    const gameRoomId = matchMaker.create(
      this.gameRoomName, {
        create: true,
        maxClients: this.clients,
        initialPlayerData: this.state,
      });
    debugLobbies(
      'Received start_game for %o, created %o (%o)', this.roomId, this.gameRoomName, gameRoomId);
    this.broadcast({ action: 'join_game', roomId: gameRoomId });
  }
}

export const subscribeToGameStart =
  <S extends LobbyRoomState<P>, P extends LobbyPlayer = LobbyPlayer>(
    room: LobbyRoom<S, P>, matchMaker: MatchMaker) => {
    debugLobbies('Room %o created, registering to start_game', room.roomId);
    room.once('start_game', () => room.createGameRoom(matchMaker));
  };
