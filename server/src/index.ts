import { Server, Room, Client } from "colyseus";
import { createServer } from "http";
import express from "express";

const app: express.Application = express();
const port = Number(process.env.PORT) || 3000;

const gameServer = new Server({
    server: createServer(app)
});

class ChatRoom extends Room {
    // maximum number of clients per active session
    maxClients = 4;

    onInit() {
        this.setState({ messages: [] });
    }
    onJoin(client: Client) {
        this.state.messages.push(`${client.sessionId} joined.`);
    }
    onMessage(client: Client, data: any) {
        this.state.messages.push(data);
    }
}

// Register ChatRoom as "chat"
gameServer.register("chat", ChatRoom);
gameServer.listen(port);