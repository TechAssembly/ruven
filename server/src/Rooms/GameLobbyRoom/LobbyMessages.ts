export interface ReadyMessage {
  action: 'ready';
}

export interface StartMessage {
  action: 'start';
}

export type LobbyMessages =
  | StartMessage
  | ReadyMessage;
