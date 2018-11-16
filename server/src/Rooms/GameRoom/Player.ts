export enum KeyState {
  Up = 'Up', // Just released
  Down = 'Down', // Just pressed
  Pressed = 'Pressed', // Being pressed
  Released = 'Released', // Not being pressed
}

export interface PlayerGameState {
  xPosition: number;
  yPosition: number;
  zPosition: number;
  rotation: number;
  xInput: number;
  zInput: number;
  leftShiftPressed: KeyState;
}

export class Player {
  constructor(
    public id: string,
    public name: string,
    public playerGameState?: PlayerGameState,
  ) { }
}
