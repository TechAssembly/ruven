import { Server } from 'colyseus';
import { createServer } from 'http';
import express, { Request, Response } from 'express';

import { GameRoom } from './Rooms/GameRoom/GameRoom';

const app: express.Application = express();
const port: number = Number(process.env.PORT) || 3000;

app.get('/', (req: Request, res: Response) => {
  res.send('Game is up');
});

const gameServer = new Server({
  server: createServer(app),
});

gameServer.register('game', GameRoom);
gameServer.matchMaker.create('game', {});
gameServer.listen(port, undefined, undefined, () => {
  console.log('Server is listening on port', port);
});
