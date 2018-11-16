import { Position } from './Position';
import { PlayerStats } from './PlayerStats';
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
  public stats: PlayerStats;

  constructor(
    public position: Position = new Position(),
    public rotation = 0,
    public name: string =  `player${Player.playerCounter}`,
    public team: string = 'team ' + `player${Player.playerCounter}`) {
    Player.playerCounter += 1;
    this.score = 0;
    this.currentHealth = 100;
    this.stats = new PlayerStats();
    this.bodyParts = new BodyParts('Lion');
    this.currentKarma = this.stats.basicKarma;
    this.currentHealthRegeneration = this.stats.basicHealthRegeneration;
    this.currentDamage = this.stats.basicDamage;
    this.currentMovementSpeed = this.stats.basicMovementSpeed;
    this.currentArmor = this.stats.basicArmor;
    this.essence = Essence.getEssence('NEUTRAL');
  }
}
