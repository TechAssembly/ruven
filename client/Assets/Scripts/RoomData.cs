using Colyseus;

public class RoomData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Mode { get; set; }
    public int CurrentClients { get; set; }
    public int MaxClients { get; set; }

    public static RoomData FromColyseusRoom(string mode, RoomAvailable room)
    {
        return new RoomData
        {
            Id = room.roomId,
            MaxClients = (int)room.maxClients,
            CurrentClients = (int)room.clients,
            Name = "Room " + room.roomId,
            Mode = mode,
        };
    }
}