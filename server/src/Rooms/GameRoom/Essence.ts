import * as jsonData from '../../database/Essences.json';

export class Essence {
  public static getEssence(key: string): number {
    return (<any>jsonData).Essence[key];
  }
}
