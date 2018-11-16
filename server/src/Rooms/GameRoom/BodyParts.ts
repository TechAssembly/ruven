import { Part } from './Part';

export class BodyParts {
  public head : Part;
  public torso : Part;
  public legs : Part;

  constructor(public characterName: string) {
    this.head = new Part(characterName, 'Head');
    this.torso = new Part(characterName, 'Torso');
    this.legs = new Part(characterName, 'Legs');
  }
}
