import { Position } from './Position';
import { PlayerCharactiristics } from './PlayerCharactiristics';
import { BodyParts } from './BodyParts';
import { Essence } from './Essence';

export class Player {
  private static playerCounter: number;

  public score : number;
  public currentKarma: number;
  public currentHealth: number;
  public currentHealthRegeneration: number;
  public currentDamage: number;
  public currentMovementSpeed: number;
  public currentArmor: number;
  public essence: number;
  public bodyParts: BodyParts;
  public charactirisrics: PlayerCharactiristics;

  constructor(
    public position: Position = new Position(),
    public rotation = 0,
    public name: string =  `player${Player.playerCounter}`,
    public team: string = 'team ' + `player${Player.playerCounter}`) {
    Player.playerCounter += 1;
    this.score = 0;
    this.currentHealth = 100;
    this.charactirisrics = new PlayerCharactiristics();
    this.bodyParts = new BodyParts('Human');
    this.currentKarma = this.charactirisrics.basicKarma;
    this.currentHealthRegeneration = this.charactirisrics.basicHealthRegeneration;
    this.currentDamage = this.charactirisrics.basicDamage;
    this.currentMovementSpeed = this.charactirisrics.basicMovementSpeed;
    this.currentArmor = this.charactirisrics.basicArmor;
    this.essence = Essence.getEssence('NEAUTRAL');
  }
}
