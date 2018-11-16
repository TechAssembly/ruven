import * as data from '../../database/Essences.json';

export class Essence {
  public static getEssence(key : string) : number {
    return data.Essence[key];
  }
}
