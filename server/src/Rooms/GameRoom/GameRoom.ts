import { Room, Client } from 'colyseus';
import { GameState } from './GameState';
import { GameRoomMessage } from './Messages';

export class GameRoom extends Room<GameState> {
  constructor() {
    super();
  }

  onInit (options: any) {
    this.maxClients = 16;
    this.setState(new GameState());
  }

  onJoin(client: Client) {
    this.state.addPlayer(client);
  }

  onLeave(client: Client) {
    this.state.removePlayer(client);
  }

  onMessage(client: Client, message: GameRoomMessage) {
    switch (message.action) {
      case 'move':
        this.state.movePlayer(client, message);
    }
  }
}
