import { StatsAdditions } from './StatsAdditions';
import { StatsConverter } from './StatsConverter';
import { BodyPart } from './BodyParts';

export class Part{
  public stats: StatsAdditions;

  constructor(public characterName: string, public part: BodyPart) {
    this.stats = StatsConverter.getStats(characterName, part);
  }
}
