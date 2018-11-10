import { Client } from "@techassembly/colyseus";
import { LobbyPlayer, PlayerState } from '../LobbyRoomState';
import { debugTeam, debugErrors } from '../../../loggers';

export class TeamLobbyPlayer extends LobbyPlayer {
  constructor(
    client: Client,
    state: PlayerState = PlayerState.Waiting,
    public team: string | null = null,
  ) {
    super(client, state);
  }
}

export const samePlayer = (player: TeamLobbyPlayer) =>
  (otherPlayer: TeamLobbyPlayer) => player.id === otherPlayer.id;

export class Team {
  constructor(public id: string, public players: TeamLobbyPlayer[] = []) {}

  public contains(player: TeamLobbyPlayer): boolean {
    return this.players.some(samePlayer(player));
  }

  public add(player: TeamLobbyPlayer): void {
    const index = this.players.findIndex(samePlayer(player));
    if (index >= 0) {
      debugErrors('Player %o already in team %o, aborting %o', player.id, this.id, 'add');
      return;
    }

    debugTeam('Player %o added to team %o', player.id, this.id);
    player.team = this.id;
    this.players.push(player);
  }

  public remove(player: TeamLobbyPlayer): boolean {
    const index = this.players.findIndex(samePlayer(player));
    if (index < 0) {
      debugErrors('Player %o already in team %o, aborting %o', player.id, this.id, 'remove');
      return false;
    }

    debugTeam('Player %o removed from team %o', player.id, this.id);
    player.team = null;
    this.players.splice(index, 1);
    return true;
  }
}

export const sortTeamsByPlayerCount = (teamA: Team, teamB: Team) =>
  teamA.players.length > teamB.players.length ? 1 : -1;
