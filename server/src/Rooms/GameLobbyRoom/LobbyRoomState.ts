import { EntityMap, Client } from '@techassembly/colyseus';
import { debugLobbies } from '../../loggers';
import { randomElement } from '../../utils';

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
    public ready = false,
  ) {
    debugLobbies('Player recieved, options: %O', client.options);
    this.id = client.id;
    this.name = client.options && client.options.name || DEBUG_generatePlayerId();
  }
}

export abstract class LobbyRoomState<P extends LobbyPlayer = LobbyPlayer> {
  players: EntityMap<P> = {};
  roomOwner: string | null = null;

  protected abstract createNewPlayer(client: Client): P;

  addPlayer(client: Client): P {
    if (this.roomOwner === null) {
      this.roomOwner = client.id;
      debugLobbies('Room has no owner, setting %o as owner', this.roomOwner);
    }

    const player = this.createNewPlayer(client);
    this.players[client.id] = player;
    return player;
  }

  protected playerByClient(client: Client): P {
    return this.players[client.id];
  }

  removePlayer(client: Client): void {
    delete this.players[client.id];
    const playerIds = Object.keys(this.players);
    if (client.id === this.roomOwner && playerIds.length > 0) {
      this.roomOwner = randomElement(playerIds);
      debugLobbies('Owner %o left the room, setting %o as owner', client.id, this.roomOwner);
    }
  }

  readyPlayer(client: Client): void {
    this.players[client.id].ready = true;
  }
}
