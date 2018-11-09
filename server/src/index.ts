import { Server } from 'colyseus';
import { createServer } from 'http';
import express from 'express';

import { GameRoom } from './Rooms/GameRoom/GameRoom';
import { FreeForAllLobbyRoom, TeamLobbyRoom } from './Rooms/GameLobbyRoom';

const app: express.Application = express();
const port: number = Number(process.env.PORT) || 3000;

const gameServer = new Server({
  server: createServer(app),
});

gameServer.register('game', GameRoom);
gameServer.register('free_for_all_lobby', FreeForAllLobbyRoom);
gameServer.register('team_deathmatch_lobby', TeamLobbyRoom);

gameServer.matchMaker.create('game', {});
gameServer.listen(port);
