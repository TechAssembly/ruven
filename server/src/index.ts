import { Server } from 'colyseus';
import { createServer } from 'http';
import express from 'express';

import { GameRoom } from './Rooms/GameRoom/GameRoom';

const app: express.Application = express();
const port: number = Number(process.env.PORT) || 3000;

const gameServer = new Server({
  server: createServer(app),
});

gameServer.register('game', GameRoom);
gameServer.matchMaker.create('game', {});
gameServer.listen(port);
