import { StartMessage, ReadyMessage } from '../LobbyMessages';

export interface ChangeTeamMessage {
  action: 'change_team';
  requestedTeamId: string;
}

export type TeamLobbyMessages =
  | StartMessage
  | ReadyMessage
  | ChangeTeamMessage;
