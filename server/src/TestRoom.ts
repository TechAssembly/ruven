import { Room, Client } from "colyseus";

export class BattleRoom extends Room {

    onInit(options: any) {
        this.setState({
            players: {}
        });
    }

    onJoin(client: Client) {
        this.state.players[client.sessionId] = {
            x: 0,
            y: 0
        };
    }

    onLeave(client: Client) {
        delete this.state.players[client.sessionId];
    }

    onMessage(client: Client, data: any) {
        if (data.action === "left") {
            this.state.players[client.sessionId].x -= 1;
        } else if (data.action === "right") {
            this.state.players[client.sessionId].x += 1;
        }
    }
}