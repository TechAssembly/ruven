export interface ReadyMessage {
  action: 'ready';
}

export interface ChangeTeamMessage {
  action: 'change_team';
  requestedTeamId: string;
}

export type TeamLobbyMessages =
  | ChangeTeamMessage
  | ReadyMessage;
