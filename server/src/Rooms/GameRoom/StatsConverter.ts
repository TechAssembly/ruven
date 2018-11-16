import * as data from  '../../database/Characters.json';
import { StatsAdditions } from './StatsAdditions';
import { BodyPart } from './BodyParts.js';

export class StatsConverter {
  public static getStats(name: string = 'Human', part: BodyPart) : StatsAdditions {
    const databasePart = (<any>data)[name][part].Stats || {};
    const health = StatsConverter.filterState(databasePart.HP);
    const healthRegeneration = StatsConverter.filterState(databasePart.HPR);
    const armor = StatsConverter.filterState(databasePart.AR);
    const baseDamage = StatsConverter.filterState(databasePart.BD);
    const karma = StatsConverter.filterState(databasePart.CK);
    const movementSpeed = StatsConverter.filterState(databasePart.MS);
    return new StatsAdditions(
      health, healthRegeneration, armor, baseDamage, karma, movementSpeed);
  }

  private static filterState = (state: any): number => state || 0;
}
