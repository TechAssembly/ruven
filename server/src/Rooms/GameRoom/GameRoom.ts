import { Room, Client } from 'colyseus';
import { GameState } from './GameState';
import MoveMessage from './Messages/MoveMessage';

export class GameRoom extends Room<GameState> {

  onInit (options: any) {
    this.setState(new GameState());
  }

  onJoin(client: Client) {
    this.state.addPlayer(client);
  }

  onLeave(client: Client) {
    this.state.removePlayer(client);
  }

  onMessage(client: Client, message: MoveMessage) {
    switch (message.action) {
      case 'move':
        this.state.movePlayer(client, message.direction);
    }
  }
}
