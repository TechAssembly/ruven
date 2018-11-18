import { Server, RegisteredHandler } from '@techassembly/colyseus';
import { createServer } from 'http';
import express, { Request, Response } from 'express';
import cors from 'cors';

import { FreeForAllLobbyRoom, TeamLobbyRoom } from './Rooms/GameLobbyRoom';
import { ruvenDebug, debugErrors } from './loggers';
import { subscribeToGameStart } from './Rooms/GameLobbyRoom/LobbyRoom';
import { FreeForAllGameRoom } from './Rooms/GameRoom/FreeForAllGameRoom';
import { TeamDeathmatchGameRoom } from './Rooms/GameRoom/TeamDeathmatchGameRoom';
import { RoomConstructor } from '@techassembly/colyseus/lib/Room';

const app: express.Application = express();
const port: number = Number(process.env.PORT) || 3000;

app.use(cors());

app.get('/', (req: Request, res: Response) => {
  res.send('Game is up');
});

const gameServer = new Server({
  server: createServer(app),
});

const handleGameLobbyEvents = ({ lobby }: { lobby: RegisteredHandler }) => {
  lobby.on('create', room => subscribeToGameStart(room, gameServer.matchMaker));
};

interface GameModeRoomNames {
  lobbyName: string;
  gameRoomName: string;
}

const registerRoom = async (
  { lobbyName, gameRoomName }: GameModeRoomNames,
  lobbyKlass: RoomConstructor,
  gameKlass: RoomConstructor,
): Promise<{ lobby: RegisteredHandler, game: RegisteredHandler }> => {
  const lobby = await gameServer.register(lobbyName, lobbyKlass, { gameRoomName });
  const game = await gameServer.register(gameRoomName, gameKlass);
  return { lobby, game };
};

async function main() {
  const FREE_FOR_ALL: GameModeRoomNames = {
    lobbyName: 'free_for_all_lobby',
    gameRoomName: 'free_for_all_game',
  };

  const TEAM_DEATHMATCH: GameModeRoomNames = {
    lobbyName: 'team_deathmatch_lobby',
    gameRoomName: 'team_deathmatch_game',
  };

  handleGameLobbyEvents(await registerRoom(FREE_FOR_ALL, FreeForAllLobbyRoom, FreeForAllGameRoom));
  handleGameLobbyEvents(await registerRoom(TEAM_DEATHMATCH, TeamLobbyRoom, TeamDeathmatchGameRoom));

  gameServer.matchMaker.create(FREE_FOR_ALL.lobbyName, {});
  gameServer.matchMaker.create(TEAM_DEATHMATCH.lobbyName, {});

  // app.use('/colyseus', monitor(gameServer));

  gameServer.listen(port, undefined, undefined, () => {
    ruvenDebug('Server is listening on port %d', port);
  });
}

main().catch(e => debugErrors('Something went wrong: %O', e));
