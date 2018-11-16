import { StatsAdditions } from './StatsAdditions';
import { StatsConverter } from './StatsConverter';

export class Part{
  public stats: StatsAdditions;

  constructor(public characterName: string, public part : string) {
    this.stats = StatsConverter.getStats(characterName, part);
  }
}
