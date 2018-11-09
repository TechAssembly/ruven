import { Client } from "colyseus";
import { LobbyRoom } from "./LobbyRoom";
import { TeamDeathMatchRoomState } from "./LobbyRoomState";

export interface ITeam {
  id: number;
  players: string[];
}

export const NUMBER_OF_TEAMS: number = 2;

export class TeamDeathMatch extends LobbyRoom<TeamDeathMatchRoomState> {
  mode = "TD";

  onInit(options: any): void {
    this.setState({
      players: {},
      teams: [{ id: 1, players: [] }, { id: 2, players: [] }],
    });
  }

  /*-------------------------------------------------------------------
    Teams should be balanced in terms of amounts.
    Allow a team to have up to one more player than the other team.
    Teams are sorted by amount of players
  --------------------------------------------------------------------*/
  onJoin(client: Client): void {
    /*creating new player and adding them to players list*/
    /*team with lowest number of players is first*/
    let chosenTeam: ITeam = this.state.teams[0];
    /*if all teams are equal in number of players add the player randomly to a team*/
    if (this.state.teams[0].players.length === this.state.teams[this.state.teams.length - 1].players.length) {
      chosenTeam = this.randomTeam();
    }
    this.state.addPlayer(client.sessionId, chosenTeam);
  }

  onLeave(client: Client): void {
    this.state.removePlayer(client.sessionId, this.state.players[client.sessionId].teamId);
  }

  onMessage(client: Client, data: any): void {
    return;
  }

  /*-------------------------------------------------------------------
    Teams should be balanced in terms of amounts.
    Allow a team to have up to one more player than the other team.
    Teams are sorted by amount of players
  --------------------------------------------------------------------*/
  onChangeTeam(client: Client, newTeamNumber: number): void {
    const newTeamIndex: number = this.state.teams.findIndex((team: ITeam) => team.id === newTeamNumber);
    if (this.state.teams[newTeamIndex].players.length > this.state.teams[0].players.length + 1) {
      return;
    }
    this.state.changePlayerTeam(client.sessionId, newTeamNumber);
  }

  onReady(client: Client): void {
    this.state.readyPlayer(client.sessionId);
  }

  randomTeam(): ITeam {
    return this.state.teams[Math.floor(Math.random() * this.state.teams.length)];
  }
}
