import { EntityMap } from "colyseus";
import { defaultLobbyPlayer, defaultTeamLobbyPlayer, ILobbyPlayer, ITeamLobbyPlayer, PlayerState } from "./Models";
import { ITeam } from "./TeamDeathMatchRoom";

export type PlayersMap<T extends ILobbyPlayer> = { [id: string]: T };

export abstract class LobbyRoomState<T extends ILobbyPlayer> {
  players: EntityMap<T> = {};

  abstract addPlayer(playerId: string, team?: ITeam): void;

  abstract removePlayer(playerId: string, teamId?: number): void;

  readyPlayer(playerId: string): void {
    this.players[playerId].state = PlayerState.ready;
  }
}

export type RoomState = DeathMatchRoomState | TeamDeathMatchRoomState;

export class DeathMatchRoomState extends LobbyRoomState<ILobbyPlayer> {
  addPlayer(playerId: string): void {
    this.players[playerId] = defaultLobbyPlayer(playerId);
  }

  removePlayer(playerId: string): void {
    delete this.players[playerId];
  }
}

export class TeamDeathMatchRoomState extends LobbyRoomState<ITeamLobbyPlayer> {
  teams: ITeam[] = [];

  addPlayer(playerId: string, team: ITeam): void {
    this.players[playerId] = defaultTeamLobbyPlayer(playerId, team.id);
    const requiredTeam: ITeam | undefined = this.teams.find((teamInTeams: ITeam) => teamInTeams.id === team.id);
    if (requiredTeam) {
      requiredTeam.players.push(playerId);
    }
    this.teams.sort((teamA: ITeam, teamB: ITeam) => (teamA.players.length > teamB.players.length ? 1 : -1));
  }

  removePlayer(playerId: string, teamId: number): void {
    this.teams[teamId].players.splice(this.teams[teamId].players.findIndex((id: string) => id === playerId), 1);
    delete this.players[playerId];
    this.teams.sort((teamA: ITeam, teamB: ITeam) => (teamA.players.length > teamB.players.length ? 1 : -1));
  }

  changePlayerTeam(playerId: string, newTeamId: number): void {
    const team: ITeam | undefined = this.teams.find(
      (teamInTeams: ITeam) => teamInTeams.id === this.players[playerId].teamId
    );
    /*remove player from previous team*/
    if (team) {
      const playerIndex: number = team.players.findIndex((id: string) => id === playerId);
      team.players.splice(playerIndex, 1);
    }
    /*add player to new team*/
    const newTeam: ITeam | undefined = this.teams.find((teamInTeams: ITeam) => teamInTeams.id === newTeamId);
    if (newTeam) {
      newTeam.players.push(playerId);
      this.players[playerId].teamId = newTeamId;
    }
    this.teams.sort((teamA: ITeam, teamB: ITeam) => (teamA.players.length > teamB.players.length ? 1 : -1));
  }
}
