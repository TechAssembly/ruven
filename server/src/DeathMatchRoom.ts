import { Client } from "colyseus";
import { LobbyRoom } from "./LobbyRoom";
import { DeathMatchRoomState } from "./LobbyRoomState";

export class DeathMatch extends LobbyRoom<DeathMatchRoomState> {
  mode = "FFA";

  onJoin(client: Client): void {
    this.state.addPlayer(client.sessionId);
  }

  onLeave(client: Client): void {
    this.state.removePlayer(client.sessionId);
  }
}
