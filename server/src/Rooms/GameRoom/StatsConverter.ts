import * as data from  '../../database/Characters.json';
import { StatsAdditions } from './StatsAdditions';

export class StatsConverter {
  public static getStats (name : string = 'Human', part : string) : StatsAdditions {
    const databasePart = data[name][part];
    const health = StatsConverter.filterState(databasePart['HP']);
    const healthRegeneration = StatsConverter.filterState(databasePart['HRP']);
    const armor = StatsConverter.filterState(databasePart['AR']);
    const baseDamage = StatsConverter.filterState(databasePart['BD']);
    const karma = StatsConverter.filterState(databasePart['CK']);
    const movementSpeed = StatsConverter.filterState(databasePart['MS']);
    return new StatsAdditions(health, healthRegeneration, armor,
                              baseDamage, karma, movementSpeed);
  }

  private static filterState(state : any) : number {
    if (state === 'undefined') {
      return 0;
    }
    return state;
  }
}
