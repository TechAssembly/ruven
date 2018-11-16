export class PlayerCharactiristics{
  constructor(public basicKarma : number = 0,
              public basicHealthRegeneration : number = 0.5,
              public basicDamage : number = 10,
              public basicMovementSpeed : number = 25,
              public basicArmor : number = 0,
              public maxMovementSpeed: number = 250,
              public maxArmor: number = 100,
              public maxKarma: number = 150) {}
}
