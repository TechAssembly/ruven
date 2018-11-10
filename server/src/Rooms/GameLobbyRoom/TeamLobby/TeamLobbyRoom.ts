import { Client } from "@techassembly/colyseus";
import { LobbyRoom } from '../LobbyRoom';
import { TeamLobbyRoomState } from './TeamLobbyRoomState';
import { TeamLobbyMessages } from './TeamLobbyMessages';

export const DEFAULT_TEAM_COUNT: number = 2;

export class TeamLobbyRoom extends LobbyRoom<TeamLobbyRoomState> {

  protected initialPlayerState(data?: any): TeamLobbyRoomState {
    const teamCount = data && data.roomCount && data.roomCount >= 2
      ? data.roomCount : DEFAULT_TEAM_COUNT;
    return new TeamLobbyRoomState(teamCount);
  }

  onMessage(client: Client, data: TeamLobbyMessages): void {
    switch (data.action) {
      case 'ready':
        this.state.readyPlayer(client);
        break;
      case 'change_team':
        this.state.changePlayerTeam(client, data.requestedTeamId);
        break;
    }
  }
}
