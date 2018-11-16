import { Part } from './Part';

export enum BodyPart {
  Head = 'Head',
  Torso = 'Torso',
  Legs = 'Legs',
}

export class BodyParts {
  public head: Part;
  public torso: Part;
  public legs: Part;

  constructor(public characterName: string) {
    this.head = new Part(characterName, BodyPart.Head);
    this.torso = new Part(characterName, BodyPart.Torso);
    this.legs = new Part(characterName, BodyPart.Legs);
  }
}
