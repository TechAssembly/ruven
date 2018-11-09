import { Server, RegisteredHandler } from 'colyseus';
import { createServer } from 'http';
import express, { Request, Response } from 'express';
import { monitor } from '@colyseus/monitor';
import cors from 'cors';

import { GameRoom } from './Rooms/GameRoom/GameRoom';
import { FreeForAllLobbyRoom, TeamLobbyRoom } from './Rooms/GameLobbyRoom';

const app: express.Application = express();
const port: number = Number(process.env.PORT) || 3000;

app.use(cors());

app.get('/', (req: Request, res: Response) => {
  res.send('Game is up');
});

const gameServer = new Server({
  server: createServer(app),
});

const logRoomEvents = (handler: RegisteredHandler) => {
  handler
    .on('join', (room, client) => console.log(handler.klass.name, client.id, 'joined', room.roomId))
    .on('leave', (room, client) => console.log(handler.klass.name, client.id, 'left', room.roomId));
};

gameServer.register('game', GameRoom).then(logRoomEvents);
gameServer.register('free_for_all_lobby', FreeForAllLobbyRoom).then(logRoomEvents);
gameServer.register('team_deathmatch_lobby', TeamLobbyRoom).then(logRoomEvents);

gameServer.matchMaker.create('game', {});
gameServer.matchMaker.create('free_for_all_lobby', {});
gameServer.matchMaker.create('team_deathmatch_lobby', {});

app.use('/colyseus', monitor(gameServer));

gameServer.listen(port, undefined, undefined, () => {
  console.log('Server is listening on port', port);
});
