import { LobbyRoom } from './LobbyRoom';
import { LobbyRoomState, LobbyPlayer } from './LobbyRoomState';
import { Client } from '@techassembly/colyseus';
import { LobbyMessages } from './LobbyMessages';
import { debugErrors } from '../../loggers';

export class FreeForAllRoomState extends LobbyRoomState {
  protected createNewPlayer(client: Client): LobbyPlayer {
    return new LobbyPlayer(client);
  }
}

export class FreeForAllLobbyRoom extends LobbyRoom<FreeForAllRoomState> {
  protected initialPlayerState(): FreeForAllRoomState {
    return new FreeForAllRoomState();
  }

  onMessage(client: Client, data: LobbyMessages): void {
    switch (data.action) {
      case 'ready':
        this.state.readyPlayer(client);
        break;
      case 'start':
        this.onStart(client);
        break;
      default:
        debugErrors(
          'Client %o sent invalid %o action on FFA lobby %o',
          client.sessionId, (<any>data).action, this.roomId);
        break;
    }
  }
}
