import { LobbyPlayer, PlayerState, LobbyRoomState } from '../LobbyRoomState';
import { EntityMap, Client } from "@techassembly/colyseus";
import uuid = require('uuid');
import { TeamLobbyPlayer, Team, sortTeamsByPlayerCount } from './Team';
import { debugErrors } from '../../../loggers';

export class TeamLobbyRoomState extends LobbyRoomState<TeamLobbyPlayer> {
  teams: EntityMap<Team> = {};

  constructor(teamsCount: number = 2) {
    super();
    for (let i = 0; i < teamsCount; i += 1) {
      const id = uuid();
      this.teams[id] = new Team(id);
    }
  }

  private findLeastOccupiedTeam(): Team {
    const sortedByPlayerCount = Object.values(this.teams)
      .slice().sort(sortTeamsByPlayerCount);

    const leastAmountOfPlayers = sortedByPlayerCount[sortedByPlayerCount.length - 1].players.length;

    const teamsWithLessPlayers = sortedByPlayerCount.filter(
      t => t.players.length === leastAmountOfPlayers);

    return teamsWithLessPlayers[Math.floor(Math.random() * leastAmountOfPlayers)];
  }

  addPlayer(client: Client): TeamLobbyPlayer {
    const player = super.addPlayer(client);
    const team = this.findLeastOccupiedTeam();
    team.add(player);
    return player;
  }

  protected createNewPlayer(client: Client): TeamLobbyPlayer {
    return new TeamLobbyPlayer(client);
  }

  removePlayer(client: Client): void {
    const player = this.playerByClient(client);
    if (player.team !== null) {
      const team = this.teams[player.team];
      team.remove(player);
    }
    super.removePlayer(client);
  }

  public changePlayerTeam(client: Client, requestedTeamId: string): void {
    const requestedTeam = this.teams[requestedTeamId];
    if (!requestedTeam) {
      debugErrors(`Requested team (${requestedTeamId}) doesn\'t exist`);
      return;
    }

    const player = this.playerByClient(client);
    if (player.team !== null) {
      const currentTeam = this.teams[player.team];
      currentTeam.remove(player);
    }
    requestedTeam.add(player);
  }
}
