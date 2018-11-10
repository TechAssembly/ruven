using System.Collections.Generic;
using Colyseus;

public class RoomData
{
    public enum GameMode
    {
        TeamDeathmatch = 0,
        FreeForAll = 1,
    }

    public static string FormatGameMode(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.FreeForAll:
                return "Free For All";
            case GameMode.TeamDeathmatch:
                return "Team Deathmatch";
            default:
                return "Unknown - There be bugs";
        }
    }

    public static readonly Dictionary<GameMode, string> GameModeToLobbyName = new Dictionary<GameMode, string>
    {
        {GameMode.TeamDeathmatch, "team_deathmatch_lobby"},
        {GameMode.FreeForAll, "free_for_all_lobby"},
    };

    public string Id { get; set; }
    public string Name { get; set; }
    public GameMode Mode { get; set; }
    public int CurrentClients { get; set; }
    public int MaxClients { get; set; }

    public static RoomData FromColyseusRoom(GameMode mode, RoomAvailable room)
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