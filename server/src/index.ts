import { Server, RegisteredHandler } from "@techassembly/colyseus";
import { createServer } from 'http';
import express, { Request, Response } from 'express';
import cors from 'cors';

import { GameRoom } from './Rooms/GameRoom/GameRoom';
import { FreeForAllLobbyRoom, TeamLobbyRoom } from './Rooms/GameLobbyRoom';
import { ruvenDebug } from './loggers';

const app: express.Application = express();
const port: number = Number(process.env.PORT) || 3000;

app.use(cors());

app.get('/', (req: Request, res: Response) => {
  res.send('Game is up');
});

const gameServer = new Server({
  server: createServer(app),
});

gameServer.register('game', GameRoom);
gameServer.register('free_for_all_lobby', FreeForAllLobbyRoom);
gameServer.register('team_deathmatch_lobby', TeamLobbyRoom);

gameServer.matchMaker.create('game', {});
gameServer.matchMaker.create('free_for_all_lobby', {});
gameServer.matchMaker.create('team_deathmatch_lobby', {});

//app.use('/colyseus', monitor(gameServer));

gameServer.listen(port, undefined, undefined, () => {
  ruvenDebug('Server is listening on port %d', port);
});
