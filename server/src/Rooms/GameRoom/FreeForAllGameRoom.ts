import { GameRoom } from './GameRoom';
import { GameState } from './GameState';
import { Player } from './Player';
import { Client } from '@techassembly/colyseus';

export class FreeForAllGameState extends GameState {
  createNewPlayer(client: Client, options: any): Player {
    return new Player(client.sessionId, options.name);
  }
}

export class FreeForAllGameRoom extends GameRoom<FreeForAllGameState> {
  initialGameState(): FreeForAllGameState {
    return new FreeForAllGameState();
  }
}
