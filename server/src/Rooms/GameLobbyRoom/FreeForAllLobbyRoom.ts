import { LobbyRoom } from './LobbyRoom';
import { LobbyRoomState, LobbyPlayer } from './LobbyRoomState';
import { Client } from "@techassembly/colyseus";

export class FreeForAllRoomState extends LobbyRoomState {
  protected createNewPlayer(client: Client): LobbyPlayer {
    return new LobbyPlayer(client);
  }
}

export class FreeForAllLobbyRoom extends LobbyRoom<FreeForAllRoomState> {
  protected initialPlayerState(): FreeForAllRoomState {
    return new FreeForAllRoomState();
  }

  onMessage(client: Client, data: {action: string}): void {
    if (data.action === 'ready') {
      this.state.readyPlayer(client);
    }
  }
}
