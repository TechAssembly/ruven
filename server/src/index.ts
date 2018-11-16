import { Server, RegisteredHandler } from '@techassembly/colyseus';
import { createServer } from 'http';
import express, { Request, Response } from 'express';
import cors from 'cors';

import { GameRoom } from './Rooms/GameRoom/GameRoom';
import { FreeForAllLobbyRoom, TeamLobbyRoom } from './Rooms/GameLobbyRoom';
import { ruvenDebug, debugErrors } from './loggers';
import { subscribeToGameStart } from './Rooms/GameLobbyRoom/LobbyRoom';
import { FreeForAllGameRoom } from './Rooms/GameRoom/FreeForAllGameRoom';

const app: express.Application = express();
const port: number = Number(process.env.PORT) || 3000;

app.use(cors());

app.get('/', (req: Request, res: Response) => {
  res.send('Game is up');
});

const gameServer = new Server({
  server: createServer(app),
});

const handleGameLobbyEvents = (handler: RegisteredHandler) => {
  handler.on('create', room => subscribeToGameStart(room, gameServer.matchMaker));
};

async function main() {
  handleGameLobbyEvents(await gameServer.register(
    'free_for_all_lobby',
    FreeForAllLobbyRoom,
    { gameRoomName: 'free_for_all_game' }));
  handleGameLobbyEvents(await gameServer.register(
    'team_deathmatch_lobby',
    TeamLobbyRoom,
    { gameRoomName: 'team_deathmatch_game' }));

  await gameServer.register('free_for_all_game', FreeForAllGameRoom);

  gameServer.matchMaker.create('game', {});
  gameServer.matchMaker.create('free_for_all_lobby', {});
  gameServer.matchMaker.create('team_deathmatch_lobby', {});

  // app.use('/colyseus', monitor(gameServer));

  gameServer.listen(port, undefined, undefined, () => {
    ruvenDebug('Server is listening on port %d', port);
  });
}

main().catch(e => debugErrors('Something went wrong: %O', e));
