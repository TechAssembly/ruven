import { Client, Room } from "colyseus";
import { RoomState } from "./LobbyRoomState";

export abstract class LobbyRoom<T extends RoomState> extends Room<T> {
  mode: string = "";

  onInit(options: any): void {
    this.setState({
      players: {},
    });
  }

  abstract onJoin(client: Client): void;

  abstract onLeave(client: Client): void;

  onMessage(client: Client, data: any): void {
    return;
  }

  onReady(client: Client): void {
    this.state.readyPlayer(client.sessionId);
  }
}
