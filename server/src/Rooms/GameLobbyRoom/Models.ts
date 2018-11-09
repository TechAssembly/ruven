export enum PlayerState {
  offline,
  waiting,
  ready,
}

export interface ILobbyPlayer {
  id: string;
  state: PlayerState;
}

export interface ITeamLobbyPlayer extends ILobbyPlayer {
  teamId: number;
}

export const defaultLobbyPlayer: (id: string) => ILobbyPlayer = (id: string) => ({
  id,
  state: PlayerState.waiting,
});

export const defaultTeamLobbyPlayer: (id: string, teamId: number) => ITeamLobbyPlayer = (
  id: string,
  teamId: number
) => ({
  id,
  state: PlayerState.waiting,
  teamId,
});
