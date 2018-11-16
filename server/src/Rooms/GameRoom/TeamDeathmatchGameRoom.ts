import { GameRoom } from './GameRoom';
import { GameState } from './GameState';
import { Player } from './Player';
import { Client, EntityMap } from '@techassembly/colyseus';
import { Team } from '../GameLobbyRoom/TeamLobby/Team';

export class TeamDeathmatchPlayer extends Player {
  constructor(
    public team: string,
    public id: string,
    public name: string,
  ) {
    super(id, name);
  }
}

export class TeamDeathmatchGameState extends GameState<TeamDeathmatchPlayer> {
  teams: EntityMap<Team> = {};

  createNewPlayer(client: Client, options: any): TeamDeathmatchPlayer {
    return new TeamDeathmatchPlayer(options.team, client.sessionId, options.name);
  }
}

export class TeamDeathmatchGameRoom extends GameRoom<TeamDeathmatchGameState> {
  onInit(options: any) {
    super.onInit(options);
    this.state.teams = options.initialPlayerData.teams;
  }

  initialGameState(): TeamDeathmatchGameState {
    return new TeamDeathmatchGameState();
  }
}
