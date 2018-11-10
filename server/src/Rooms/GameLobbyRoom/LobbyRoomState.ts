import { EntityMap, Client } from "@techassembly/colyseus";
import { debugLobbies } from '../../loggers';

export enum PlayerState {
  Waiting,
  Ready,
}

// tslint:disable-next-line:variable-name
let DBEUG_newPlayerId = 0;
// tslint:disable-next-line:variable-name
const DEBUG_generatePlayerId = () => {
  DBEUG_newPlayerId += 1;
  return `ruven#${DBEUG_newPlayerId}`;
};

export class LobbyPlayer {
  public name: string;
  public id: string;

  constructor(
    client: Client,
    public state: PlayerState = PlayerState.Waiting,
  ) {
    this.id = client.sessionId;
    this.name = client.options && client.options.name || DEBUG_generatePlayerId();
  }
}

export abstract class LobbyRoomState<P extends LobbyPlayer = LobbyPlayer> {
  players: EntityMap<P> = {};

  protected abstract createNewPlayer(client: Client): P;

  addPlayer(client: Client): P {
    const player = this.createNewPlayer(client);
    this.players[client.sessionId] = player;
    return player;
  }

  protected playerByClient(client: Client): P {
    return this.players[client.sessionId];
  }

  removePlayer(client: Client): void {
    delete this.players[client.sessionId];
  }

  readyPlayer(client: Client): void {
    this.players[client.sessionId].state = PlayerState.Ready;
  }
}
